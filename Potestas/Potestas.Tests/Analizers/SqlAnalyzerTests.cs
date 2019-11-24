//using Moq;
//using Potestas.Configuration;
//using Potestas.SqlPlugin.Analyzers;
//using Potestas.SqlPlugin.Storages;

//namespace Potestas.Tests.Analizers
//{
//    public class SqlAnalyzerTests : BaseAnalyzerTests
//    {
//        protected override IEnergyObservationAnalizer GetAnalyzer()
//        {
//            var configurationMock = new Mock<IConfiguration>();
//            configurationMock.Setup(m => m.GetValue("connectionString"))
//                .Returns("Server=127.0.0.1,1423;Database=Potestas;User Id=SA;Password=DOCKERTASK_1");

//            var sqlStorage = new FlashObservationsSqlStorage(configurationMock.Object);

//            sqlStorage.Clear();
//            foreach (var flashObservation in ListStorage)
//            {
//                sqlStorage.Add(flashObservation);
//            }

//            var a = sqlStorage.Count;
//            return new SqlAnalyzer(new Configuration.Configuration());
//        }
//    }
//}