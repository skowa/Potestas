using System;
using Microsoft.AspNetCore.Mvc;

namespace Potestas.Web.Controllers
{
	[ApiController]
	[Route("api/[controller]")]
	public class EnergyObservationAnalyzerController : Controller
	{
		private readonly IEnergyObservationAnalizer _analyzer;

		public EnergyObservationAnalyzerController(IEnergyObservationAnalizer analyzer)
		{
			_analyzer = analyzer;
		}

		[HttpGet]
		public IActionResult GetMaxEnergy()
		{
			return this.Ok(_analyzer.GetMaxEnergy());
		}

		[HttpGet]
		public IActionResult GetMinEnergy()
		{
			return this.Ok(_analyzer.GetMinEnergy());
		}

		[HttpGet]
		public IActionResult GetMaxEnergy([FromQuery] DateTime dateTime)
		{
			return this.Ok(_analyzer.GetMaxEnergy(dateTime));
		}

		[HttpGet]
		public IActionResult GetMaxEnergy([FromBody] Coordinates coordinates)
		{
			return this.Ok(_analyzer.GetMaxEnergy(coordinates));
		}

		[HttpGet]
		public IActionResult GetMinEnergy([FromQuery] DateTime dateTime)
		{
			return this.Ok(_analyzer.GetMinEnergy(dateTime));
		}

		[HttpGet]
		public IActionResult GetMinEnergy([FromBody] Coordinates coordinates)
		{
			return this.Ok(_analyzer.GetMaxEnergy(coordinates));
		}

		[HttpGet]
		public IActionResult GetAverageEnergy()
		{
			return this.Ok(_analyzer.GetAverageEnergy());
		}

		[HttpGet]
		public IActionResult GetAverageEnergy([FromQuery] DateTime startFrom, [FromQuery] DateTime endBy)
		{
			return this.Ok(_analyzer.GetAverageEnergy(startFrom, endBy));
		}

		[HttpGet]
		public IActionResult GetAverageEnergy([FromBody] Coordinates rectTopLeft, [FromBody] Coordinates rectBottomRight)
		{
			return this.Ok(_analyzer.GetAverageEnergy(rectTopLeft, rectBottomRight));
		}

		[HttpGet]
		public IActionResult GetMaxEnergyTime()
		{
			return this.Ok(_analyzer.GetMaxEnergyTime());
		}

		[HttpGet]
		public IActionResult GetMinEnergyTime()
		{
			return this.Ok(_analyzer.GetMinEnergyTime());
		}

		[HttpGet]
		public IActionResult GetMaxEnergyPosition()
		{
			return this.Ok(_analyzer.GetMaxEnergyPosition());
		}

		[HttpGet]
		public IActionResult GetMinEnergyPosition()
		{
			return this.Ok(_analyzer.GetMinEnergyPosition());
		}

		[HttpGet]
		public IActionResult GetDistributionByEnergyValue()
		{
			return this.Ok(_analyzer.GetDistributionByEnergyValue());
		}

		[HttpGet]
		public IActionResult GetDistributionByCoordinates()
		{
			return this.Ok(_analyzer.GetDistributionByCoordinates());
		}

		[HttpGet]
		public IActionResult GetDistributionByObservationTime()
		{
			return this.Ok(_analyzer.GetDistributionByObservationTime());
		}
	}
}