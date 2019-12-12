using MongoDB.Bson;
using Potestas.Observations;

namespace Potestas.NoSqlPlugin.Extensions
{
	internal static class FlashObservationMapper
	{
		internal static BsonDocument ToBsonDocument(this FlashObservation flashObservation)
		{
			return new BsonDocument
			{
				["DurationMs"]= flashObservation.DurationMs,
				["EstimatedValue"] = flashObservation.EstimatedValue,
				["Intensity"] = flashObservation.Intensity,
				["ObservationTime"] = flashObservation.ObservationTime,
				["ObservationPoint"] = flashObservation.ObservationPoint.ToBsonDocument()
			};
		}

		internal static FlashObservation ToFlashObservation(this BsonDocument bsonDocument)
		{
			var coordinatesBsonDocument = bsonDocument["ObservationPoint"] as BsonDocument;

			return new FlashObservation(coordinatesBsonDocument.ToCoordinates(), bsonDocument["Intensity"].AsDouble, bsonDocument["DurationMs"].AsInt32, bsonDocument["ObservationTime"].ToUniversalTime());
		}
	}
}