using System;
using Potestas.ApplicationFrame.SourceRegistrations;

namespace Potestas.ApplicationFrame.ProcessingGroups
{
    class RegisteredSourceProcessingGroup<T> : IProcessingGroup<T> where T : IEnergyObservation
    {
        private readonly RegisteredEnergyObservationSourceWrapper<T> _sourceRegistration;
        private readonly IDisposable _processorSubscription;

        public IEnergyObservationProcessor<T> Processor { get; }

        public IEnergyObservationStorage<T> Storage { get; }

        public IEnergyObservationAnalizer Analizer { get; }

        public RegisteredSourceProcessingGroup(RegisteredEnergyObservationSourceWrapper<T> sourceRegistration, IProcessingFactory<T> factory)
        {
            _sourceRegistration = sourceRegistration ?? throw new ArgumentNullException(nameof(sourceRegistration));
            if (factory == null)
            {
                throw new ArgumentNullException(nameof(factory));
            }

            Processor = factory.CreateProcessor();
            Storage = factory.CreateStorage();
            Analizer = factory.CreateAnalizer();

            _processorSubscription = _sourceRegistration.Subscribe(Processor);
        }

        public void Detach()
        {
            _processorSubscription.Dispose();
            _sourceRegistration.RemoveProcessingGroup(this);
        }
    }
}