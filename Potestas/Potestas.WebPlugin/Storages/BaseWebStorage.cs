using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Net;
using Potestas.Configuration;
using Potestas.Storages;
using Potestas.Utils;
using Newtonsoft.Json;

namespace Potestas.WebPlugin.Storages
{
	public abstract class BaseWebStorage<T> : BaseStorage<T>, IDisposable, IEnergyObservationStorage<T> where T : IEnergyObservation
	{
		private readonly WebClient _webClient;

		protected BaseWebStorage(IConfiguration configuration)
		{
			var baseAddress = configuration.GetValue("webApiBaseAddress") ??
			                  throw new ArgumentNullException(nameof(configuration));
			_webClient = new WebClient {BaseAddress = baseAddress};
		}

		public int Count
		{
			get
			{
				string response = _webClient.DownloadString($"{this.GetResourcePath()}/GetObservationsCount");
				if (int.TryParse(response, out var count))
				{
					return count;
				}

				throw new InvalidOperationException($"Cannot get count, because web API returns not convertable to integer string {response}.");
			}
		}

		public bool IsReadOnly { get; } = false;
		public string Description { get; } = "Stores energy observations using provided Web API";

		public IEnumerator<T> GetEnumerator()
		{
			string response = _webClient.DownloadString(this.GetResourcePath());
			var observations = this.GetFromJson(response);

			return observations.GetEnumerator();
		}

		public void Add(T item)
		{
			if (Validator.IsGenericTypeNull(item))
			{
				throw new ArgumentNullException(nameof(item));
			}

			_webClient.Headers.Add("Content-Type", "application/json");
			_webClient.UploadString(this.GetResourcePath(), JsonConvert.SerializeObject(item));
		}

		public void Clear()
		{
			_webClient.UploadValues($"{this.GetResourcePath()}/DeleteAll", "DELETE", new NameValueCollection());
		}

		public bool Contains(T item) => base.Contains(item, this);

		public void CopyTo(T[] array, int arrayIndex) => base.CopyTo(array, arrayIndex, this);

		public bool Remove(T item)
		{
			if (Validator.IsGenericTypeNull(item))
			{
				return false;
			}

			try
			{
				_webClient.UploadValues($"{this.GetResourcePath()}/{item.Id}", "DELETE", new NameValueCollection());
				return true;
			}
			catch (WebException)
			{
				return false;
			}
			
		}

		protected abstract string GetResourcePath();
		protected abstract IEnumerable<T> GetFromJson(string json);

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}

		public void Dispose()
		{
			_webClient?.Dispose();
		}
	}
}