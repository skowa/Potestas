using System;
using System.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Potestas.Web.Controllers
{
	public abstract class BaseEnergyObservationStorageController<T> : ControllerBase where T : IEnergyObservation
	{
		protected BaseEnergyObservationStorageController(IEnergyObservationStorage<T> storage)
		{
			Storage = storage;
		}

		protected IEnergyObservationStorage<T> Storage { get; }

		[HttpGet]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status500InternalServerError)]
		public IActionResult Get()
		{
			return this.Ok(Storage);
		}

		[HttpGet("GetObservationsCount")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status500InternalServerError)]
		public IActionResult Count()
		{
			return this.Ok(Storage.Count);
		}

		[HttpDelete("{id}")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		[ProducesResponseType(StatusCodes.Status500InternalServerError)]
		public IActionResult Delete(int id)
		{
			T observation;
			try
			{
				observation = Storage.Single(o => o.Id == id);
			}
			catch (InvalidOperationException)
			{
				return this.BadRequest($"Cannot perform remove operation, because no observation with id {id} found.");
			}

			Storage.Remove(observation);
			return this.Ok();
		}

		[HttpDelete("DeleteAll")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status500InternalServerError)]
		public IActionResult DeleteAll()
		{
			Storage.Clear();

			return this.Ok();
		}
	}
}
