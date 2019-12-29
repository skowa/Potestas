using System;
using Potestas.Analizers;
using Potestas.Configuration;
using Potestas.Logging;
using Potestas.Logging.Decorators;
using Potestas.Processors;
using Potestas.Storages;

namespace Potestas.Factories
{
    public abstract class FileBaseProcessingFactory<T> : IProcessingFactory<T> where T : IEnergyObservation
    {
        private readonly IConfiguration _configuration;

        protected FileBaseProcessingFactory(ILogger logger, IConfiguration configuration)
        {
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
            Logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        protected ILogger Logger { get; set; }
        
        public abstract IEnergyObservationProcessor<T> CreateProcessor();

        public IEnergyObservationStorage<T> CreateStorage()
        {
            return new LogEnergyObservationStorageDecorator<T>(new FileStorage<T>(GetSerializer(), GetFilePath()), Logger);
        }

        public IEnergyObservationAnalizer CreateAnalizer() =>
	        new LogEnergyObservationAnalyzerDecorator(new LINQAnalizer<T>(this.CreateStorage()), Logger);

        protected string GetFilePath()
        {
            return _configuration.GetValue("storageFilePath");
        }

        protected ISerializer<T> GetSerializer()
        {
            if (Enum.TryParse(_configuration.GetValue("serializerType"), out SerializerType serializerType))
            {
                switch (serializerType)
                {
                    case SerializerType.Binary:
                        return new BinarySerializer<T>();
                }
            }

            return null;
        }
    }

    internal enum SerializerType
    {
        Binary
    }
}