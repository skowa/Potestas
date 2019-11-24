using Moq;
using Potestas.Configuration;
using Potestas.Observations;
using Potestas.OrmPlugin.DapperConfiguration;
using Potestas.OrmPlugin.Storages;
using Xunit;

namespace Potestas.Tests.Storages
{
    public class SqlStorageViaDapperTests
    {
        private readonly FlashObservationsSqlStorage _sqlStorage;

        public SqlStorageViaDapperTests()
        {
            DapperInitializer.InitDapper();
            var configurationMock = new Mock<IConfiguration>();
            configurationMock.Setup(m => m.GetValue("connectionString"))
                .Returns("Server=127.0.0.1,1423;Database=Potestas;User Id=SA;Password=DOCKERTASK_1");

            _sqlStorage = new FlashObservationsSqlStorage(configurationMock.Object);
        }

       // [Fact]
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

       // [Fact]
        public void RemoveTest()
        {
            var flashObservation = new FlashObservation(2597, new Coordinates(2.23, 23.4), 3200.1, 2000);
            _sqlStorage.Remove(flashObservation);
        }

        //[Fact]
        public void Count()
        {
            Assert.Equal(16, _sqlStorage.Count);
        }
    }
}