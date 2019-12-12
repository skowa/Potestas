using MongoDB.Bson;

namespace Potestas.NoSqlPlugin.Extensions
{
	internal static class CoordinatesMapper
	{
		internal static Coordinates ToCoordinates(this BsonDocument bsonDocument)
		{
			return bsonDocument != null
				? new Coordinates(bsonDocument["X"].AsDouble, bsonDocument["Y"].AsDouble)
				: new Coordinates();
		}
	}
}