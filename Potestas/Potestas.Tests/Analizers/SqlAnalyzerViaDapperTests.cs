//using Potestas.Configuration;
//using Potestas.OrmPlugin.Analyzers;
//using Moq;
//using Potestas.OrmPlugin.DapperConfiguration;
//using Potestas.OrmPlugin.Storages;

//namespace Potestas.Tests.Analizers
//{
//    public class SqlAnalyzerViaDapperTests : BaseAnalyzerTests
//    {
//        protected override IEnergyObservationAnalizer GetAnalyzer()
//        {
//            DapperInitializer.InitDapper();

//            var configurationMock = new Mock<IConfiguration>();
//            configurationMock.Setup(m => m.GetValue("connectionString"))
//                .Returns("Server=127.0.0.1,1423;Database=Potestas;User Id=SA;Password=DOCKERTASK_1");

//            var sqlStorage = new FlashObservationsSqlStorage(configurationMock.Object);

//            sqlStorage.Clear();
//            foreach (var flashObservation in ListStorage)
//            {
//                sqlStorage.Add(flashObservation);
//            }

//            return new SqlAnalyzer(configurationMock.Object);
//        }
//    }
//}