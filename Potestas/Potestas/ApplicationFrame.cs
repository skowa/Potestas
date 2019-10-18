using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

namespace Potestas
{
    /* TASK. Try to refactor this code and add proper exception handling
     * 
     */
    public enum SourceStatus
    {
        Pending,
        Running,
        Completed,
        Failed
    }

    public interface ISourceRegistration<T> where T : IEnergyObservation
    {
        SourceStatus Status { get; }

        IReadOnlyCollection<IProcessingGroup<T>> ProcessingUnits { get; }

        Task Start();

        void Stop();

        void Unregister();

        IProcessingGroup<T> AttachProcessingGroup(IProcessingFactory<T> factory);
    }

    public interface IProcessingGroup<T> where T : IEnergyObservation
    {
        IEnergyObservationProcessor<T> Processor { get; }

        IEnergyObservationStorage<T> Storage { get; }

        IEnergyObservationAnalizer Analizer { get; }

        void Detach();
    }

    public interface IEnergyObservationApplicationModel<T> where T : IEnergyObservation
    {
        IReadOnlyCollection<ISourceFactory<T>> SourceFactories { get; }

        IReadOnlyCollection<IProcessingFactory<T>> ProcessingFactories { get; }

        IReadOnlyCollection<ISourceRegistration<T>> RegisteredSources { get; }

        void LoadPlugin(Assembly assembly);

        ISourceRegistration<T> CreateAndRegisterSource(ISourceFactory<T> factory);
    }

    class RegisteredSourceProcessingGroup<T> : IProcessingGroup<T> where T : IEnergyObservation
    {
        private readonly RegisteredEnergyObservationSourceWrapper<T> _sourceRegistration;
        private readonly IDisposable _processorSubscription;

        public IEnergyObservationProcessor<T> Processor { get; }

        public IEnergyObservationStorage<T> Storage { get; }

        public IEnergyObservationAnalizer Analizer { get; }

        public RegisteredSourceProcessingGroup(RegisteredEnergyObservationSourceWrapper<T> sourceRegistration, IProcessingFactory<T> factory)
        {
            _sourceRegistration = sourceRegistration;
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

    class RegisteredEnergyObservationSourceWrapper<T> : ISourceRegistration<T>, IEnergyObservationProcessor<T> where T : IEnergyObservation
    {
        private readonly ApplicationFrame<T> _app;
        private readonly IEnergyObservationSource<T> _inner;
        private readonly IDisposable _internalSubscription;
        private readonly List<IProcessingGroup<T>> _processingGroups;
        private CancellationTokenSource _cts;

        public RegisteredEnergyObservationSourceWrapper(ApplicationFrame<T> app, IEnergyObservationSource<T> inner)
        {
            _app = app;
            _inner = inner;
            _processingGroups = new List<IProcessingGroup<T>>();
            Subscribe(this);
        }

        public SourceStatus Status { get; private set; }

        public IReadOnlyCollection<IProcessingGroup<T>> ProcessingUnits => _processingGroups.AsReadOnly();

        public string Description => "Internal application listener to track Sources State";

        internal IDisposable Subscribe(IEnergyObservationProcessor<T> processor)
        {
            return _inner.Subscribe(processor);
        }

        public IProcessingGroup<T> AttachProcessingGroup(IProcessingFactory<T> factory)
        {
            var processingGroup = new RegisteredSourceProcessingGroup<T>(this, factory);
            _processingGroups.Add(processingGroup);
            return processingGroup;
        }

        internal void RemoveProcessingGroup(IProcessingGroup<T> processingGroup)
        {
            _processingGroups.Remove(processingGroup);
        }

        public void OnCompleted() => Status = SourceStatus.Completed;

        public void OnError(Exception error) => Status = SourceStatus.Failed;

        public void OnNext(T value) => Status = SourceStatus.Running;

        public Task Start()
        {
            // TODO: add SemaphoreSlim to prevent multiple runs
            _cts = new CancellationTokenSource();
            return _inner.Run(_cts.Token);
        }

        public void Stop()
        {
            _cts.Cancel();
        }

        public void Unregister()
        {
            _internalSubscription.Dispose();
            _app.RemoveRegistration(this);
        }
    }

    public sealed class ApplicationFrame<T> : IEnergyObservationApplicationModel<T> where T : IEnergyObservation
    {
        private readonly static FactoriesLoader<T> _factoriesLoader = new FactoriesLoader<T>();

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
            var (sourceFactories, processingFactories) = _factoriesLoader.Load(assembly);
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
