using System;
using System.Threading;
using System.Threading.Tasks;
using Potestas.Observations;

namespace Potestas.Sources
{
    public class RandomEnergySource : EnergySourceBase<FlashObservation>
    {
        private static readonly int MaxGenerationDelayMs = 5000;
        private readonly Random _random = new Random();

        protected override async Task GenerateObservations(CancellationToken cancellationToken)
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                int delay = _random.Next(MaxGenerationDelayMs);
                await Task.Delay(delay);
                await StartGeneration(cancellationToken);
            }
        }

        private async Task StartGeneration(CancellationToken cancellationToken)
        {
            await Task.Run(() =>
            {
                try
                {
                    var flashObservation = GenerateFlashObservation();
                    NewValueGenerated(this, flashObservation);
                }
                catch (Exception ex)
                {
                    PublishException(this, ex);
                    throw;
                }
            });
        }

        private FlashObservation GenerateFlashObservation()
        {
            var randomCoordinates = new Coordinates(_random.NextDouble() * Coordinates.MaxXCoordinate, _random.NextDouble() * Coordinates.MaxYCoordinate);
            var randomIntensity = _random.NextDouble() * FlashObservation.MaxIntensityValue;
            var randomDurationMs = _random.Next(10000);

            var flashObservation = new FlashObservation(randomCoordinates, randomIntensity, randomDurationMs);

            return flashObservation;
        }
    }
}