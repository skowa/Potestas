using System;
using Potestas.Utils;

namespace Potestas.Processors
{
    public class SaveToStorageProcessor<T> : IEnergyObservationProcessor<T> where T : IEnergyObservation
    {
        private readonly IEnergyObservationStorage<T> _storage;

        public SaveToStorageProcessor(IEnergyObservationStorage<T> storage)
        {
            _storage = storage;
        }

        public string Description => "Saves observations to provided storage";

        public void OnCompleted()
        {

        }

        public void OnError(Exception error)
        {

        }

        public void OnNext(T value)
        {
            if (Validator.IsGenericTypeNull(value))
            {
                throw new ArgumentNullException(nameof(value));
            }

            _storage.Add(value);
        }
    }
}
