using System;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Potestas.Observations;
using Potestas.Web.Mappers;
using Potestas.Web.Models;

namespace Potestas.Web.Controllers
{
	[ApiController]
	[Route("api/[controller]")]
	public class FlashObservationStorageController : BaseEnergyObservationStorageController<FlashObservation>
	{
		public FlashObservationStorageController(IEnergyObservationStorage<FlashObservation> storage) : base(storage)
		{
		}

		[HttpPost]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		[ProducesResponseType(StatusCodes.Status500InternalServerError)]
		public IActionResult Add([FromBody]FlashObservationModel observation)
		{
			try
			{
				this.Storage.Add(observation.ToFlashObservation());
			}
			catch (ArgumentException e)
			{
				return this.BadRequest(e.Message);
			}

			return this.Ok();
		}
	}
}