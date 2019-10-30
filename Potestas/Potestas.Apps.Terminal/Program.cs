using Potestas.Analizers;
using Potestas.Storages;
using System;
using System.Threading;
using System.Threading.Tasks;
using Potestas.ApplicationFrame;
using Potestas.ApplicationFrame.SourceRegistrations;
using Potestas.Observations;
using Potestas.Sources;

namespace Potestas.Apps.Terminal
{
    static class Program
    {
        private static readonly IEnergyObservationApplicationModel<FlashObservation> _app;
        private static ISourceRegistration<FlashObservation> _testRegistration;

        static Program()
        {
            _app = new ApplicationFrame<FlashObservation>();
        }

        static async Task Main()
        {
            Console.CancelKeyPress += Console_CancelKeyPress;
            _testRegistration = _app.CreateAndRegisterSource(new ConsoleSourceFactory());
            _testRegistration.AttachProcessingGroup(new ConsoleProcessingFactory());
            _testRegistration.Start().Wait();
        }

        private static void Console_CancelKeyPress(object sender, ConsoleCancelEventArgs e)
        {
            Console.WriteLine("Stopping application...");
            e.Cancel = true;
            _testRegistration.Stop();
        }
    }

    class ConsoleSourceFactory : ISourceFactory<FlashObservation>
    {
        public IEnergyObservationEventSource<FlashObservation> CreateEventSource()
        {
            throw new NotImplementedException();
        }

        public IEnergyObservationSource<FlashObservation> CreateSource()
        {
            return new RandomEnergySource();
        }
    }

    class ConsoleProcessingFactory : IProcessingFactory<FlashObservation>
    {
        public IEnergyObservationAnalizer CreateAnalizer()
        {
            return new LINQAnalizer<FlashObservation>(this.CreateStorage());
        }

        public IEnergyObservationProcessor<FlashObservation> CreateProcessor()
        {
            return new ConsoleProcessor();
        }

        public IEnergyObservationStorage<FlashObservation> CreateStorage()
        {
            return new ListStorage<FlashObservation>();
        }
    }
}
