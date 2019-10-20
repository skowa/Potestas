using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Potestas.Sources
{
    /* TASK. Implement random observation source.
     * 1. This class should generate observations by random periods of time.
     * 1. Implement both IEnergyObservationSource and IEnergyObservationSourceEventSource interfaces.
     * 2. Try to implement it with abstract class or delegate parameters to make it universal.
     */
    public abstract class EnergySourceBase<T> : IEnergyObservationEventSource<T>, IEnergyObservationSource<T> where T : IEnergyObservation
    {
        public event EventHandler<T> NewValueObserved;
        public event EventHandler<Exception> ObservationError;
        public event EventHandler ObservationEnd;

        private readonly List<IObserver<T>> _observers = new List<IObserver<T>>();

        public IDisposable Subscribe(IObserver<T> observer)
        {
            if (observer == null)
            {
                throw new ArgumentNullException(nameof(observer));
            }

            EventHandler<T> onNext = (o, e) => observer.OnNext(e);
            EventHandler onCompleted = (o, e) => observer.OnCompleted();
            EventHandler<Exception> onError = (o, e) => observer.OnError(e);

            if (!_observers.Contains(observer))
            {
                NewValueObserved += onNext;
                ObservationEnd += onCompleted;
                ObservationError += onError;

                _observers.Add(observer);
            }

            return new RandomEnergySourceSubscription<T>(this, observer, onNext, onError, onCompleted);
        }

        internal void Unsubscribe(IObserver<T> observer)
        {
            if (observer == null)
            {
                throw new ArgumentNullException(nameof(observer));
            }
            
            if (_observers.Contains(observer))
            {
                _observers.Remove(observer);
            }
        }

        public string Description => "Generates observations by random period of time";

        public async Task Run(CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            await Task.WhenAny(CheckCancellation(cancellationToken), GenerateObservations(cancellationToken));

            Done(this, EventArgs.Empty);
        }

        protected abstract Task GenerateObservations(CancellationToken cancellationToken);

        private async Task CheckCancellation(CancellationToken cancellationToken)
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                await Task.Delay(500);
            }
        }

        protected void NewValueGenerated(object sender, T eventArgs) => NewValueObserved?.Invoke(sender, eventArgs);
       
        protected void Done(object sender, EventArgs eventArgs) => ObservationEnd?.Invoke(sender, eventArgs);

        protected void PublishException(object sender, Exception eventArgs) => ObservationError?.Invoke(sender, eventArgs);
    }

    internal class RandomEnergySourceSubscription<T> : IDisposable where T : IEnergyObservation
    {
        private readonly EnergySourceBase<T> _randomEnergySource;
        private readonly IObserver<T> _observer;
        private readonly EventHandler<T> _onNext;
        private readonly EventHandler<Exception> _onError;
        private readonly EventHandler _onCompleted;

        public RandomEnergySourceSubscription(EnergySourceBase<T> randomEnergySource, IObserver<T> observer, 
            EventHandler<T> onNext, EventHandler<Exception> onError, EventHandler onCompleted)
        {
            _randomEnergySource = randomEnergySource;
            _observer = observer;
            _onNext = onNext;
            _onError = onError;
            _onCompleted = onCompleted;
        }

        public void Dispose()
        {
            _randomEnergySource.Unsubscribe(_observer);

            _randomEnergySource.NewValueObserved -= _onNext;
            _randomEnergySource.ObservationEnd -= _onCompleted;
            _randomEnergySource.ObservationError -= _onError;
        }
    }
}
