//using Moq;
//using Potestas.Configuration;
//using Potestas.WebPlugin.Analyzers;
//using Potestas.WebPlugin.Storages;

//namespace Potestas.Tests.Analizers
//{
//	public class WebAnalyzerTests : BaseAnalyzerTests
//	{
//		protected override IEnergyObservationAnalizer GetAnalyzer()
//		{
//			var configurationMock = new Mock<IConfiguration>();
//			configurationMock.Setup(m => m.GetValue("webApiBaseAddress"))
//				.Returns("https://localhost:44303");

//			var sqlStorage = new FlashObservationWebStorage(configurationMock.Object);

//			sqlStorage.Clear();
//			foreach (var flashObservation in ListStorage)
//			{
//				sqlStorage.Add(flashObservation);
//			}

//			return new WebAnalyzer(configurationMock.Object);
//		}
//	}
//}