using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Potestas.ApplicationFrame.ProcessingGroups;

namespace Potestas.ApplicationFrame.SourceRegistrations
{
    class RegisteredEnergyObservationSourceWrapper<T> : ISourceRegistration<T>, IEnergyObservationProcessor<T> where T : IEnergyObservation
    {
        private readonly ApplicationFrame<T> _app;
        private readonly IEnergyObservationSource<T> _inner;
        private readonly IDisposable _internalSubscription;
        private readonly List<IProcessingGroup<T>> _processingGroups;
        private CancellationTokenSource _cts;

        public RegisteredEnergyObservationSourceWrapper(ApplicationFrame<T> app, IEnergyObservationSource<T> inner)
        {
            _app = app ?? throw new ArgumentNullException(nameof(app));
            _inner = inner ?? throw new ArgumentNullException(nameof(inner));

            _processingGroups = new List<IProcessingGroup<T>>();
            Subscribe(this);
        }

        public SourceStatus Status { get; private set; }

        public IReadOnlyCollection<IProcessingGroup<T>> ProcessingUnits => _processingGroups.AsReadOnly();

        public string Description => "Internal application listener to track Sources State";

        internal IDisposable Subscribe(IEnergyObservationProcessor<T> processor)
        {
            if (processor == null)
            {
                throw new ArgumentNullException(nameof(processor));
            }

            return _inner.Subscribe(processor);
        }

        public IProcessingGroup<T> AttachProcessingGroup(IProcessingFactory<T> factory)
        {
            if (factory == null)
            {
                throw new ArgumentNullException(nameof(factory));
            }

            var processingGroup = new RegisteredSourceProcessingGroup<T>(this, factory);
            _processingGroups.Add(processingGroup);
            return processingGroup;
        }

        internal void RemoveProcessingGroup(IProcessingGroup<T> processingGroup)
        {
            if (processingGroup == null)
            {
                throw new ArgumentNullException(nameof(processingGroup));
            }

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
}