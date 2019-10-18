using Potestas.Observations;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Potestas.Apps.Terminal
{
    class ConsoleSourceSubscription : IDisposable
    {
        private readonly ConsoleSource _source;
        private readonly IObserver<FlashObservation> _processor;

        public ConsoleSourceSubscription(ConsoleSource source, IObserver<FlashObservation> processor)
        {
            _source = source;
            _processor = processor;
        }

        public void Dispose()
        {
            _source.Unsubscribe(_processor);
        }
    }

    class ConsoleSource : IEnergyObservationSource<FlashObservation>
    {
        private readonly List<IObserver<FlashObservation>> _processors;

        public string Description => "Console input energy observation";

        public ConsoleSource()
        {
            _processors = new List<IObserver<FlashObservation>>();
        }

        public async Task Run(CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            await Task.WhenAny(
                ReadInput(cancellationToken),
                CheckCancellation(cancellationToken)
                );
        }

        private async Task CheckCancellation(CancellationToken cancellationToken)
        {
            while(!cancellationToken.IsCancellationRequested)
            {
                await Task.Delay(500);
            }
            Done();
        }

        private async Task ReadInput(CancellationToken cancellationToken)
        {
            await Task.Run(() =>
            {
                while (!cancellationToken.IsCancellationRequested)
                {
                    try
                    {
                        var str = Console.ReadLine();
                        if (!string.IsNullOrEmpty(str))
                        {
                            GenerateRandomObservation();
                        }
                    }
                    catch (Exception ex)
                    {
                        PublishException(ex);
                        throw;
                    }
                }
            });
        }

        private void Done()
        {
            foreach (var processor in _processors)
            {
                processor.OnCompleted();
            }
        }

        private void GenerateRandomObservation()
        {
            FlashObservation obs = new FlashObservation();
            foreach(var processor in _processors)
            {
                processor.OnNext(obs);
            }
        }

        private void PublishException(Exception error)
        {
            foreach (var processor in _processors)
            {
                processor.OnError(error);
            }
        }

        public IDisposable Subscribe(IObserver<FlashObservation> observer)
        {
            _processors.Add(observer);
            return new ConsoleSourceSubscription(this, observer);
        }

        internal void Unsubscribe(IObserver<FlashObservation> observer)
        {
            _processors.Remove(observer);
        }
    }
}
