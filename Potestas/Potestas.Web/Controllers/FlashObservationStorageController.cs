using Microsoft.AspNetCore.Mvc;
using Potestas.Observations;

namespace Potestas.Web.Controllers
{
	[ApiController]
	[Route("api/[controller]")]
	public class FlashObservationStorageController : BaseEnergyObservationStorageController<FlashObservation>
	{
		public FlashObservationStorageController(IEnergyObservationStorage<FlashObservation> storage) : base(storage)
		{
		}
	}
}