using System;
using Potestas.Analizers;
using Potestas.Configuration;
using Potestas.Processors;
using Potestas.Storages;

namespace Potestas.Factories
{
    public abstract class FileBaseProcessingFactory<T> : IProcessingFactory<T> where T : IEnergyObservation
    {
        private readonly IConfiguration _configuration;

        protected FileBaseProcessingFactory(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public abstract IEnergyObservationProcessor<T> CreateProcessor();

        public IEnergyObservationStorage<T> CreateStorage()
        {
            return new FileStorage<T>(GetSerializer(), GetFilePath());
        }

        public IEnergyObservationAnalizer CreateAnalizer() => new LINQAnalizer<T>(this.CreateStorage());

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