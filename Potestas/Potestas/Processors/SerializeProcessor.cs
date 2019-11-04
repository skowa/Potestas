using System;
using System.IO;
using Potestas.Utils;

namespace Potestas.Processors
{
    /* TASK. Implement processor which serializes IEnergyObservation to the provided stream.
     * 1. Use serialization mechanism here. 
     * 2. Some IEnergyObservation could not be serializable.
     */
    public class SerializeProcessor<T> : IDisposable, IEnergyObservationProcessor<T> where T : IEnergyObservation
    {
        private readonly ISerializer<T> _serializer;
        private bool _isDisposed;

        public SerializeProcessor(ISerializer<T> serializer)
        {
            _serializer = serializer ?? throw new ArgumentNullException(nameof(serializer));
            Stream = new MemoryStream();
        }

        public string Description => "The processor which serializes IEnergyObservation to the provided stream.";

        internal Stream Stream { get; }
        internal long PreviousObjectPosition { get; private set; }

        public void OnCompleted()
        {
            this.Dispose();
        }

        public void OnError(Exception error)
        {
            this.Dispose();
        }

        public void OnNext(T value)
        {
            if (Validator.IsGenericTypeNull(value))
            {
                throw new ArgumentNullException(nameof(value));
            }

            PreviousObjectPosition = Stream.Position;
            _serializer.Serialize(Stream, value);
        }

        public void Dispose()
        {
            if (!_isDisposed)
            {
                Stream?.Dispose();
                _isDisposed = true;
            }
        }
    }
}
