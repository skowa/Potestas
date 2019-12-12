using System;
using Moq;
using Potestas.Configuration;
using Potestas.NoSqlPlugin.Storages;
using Potestas.Observations;
using Xunit;

namespace Potestas.Tests.Storages
{
    public class NoSqlStorageTests
    {
        private readonly BaseNoSqlStorage<FlashObservation> _sqlStorage;

        public NoSqlStorageTests()
        {
            var configurationMock = new Mock<IConfiguration>();
            configurationMock.Setup(m => m.GetValue("connectionStringNoSql"))
                .Returns("mongodb://localhost:27018"); 
            configurationMock.Setup(m => m.GetValue("noSqlDbName"))
                .Returns("Potestas");

            _sqlStorage = new FlashObservationNoSqlStorage(configurationMock.Object);
        }

        //[Fact]
        public void GetEnumeratorTest()
        {
            foreach (var flashObservation in _sqlStorage)
            {

            }
        }
        
       // [Fact]
        public void AddTest()
        {
            var flashObservation = new FlashObservation(new Coordinates(12.5, 23.4), 300.1, 2000, new DateTime(2019, 12, 12));
            _sqlStorage.Add(flashObservation);
        }

        //[Fact]
        public void RemoveTest()
        {
            var flashObservation = new FlashObservation(new Coordinates(12.5,  23.4), 300.1, 2000, new DateTime(2019, 12, 12));
            _sqlStorage.Remove(flashObservation);
        }

        //[Fact]
        public void Count()
        {
            Assert.Equal(3, _sqlStorage.Count);
        }
    }
}