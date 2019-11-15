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
        private bool _isDisposed;

        public SerializeProcessor(ISerializer<T> serializer)
        {
            Serializer = serializer ?? throw new ArgumentNullException(nameof(serializer));
            Stream = new MemoryStream();
        }

        public string Description => "The processor which serializes IEnergyObservation to the provided stream.";

        public Stream Stream { get; }
        protected internal long LastObjectPosition { get; protected set; }
        internal long PreviousObjectPosition { get; private set; }
        protected ISerializer<T> Serializer { get; }

        public virtual void OnCompleted()
        {

        }

        public virtual void OnError(Exception error)
        {

        }

        public void OnNext(T value)
        {
            if (Validator.IsGenericTypeNull(value))
            {
                throw new ArgumentNullException(nameof(value));
            }

            PreviousObjectPosition = Stream.Position;
            this.SerializeToStream(value);
            LastObjectPosition = Stream.Position;
        }

        public virtual void Dispose()
        {
            if (!_isDisposed)
            {
                Stream?.Dispose();
                _isDisposed = true;
            }
        }

        protected virtual void SerializeToStream(T value)
        {
            Serializer.Serialize(Stream, value);
        }
    }
}
