using MongoDB.Bson;
using Potestas.Configuration;
using Potestas.NoSqlPlugin.Extensions;
using Potestas.Observations;

namespace Potestas.NoSqlPlugin.Storages
{
    public class FlashObservationNoSqlStorage : BaseNoSqlStorage<FlashObservation>
    {
        public FlashObservationNoSqlStorage(IConfiguration configuration) : base(configuration)
        {

        }

        protected override string CollectionName { get; } = "FlashObservations";

		protected override BsonDocument CreateBsonDocument(FlashObservation item)
		{
			return item.ToBsonDocument();
		}

        protected override FlashObservation FromBsonDocument(BsonDocument bsonDocument)
        {
	        return bsonDocument.ToFlashObservation();
        }
    }
}