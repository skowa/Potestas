using Moq;
using Potestas.Configuration;
using Potestas.Observations;
using Potestas.OrmPlugin.DapperConfiguration;
using Potestas.OrmPlugin.Storages;
using Xunit;

namespace Potestas.Tests.Storages
{
    public class SqlStorageViaEFTests
    {
        private readonly FlashObservationsSqlStorageEF _sqlStorage;

        public SqlStorageViaEFTests()
        {
            DapperInitializer.InitDapper();
            var configurationMock = new Mock<IConfiguration>();
            configurationMock.Setup(m => m.GetValue("connectionString"))
                .Returns("Server=127.0.0.1,1423;Database=PotestasEF;User Id=SA;Password=DOCKERTASK_1");

            _sqlStorage = new FlashObservationsSqlStorageEF(configurationMock.Object);
        }

     //   [Fact]
        public void GetEnumeratorTest()
        {
            foreach (var flashObservation in _sqlStorage)
            {

            }
        }

       // [Fact]
        public void AddTest()
        {
            var flashObservation = new FlashObservation(new Coordinates(2.23, 23.4), 3200.1, 2000);
            _sqlStorage.Add(flashObservation);
        }

        //  [Fact]
        public void RemoveTest()
        {
            var flashObservation = new FlashObservation(1, new Coordinates(2.23, 23.4), 3200.1, 2000);
            var result = _sqlStorage.Remove(flashObservation);
        }

      //  [Fact]
        public void Count()
        {
            Assert.Equal(1, _sqlStorage.Count);
        }
    }
}