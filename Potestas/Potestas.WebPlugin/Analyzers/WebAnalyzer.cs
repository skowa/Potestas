using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using Potestas.Configuration;
using Potestas.WebPlugin.Entities;
using Potestas.WebPlugin.ExtensionMethods;
using Potestas.WebPlugin.Mappers;
using Newtonsoft.Json;

namespace Potestas.WebPlugin.Analyzers
{
	public class WebAnalyzer : IEnergyObservationAnalizer, IDisposable
	{
		private readonly WebClient _webClient;
		private readonly string _resourcePath = "api/EnergyObservationAnalyzer";

		public WebAnalyzer(IConfiguration configuration)
		{
			var baseAddress = configuration.GetValue("webApiBaseAddress") ??
			                  throw new ArgumentNullException(nameof(configuration));

			_webClient = new WebClient { BaseAddress = $"{baseAddress}" };
		}

		public double GetMaxEnergy() => this.MakeWebClientDoubleCallCore("GetMaxEnergy");

		public double GetMaxEnergy(Coordinates coordinates)
		{
			this.AddToQueryString(coordinates, "x", "y");
			
			return this.MakeWebClientDoubleCallCore("GetMaxEnergyByObservationPoint", true);
		}

		public double GetMaxEnergy(DateTime dateTime)
		{
			this.AddToQueryString(dateTime, "dateTime");

			return this.MakeWebClientDoubleCallCore("GetMaxEnergyByObservationTime", true);
		}

		public double GetMinEnergy() => this.MakeWebClientDoubleCallCore("GetMinEnergy");

		public double GetMinEnergy(Coordinates coordinates)
		{
			this.AddToQueryString(coordinates, "x", "y");

			return this.MakeWebClientDoubleCallCore("GetMinEnergyByObservationPoint", true);
		}

		public double GetMinEnergy(DateTime dateTime)
		{
			this.AddToQueryString(dateTime, "dateTime");

			return this.MakeWebClientDoubleCallCore("GetMinEnergyByObservationTime", true);
		}

		public double GetAverageEnergy() => this.MakeWebClientDoubleCallCore("GetAverageEnergy");

		public double GetAverageEnergy(DateTime startFrom, DateTime endBy)
		{
			this.AddToQueryString(startFrom, "startFrom");
			this.AddToQueryString(endBy, "endBy");

			return this.MakeWebClientDoubleCallCore("GetAverageEnergyBetweenObservationTime", true);
		}

		public double GetAverageEnergy(Coordinates rectTopLeft, Coordinates rectBottomRight)
		{
			this.AddToQueryString(rectTopLeft, "x1", "y1");
			this.AddToQueryString(rectBottomRight, "x2", "y2");

			return this.MakeWebClientDoubleCallCore("GetAverageEnergyBetweenObservationPoint", true);
		}

		public DateTime GetMaxEnergyTime() => this.MakeWebClientDateTimeCallCore("GetMaxEnergyTime");

		public Coordinates GetMaxEnergyPosition() => this.MakeWebClientCoordinatesCallCore("GetMaxEnergyPosition");

		public DateTime GetMinEnergyTime() => this.MakeWebClientDateTimeCallCore("GetMinEnergyTime");

		public Coordinates GetMinEnergyPosition() => this.MakeWebClientCoordinatesCallCore("GetMinEnergyPosition");

		public IDictionary<double, int> GetDistributionByEnergyValue()
		{
			string response = _webClient.DownloadString($"{_resourcePath}/GetDistributionByEnergyValue");

			try
			{
				return JsonConvert.DeserializeObject<Dictionary<string, int>>(response)
					.ToDictionary(pair => double.Parse(pair.Key), pair => pair.Value);
			}
			catch (FormatException e)
			{
				throw new InvalidOperationException("Server returned not valid data.", e);
			}
		}

		public IDictionary<Coordinates, int> GetDistributionByCoordinates()
		{
			string response = _webClient.DownloadString($"{_resourcePath}/GetDistributionByCoordinates");

			try
			{
				return JsonConvert.DeserializeObject<Dictionary<string, int>>(response)
					.ToDictionary(pair => JsonConvert.DeserializeObject<CoordinatesDTO>(pair.Key).ToCoordinates(), pair => pair.Value);
			}
			catch (FormatException e)
			{
				throw new InvalidOperationException("Server returned not valid data.", e);
			}
		}

		public IDictionary<DateTime, int> GetDistributionByObservationTime()
		{
			string response = _webClient.DownloadString($"{_resourcePath}/GetDistributionByObservationTime");

			return JsonConvert.DeserializeObject<Dictionary<string, int>>(response)
				.ToDictionary(pair => pair.Key.ToDateTime(), pair => pair.Value);
		}

		public void Dispose()
		{
			_webClient?.Dispose();
		}

		private double MakeWebClientDoubleCallCore(string apiMethod, bool isQueryStringFilled = false)
		{
			string response = _webClient.DownloadString($"{_resourcePath}/{apiMethod}");
			if (isQueryStringFilled)
			{
				_webClient.QueryString.Clear();
			}

			if (!double.TryParse(response, out var energy))
			{
				throw new InvalidOperationException($"The server returned not convertible to double string {response}.");
			}

			return energy;
		}


		private DateTime MakeWebClientDateTimeCallCore(string apiMethod)
		{
			string response = _webClient.DownloadString($"{_resourcePath}/{apiMethod}");

			return response.ToDateTime();
		}

		private Coordinates MakeWebClientCoordinatesCallCore(string apiMethod)
		{
			string response = _webClient.DownloadString($"{_resourcePath}/{apiMethod}");

			var observationPoint = JsonConvert.DeserializeObject<CoordinatesDTO>(response).ToCoordinates();

			return observationPoint;
		}

		private void AddToQueryString(Coordinates coordinates, string xParameterName, string yParameterName)
		{
			_webClient.QueryString.Add(xParameterName, coordinates.X.ToString(CultureInfo.InvariantCulture));
			_webClient.QueryString.Add(yParameterName, coordinates.Y.ToString(CultureInfo.InvariantCulture));
		}

		private void AddToQueryString(DateTime observationTime, string parameterName)
		{
			_webClient.QueryString.Add(parameterName, observationTime.ToString(CultureInfo.InvariantCulture));
		}
	}
}