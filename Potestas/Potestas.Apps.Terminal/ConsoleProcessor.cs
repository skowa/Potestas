using System;
using Potestas.Observations;

namespace Potestas.Apps.Terminal
{
    class ConsoleProcessor : IEnergyObservationProcessor<FlashObservation>
    {
        public string Description => "Logs all observations to console";

        public void OnCompleted()
        {
            Console.WriteLine("Processing completed");
        }

        public void OnError(Exception error)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(error);
            Console.ForegroundColor = ConsoleColor.White;
        }

        public void OnNext(FlashObservation value)
        {
            Console.WriteLine(value);
            Console.WriteLine();
        }
    }
}
