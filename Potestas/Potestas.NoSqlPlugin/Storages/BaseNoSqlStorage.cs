using System;
using System.Collections;
using System.Collections.Generic;
using MongoDB.Bson;
using MongoDB.Driver;
using Potestas.Configuration;
using Potestas.Storages;
using Potestas.Utils;

namespace Potestas.NoSqlPlugin.Storages
{
    public abstract class BaseNoSqlStorage<T> : BaseStorage<T>, IEnergyObservationStorage<T>, ICollection<T>, IEnumerable<T>, IEnumerable where T : IEnergyObservation
    {
        private readonly IMongoCollection<BsonDocument> _collection;

        protected BaseNoSqlStorage(IConfiguration configuration)
        {
            string connectionString = configuration.GetValue("connectionStringNoSql");
            string dbName = configuration.GetValue("noSqlDbName");

            var client = new MongoClient(connectionString);
            var db = client.GetDatabase(dbName);
            _collection = db.GetCollection<BsonDocument>(this.CollectionName);
        }

        public IEnumerator<T> GetEnumerator()
        {
	        foreach (var document in _collection.Find(new BsonDocument()).ToList())
	        {
		        yield return this.FromBsonDocument(document);
	        }
        }

        public void Add(T item)
        {
            if (Validator.IsGenericTypeNull(item))
            {
                throw new ArgumentNullException(nameof(item));
            }

            var bsonDocument = this.CreateBsonDocument(item);
            _collection.InsertOne(bsonDocument);
        }

        public void Clear()
        {
			_collection.DeleteMany(new BsonDocument());
		}

        public bool Contains(T item)
        {
	        return _collection.CountDocuments(this.CreateBsonDocument(item)) > 0;
        }

		public void CopyTo(T[] array, int arrayIndex) => this.CopyTo(array, arrayIndex, this);

		public bool Remove(T item)
        {
			if (Validator.IsGenericTypeNull(item))
			{
				return false;
			}

			var bsonDocument = this.CreateBsonDocument(item);
			var deleteResult = _collection.DeleteOne(bsonDocument);

			return deleteResult.IsAcknowledged;
        }

		public int Count => (int) _collection.CountDocuments(new BsonDocument());

        public bool IsReadOnly { get; } = false;
        public string Description { get; } = "Stores energy observations into provided NoSql database.";

        protected abstract string CollectionName { get; }

        protected abstract BsonDocument CreateBsonDocument(T item);

        protected abstract T FromBsonDocument(BsonDocument bsonDocument);

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}