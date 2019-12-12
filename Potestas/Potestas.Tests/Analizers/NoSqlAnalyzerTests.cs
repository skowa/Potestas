//using Moq;
//using Potestas.Configuration;
//using Potestas.NoSqlPlugin.Analyzers;
//using Potestas.NoSqlPlugin.Storages;

//namespace Potestas.Tests.Analizers
//{
//	public class NoSqlAnalyzerTests : BaseAnalyzerTests
//	{
//		protected override IEnergyObservationAnalizer GetAnalyzer()
//		{
//			var configurationMock = new Mock<IConfiguration>();
//			configurationMock.Setup(m => m.GetValue("connectionStringNoSql"))
//				.Returns("mongodb://localhost:27018");
//			configurationMock.Setup(m => m.GetValue("noSqlDbName"))
//				.Returns("Potestas");

//			var noSqlStorage = new FlashObservationNoSqlStorage(configurationMock.Object);
//			noSqlStorage.Clear();
//			foreach (var flashObservation in ListStorage)
//			{
//				noSqlStorage.Add(flashObservation);
//			}

//			return new NoSqlAnalyzer(configurationMock.Object, "FlashObservations");
//		}
//	}
//}