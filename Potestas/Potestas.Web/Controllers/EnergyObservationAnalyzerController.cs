using System;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

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
		public IActionResult GetMaxEnergy([FromQuery] double x, [FromQuery] double y)
		{
			return this.Ok(_analyzer.GetMaxEnergy(new Coordinates(x, y)));
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
		public IActionResult GetMinEnergy([FromQuery] double x, [FromQuery] double y)
		{
			return this.Ok(_analyzer.GetMinEnergy(new Coordinates(x, y)));
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
		public IActionResult GetAverageEnergy([FromQuery] double x1, [FromQuery] double y1, [FromQuery] double x2, [FromQuery] double y2)
		{
			return this.Ok(_analyzer.GetAverageEnergy(new Coordinates(x1, y1), new Coordinates(x2, y2)));
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
			return this.Ok(_analyzer.GetDistributionByEnergyValue()
				.ToDictionary(pair => pair.Key.ToString(CultureInfo.InvariantCulture), pair => pair.Value));
		}

		[HttpGet("GetDistributionByCoordinates")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status500InternalServerError)]
		public IActionResult GetDistributionByCoordinates()
		{
			return this.Ok(_analyzer.GetDistributionByCoordinates()
				.ToDictionary(pair => JsonConvert.SerializeObject(pair.Key), pair => pair.Value));
		}

		[HttpGet("GetDistributionByObservationTime")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status500InternalServerError)]
		public IActionResult GetDistributionByObservationTime()
		{
			return this.Ok(_analyzer.GetDistributionByObservationTime()
				.ToDictionary(pair => JsonConvert.ToString(pair.Key), pair => pair.Value));
		}
	}
}