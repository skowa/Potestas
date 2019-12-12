using System;
using System.Collections.Generic;
using System.Linq;
using MongoDB.Bson;
using MongoDB.Driver;
using Potestas.Configuration;
using Potestas.NoSqlPlugin.Extensions;

namespace Potestas.NoSqlPlugin.Analyzers
{
	public class NoSqlAnalyzer : IEnergyObservationAnalizer
	{
		private readonly IMongoCollection<BsonDocument> _collection;

		public NoSqlAnalyzer(IConfiguration configuration, string collectionName)
		{
			string connectionString = configuration.GetValue("connectionStringNoSql");
			string dbName = configuration.GetValue("noSqlDbName");

			var client = new MongoClient(connectionString);
			var db = client.GetDatabase(dbName);
			_collection = db.GetCollection<BsonDocument>(collectionName);
		}

		public IDictionary<double, int> GetDistributionByEnergyValue()
		{
			return _collection.Aggregate()
				.Group(new BsonDocument {{"_id", "$EstimatedValue"}, {"count", new BsonDocument("$sum", 1)}}).ToList()
				.ToDictionary(d => d["_id"].AsDouble, d => d["count"].AsInt32);
		}

		public IDictionary<Coordinates, int> GetDistributionByCoordinates()
		{
			return _collection.Aggregate()
				.Group(new BsonDocument { { "_id", "$ObservationPoint" }, { "count", new BsonDocument("$sum", 1) } }).ToList()
				.ToDictionary(d => ((BsonDocument) d["_id"]).ToCoordinates(), d => d["count"].AsInt32);
		}

		public IDictionary<DateTime, int> GetDistributionByObservationTime()
		{
			return _collection.Aggregate()
				.Group(new BsonDocument { { "_id", "$ObservationTime" }, { "count", new BsonDocument("$sum", 1) } }).ToList()
				.ToDictionary(d => d["_id"].ToLocalTime(), d => d["count"].AsInt32);
		}

		public double GetMaxEnergy()
		{
			return _collection.Find(x => true).SortByDescending(d => d["EstimatedValue"]).First()["EstimatedValue"].AsDouble;
		}

		public double GetMaxEnergy(Coordinates coordinates)
		{
			return _collection.Find(new BsonDocument("ObservationPoint", coordinates.ToBsonDocument()))
				.SortByDescending(d => d["EstimatedValue"]).First()["EstimatedValue"].AsDouble;
		}

		public double GetMaxEnergy(DateTime dateTime)
		{
			return _collection.Find(new BsonDocument("ObservationTime", dateTime))
				.SortByDescending(d => d["EstimatedValue"]).First()["EstimatedValue"].AsDouble;
		}

		public double GetMinEnergy()
		{
			return _collection.Find(x => true).SortBy(d => d["EstimatedValue"]).First()["EstimatedValue"].AsDouble;
		}

		public double GetMinEnergy(Coordinates coordinates)
		{
			return _collection.Find(new BsonDocument("ObservationPoint", coordinates.ToBsonDocument()))
				.SortBy(d => d["EstimatedValue"]).First()["EstimatedValue"].AsDouble;
		}

		public double GetMinEnergy(DateTime dateTime)
		{
			return _collection.Find(new BsonDocument("ObservationTime", dateTime))
				.SortBy(d => d["EstimatedValue"]).First()["EstimatedValue"].AsDouble;
		}

		public double GetAverageEnergy()
		{
			return _collection.Find(_ => true).ToEnumerable().Average(b => b["EstimatedValue"].AsDouble);
		}

		public double GetAverageEnergy(DateTime startFrom, DateTime endBy)
		{
			var builder = Builders<BsonDocument>.Filter;
			var filter = builder.Gte("ObservationTime", startFrom) & builder.Lte("ObservationTime", endBy);
			
			return _collection.Find(filter).ToEnumerable().Average(b => b["EstimatedValue"].AsDouble);
		}

		public double GetAverageEnergy(Coordinates rectTopLeft, Coordinates rectBottomRight)
		{
			var builder = Builders<BsonDocument>.Filter;
			var filter = builder.Gte("ObservationPoint.X", rectTopLeft.X) & builder.Lte("ObservationPoint.Y", rectTopLeft.Y)
				& builder.Gte("ObservationPoint.Y", rectBottomRight.Y) & builder.Lte("ObservationPoint.X", rectBottomRight.X);

				return _collection.Find(filter).ToEnumerable().Average(b => b["EstimatedValue"].AsDouble);
		}

		public DateTime GetMaxEnergyTime()
		{
			return _collection.Find(x => true).SortByDescending(d => d["ObservationTime"]).First()["ObservationTime"].ToLocalTime();

		}

		public Coordinates GetMaxEnergyPosition()
		{
			return (_collection.Find(x => true).SortByDescending(d => d["ObservationPoint"]).First()["ObservationPoint"]
				as BsonDocument).ToCoordinates();
		}

		public DateTime GetMinEnergyTime()
		{
			return _collection.Find(x => true).SortBy(d => d["ObservationTime"]).First()["ObservationTime"].ToLocalTime();
		}

		public Coordinates GetMinEnergyPosition()
		{
			return (_collection.Find(x => true).SortBy(d => d["ObservationPoint"]).First()["ObservationPoint"]
				as BsonDocument).ToCoordinates();
		}
	}
}