using Moq;
using Potestas.Configuration;
using Potestas.Observations;
using Potestas.WebPlugin.Storages;
using Xunit;

namespace Potestas.Tests.Storages
{
	public class WebStorageTests
	{
		private readonly FlashObservationWebStorage _webStorage;

		public WebStorageTests()
		{
			var configurationMock = new Mock<IConfiguration>();
			configurationMock.Setup(m => m.GetValue("webApiBaseAddress"))
				.Returns("https://localhost:44303");

			_webStorage = new FlashObservationWebStorage(configurationMock.Object);
		}

		//[Fact]
		public void GetEnumeratorTests()
		{
			foreach (var observation in _webStorage)
			{
				
			}
		}

		//[Fact]
		public void GetCount()
		{
			Assert.Equal(16, _webStorage.Count);
		}

		//[Fact]
		public void RemoveTest()
		{
			var flashObservation = new FlashObservation(3957, new Coordinates(2.23, 23.4), 3200.1, 2000);
		}
		
		//[Fact]
		public void AddTest()
		{
			var flashObservation = new FlashObservation(new Coordinates(2.23, 23.4), 111, 1111);
			_webStorage.Add(flashObservation);
		}
	}
}