using System.Collections.Generic;
using System.Reflection;
using Potestas.ApplicationFrame.SourceRegistrations;

namespace Potestas.ApplicationFrame
{
    /* TASK. Try to refactor this code and add proper exception handling
     * 
     */

    public sealed class ApplicationFrame<T> : IEnergyObservationApplicationModel<T> where T : IEnergyObservation
    {
        private static readonly FactoriesLoader<T> FactoriesLoader = new FactoriesLoader<T>();

        private readonly List<ISourceFactory<T>> _sourceFactories;
        private readonly List<IProcessingFactory<T>> _processingFactories;
        private readonly List<RegisteredEnergyObservationSourceWrapper<T>> _registeredSources;

        public IReadOnlyCollection<ISourceFactory<T>> SourceFactories => _sourceFactories.AsReadOnly();
        public IReadOnlyCollection<IProcessingFactory<T>> ProcessingFactories => _processingFactories.AsReadOnly();
        public IReadOnlyCollection<ISourceRegistration<T>> RegisteredSources => _registeredSources.AsReadOnly();

        public ApplicationFrame()
        {
            _registeredSources = new List<RegisteredEnergyObservationSourceWrapper<T>>();
            _processingFactories = new List<IProcessingFactory<T>>();
            _sourceFactories = new List<ISourceFactory<T>>();
        }

        public void LoadPlugin(Assembly assembly)
        {
            var (sourceFactories, processingFactories) = FactoriesLoader.Load(assembly);
            _processingFactories.AddRange(processingFactories);
            _sourceFactories.AddRange(sourceFactories);
        }

        public ISourceRegistration<T> CreateAndRegisterSource(ISourceFactory<T> factory)
        {
            var source = factory.CreateSource();
            var registration = new RegisteredEnergyObservationSourceWrapper<T>(this, source);
            _registeredSources.Add(registration);
            return registration;
        }

        internal void RemoveRegistration(RegisteredEnergyObservationSourceWrapper<T> registration)
        {
            _registeredSources.Remove(registration);
        }
    }
}
