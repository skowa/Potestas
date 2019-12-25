using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;

namespace Potestas.Web.Controllers
{
	public abstract class BaseEnergyObservationStorageController<T> : ControllerBase where T : IEnergyObservation
	{
		private readonly IEnergyObservationStorage<T> _storage;

		protected BaseEnergyObservationStorageController(IEnergyObservationStorage<T> storage)
		{
			_storage = storage;
		}

		[HttpGet]
		public IActionResult Get()
		{
			return this.Ok(_storage);
		}

		[HttpPost]
		public IActionResult Add([FromBody]T observation)
		{
			try
			{
				_storage.Add(observation);
			}
			catch (Exception e)
			{
				Console.WriteLine(e);
				throw;
			}

			return this.Ok();
		}

		[HttpDelete("{id}")]
		public IActionResult Delete(int id)
		{
			T observation;
			try
			{
				observation = _storage.Single(o => o.Id == id);
			}
			catch (InvalidOperationException)
			{
				return this.NotFound(id);
			}

			_storage.Remove(observation);
			return this.Ok();
		}
	}
}
