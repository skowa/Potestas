using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using Potestas.Configuration;
using Potestas.Observations;
using Potestas.WebPlugin.Entities;
using Potestas.WebPlugin.Mappers;

namespace Potestas.WebPlugin.Storages
{
	public class FlashObservationWebStorage : BaseWebStorage<FlashObservation>
	{
		public FlashObservationWebStorage(IConfiguration configuration) : base(configuration)
		{
		}

		protected override string GetResourcePath() => "api/FlashObservationStorage";

		protected override IEnumerable<FlashObservation> GetFromJson(string json)
		{
			return JsonConvert.DeserializeObject<FlashObservationDTO[]>(json).Select(o => o.ToFlashObservation());
		}
	}
}