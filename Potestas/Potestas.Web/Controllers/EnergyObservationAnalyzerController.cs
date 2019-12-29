using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;
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

		[HttpGet("GetMaxEnergy")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status500InternalServerError)]

		public IActionResult GetMaxEnergy()
		{
			return this.Ok(_analyzer.GetMaxEnergy());
		}

		[HttpGet("GetMinEnergy")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status500InternalServerError)]
		public IActionResult GetMinEnergy()
		{
			return this.Ok(_analyzer.GetMinEnergy());
		}

		[HttpGet("GetMaxEnergyByObservationTime")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status500InternalServerError)]
		public IActionResult GetMaxEnergy([FromQuery] DateTime dateTime)
		{
			return this.Ok(_analyzer.GetMaxEnergy(dateTime));
		}

		[HttpGet("GetMaxEnergyByObservationPoint")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status500InternalServerError)]
		public IActionResult GetMaxEnergy([FromBody] Coordinates coordinates)
		{
			return this.Ok(_analyzer.GetMaxEnergy(coordinates));
		}

		[HttpGet("GetMinEnergyByObservationTime")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status500InternalServerError)]
		public IActionResult GetMinEnergy([FromQuery] DateTime dateTime)
		{
			return this.Ok(_analyzer.GetMinEnergy(dateTime));
		}

		[HttpGet("GetMinEnergyByObservationPoint")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status500InternalServerError)]
		public IActionResult GetMinEnergy([FromBody] Coordinates coordinates)
		{
			return this.Ok(_analyzer.GetMaxEnergy(coordinates));
		}

		[HttpGet("GetAverageEnergy")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status500InternalServerError)]
		public IActionResult GetAverageEnergy()
		{
			return this.Ok(_analyzer.GetAverageEnergy());
		}

		[HttpGet("GetAverageEnergyBetweenObservationTime")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status500InternalServerError)]
		public IActionResult GetAverageEnergy([FromQuery] DateTime startFrom, [FromQuery] DateTime endBy)
		{
			return this.Ok(_analyzer.GetAverageEnergy(startFrom, endBy));
		}

		[HttpGet("GetAverageEnergyBetweenObservationPoint")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status500InternalServerError)]
		public IActionResult GetAverageEnergy([MaxLength(2), FromBody] Coordinates[] coordinates)
		{
			return this.Ok(_analyzer.GetAverageEnergy(coordinates[0], coordinates[1]));
		}

		[HttpGet("GetMaxEnergyTime")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status500InternalServerError)]
		public IActionResult GetMaxEnergyTime()
		{
			return this.Ok(_analyzer.GetMaxEnergyTime());
		}

		[HttpGet("GetMinEnergyTime")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status500InternalServerError)]
		public IActionResult GetMinEnergyTime()
		{
			return this.Ok(_analyzer.GetMinEnergyTime());
		}

		[HttpGet("GetMaxEnergyPosition")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status500InternalServerError)]
		public IActionResult GetMaxEnergyPosition()
		{
			return this.Ok(_analyzer.GetMaxEnergyPosition());
		}

		[HttpGet("GetMinEnergyPosition")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status500InternalServerError)]
		public IActionResult GetMinEnergyPosition()
		{
			return this.Ok(_analyzer.GetMinEnergyPosition());
		}

		[HttpGet("GetDistributionByEnergyValue")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status500InternalServerError)]
		public IActionResult GetDistributionByEnergyValue()
		{
			return this.Ok(_analyzer.GetDistributionByEnergyValue());
		}

		[HttpGet("GetDistributionByCoordinates")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status500InternalServerError)]
		public IActionResult GetDistributionByCoordinates()
		{
			return this.Ok(_analyzer.GetDistributionByCoordinates());
		}

		[HttpGet("GetDistributionByObservationTime")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status500InternalServerError)]
		public IActionResult GetDistributionByObservationTime()
		{
			return this.Ok(_analyzer.GetDistributionByObservationTime());
		}
	}
}