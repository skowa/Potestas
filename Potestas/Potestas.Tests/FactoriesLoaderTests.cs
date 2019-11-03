using System;
using System.Linq;
using System.Reflection;
using Potestas.Factories;
using Potestas.Observations;
using Potestas.Sources;
using Xunit;

namespace Potestas.Tests
{
    public class FactoriesLoaderTests
    {
        private readonly FactoriesLoader<FlashObservation> _factoriesLoader = new FactoriesLoader<FlashObservation>();

        [Fact]
        public void Load_PotestasAssembly_FactoriesAreLoaded()
        {
            var factories = _factoriesLoader.Load(Assembly.LoadFrom("Potestas.dll"));

            Assert.Single(factories.SourceFactories);
            Assert.Equal(3, factories.ProcessingFactories.Length);
            Assert.Equal(typeof(RandomEnergySourceFactory), factories.SourceFactories[0].GetType());
            Assert.Contains(factories.ProcessingFactories, f => f.GetType() == typeof(FileProcessingFactory<FlashObservation>));
            Assert.Contains(factories.ProcessingFactories, f => f.GetType() == typeof(FileStorageProcessingFactory<FlashObservation>));
            Assert.Contains(factories.ProcessingFactories, f => f.GetType() == typeof(ListStorageProcessingFactory<FlashObservation>));
        }

        [Fact]
        public void Load_AssemblyIsNull_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => _factoriesLoader.Load(null));
        }
    }
}