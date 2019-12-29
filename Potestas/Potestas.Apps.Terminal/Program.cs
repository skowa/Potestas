using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Potestas.Analizers;
using Potestas.ApplicationFrame;
using Potestas.ApplicationFrame.ProcessingGroups;
using Potestas.ApplicationFrame.SourceRegistrations;
using Potestas.Factories;
using Potestas.Observations;
using Potestas.Sources;
using Potestas.Storages;

namespace Potestas.Apps.Terminal
{
    internal static class Program
    {
        private static readonly IEnergyObservationApplicationModel<FlashObservation> App;
        private static readonly List<ISourceRegistration<FlashObservation>> SourceRegistrations = new List<ISourceRegistration<FlashObservation>>();

        static Program()
        {
            App = new ApplicationFrame<FlashObservation>();
        }

        static async Task Main()
        {
            Console.CancelKeyPress += Console_CancelKeyPress;
            Menu();
        }

        private static void Menu()
        {
            PluginLoading();
            Console.WriteLine();
            ProcessFactoriesInApp();
            Console.WriteLine();
        }

        private static void ProcessFactoriesInApp()
        {
            Console.WriteLine("Choose source factory to work with.");
            int i = 0;
            foreach (var appSourceFactory in App.SourceFactories)
            {
                ++i;
                Console.WriteLine($"{i}: {appSourceFactory.GetType()}");
            }

            var userChoiceIsValid = false;
            ISourceFactory<FlashObservation> sourceFactory = null;
            if (App.SourceFactories.Count == 0)
            {
                sourceFactory = new RandomEnergySourceFactory();
                Console.WriteLine($"The source factory is not defined in plugin, so it will be {nameof(RandomEnergySource)}");
            }
            else
            {
                while (!userChoiceIsValid)
                {
                    if (TryReadUserInput(0, App.SourceFactories.Count, out int userChoice))
                    {
                        userChoiceIsValid = true;
                        sourceFactory = App.SourceFactories.ElementAt(userChoice - 1);
                    }
                }
            }

            Console.WriteLine("Choose processing factory to work with.");
            i = 0;
            foreach (var appProcessingFactory in App.ProcessingFactories)
            {
                ++i;
                Console.WriteLine($"{i}: {appProcessingFactory.GetType()}");
            }

            IProcessingFactory<FlashObservation> processingFactory = null;
            userChoiceIsValid = false;
            while (!userChoiceIsValid!)
            {
                if (TryReadUserInput(0, App.ProcessingFactories.Count, out int userChoice))
                {
                    userChoiceIsValid = true;
                    processingFactory = App.ProcessingFactories.ElementAt(userChoice - 1);
                }
            }
            
            Console.WriteLine($"Processing source factory {sourceFactory.GetType()}...");
            ISourceRegistration<FlashObservation> sourceRegistration = App.CreateAndRegisterSource(sourceFactory);
            IProcessingGroup<FlashObservation> processingGroup = sourceRegistration.AttachProcessingGroup(processingFactory);
            SourceRegistrations.Add(sourceRegistration); 
            Console.WriteLine("Click Ctrl+C to stop generating values");
            sourceRegistration.Start().Wait();

            ProcessAnalyzer(processingGroup.Analizer);
        }

        private static void ProcessAnalyzer(IEnergyObservationAnalizer analizer)
        {
            Console.Clear();
            Console.WriteLine("Choose method");
            Console.WriteLine("1 - GetAverageEnergy");
            Console.WriteLine("2 - GetDistributionByCoordinates");
            Console.WriteLine("3 - GetDistributionByEnergyValue");
            Console.WriteLine("4 - GetDistributionByObservationTime");
            Console.WriteLine("5 - GetMaxEnergy");
            Console.WriteLine("6 - GetMaxEnergyPosition");
            Console.WriteLine("7 - GetMaxEnergyTime");
            Console.WriteLine("8 - GetMinEnergy");
            Console.WriteLine("9 - GetMinEnergyPosition");
            Console.WriteLine("10 - GetMinEnergyTime");
            Console.WriteLine("Anything else to exit");

            var userChoiceToExit = false;
            while (!userChoiceToExit)
            {
                if (TryReadUserInput(0, 10, out int userChoice))
                {
                    switch (userChoice)
                    {
                        case 1:
                            Console.WriteLine(analizer.GetAverageEnergy());
                            break;
                        case 2:
                            WriteLineDictionary(analizer.GetDistributionByCoordinates());
                            break;
                        case 3:
                            WriteLineDictionary(analizer.GetDistributionByEnergyValue());
                            break;
                        case 4:
                            WriteLineDictionary(analizer.GetDistributionByObservationTime());
                            break;
                        case 5:
                            Console.WriteLine(analizer.GetMaxEnergy());
                            break;
                        case 6:
                            Console.WriteLine(analizer.GetMaxEnergyPosition());
                            break;
                        case 7:
                            Console.WriteLine(analizer.GetMaxEnergyTime());
                            break;
                        case 8:
                            Console.WriteLine(analizer.GetMinEnergy());
                            break;
                        case 9:
                            Console.WriteLine(analizer.GetMinEnergyPosition());
                            break;
                        case 10:
                            Console.WriteLine(analizer.GetMinEnergyTime());
                            break;
                    }
                }
                else
                {
                    userChoiceToExit = true;
                }
            }
        }

        private static void PluginLoading()
        {
	        var configuration = new Configuration.Configuration();
	        string[] plugins = configuration.GetValue("plugins").Split(';');
            Console.WriteLine("Choose what plugin to use");
            for (var i = 0; i < plugins.Length; i++)
            {
	            Console.WriteLine($"{i + 1}. {plugins[i]}");
            }

            var pluginIsChosen = false;
            while(!pluginIsChosen)
            {
                if (TryReadUserInput(0, plugins.Length, out int chosenPlugin))
                {
                    App.LoadPlugin(Assembly.LoadFrom(plugins[chosenPlugin - 1]));

                    pluginIsChosen = true;
                }
            }
        }

        private static bool TryReadUserInput(int min, int max, out int validUserChoice)
        {
            return int.TryParse(Console.ReadLine(), out validUserChoice) && validUserChoice > min && validUserChoice <= max;
        }

        private static void Console_CancelKeyPress(object sender, ConsoleCancelEventArgs e)
        {
            Console.WriteLine("Stopping application...");
            e.Cancel = true;
            foreach (var sourceRegistration in SourceRegistrations)
            {
                sourceRegistration.Stop();
            }
        }

        private static void WriteLineDictionary<TKey, TValue>(IDictionary<TKey, TValue> dictionary)
        {
            foreach (var key in dictionary.Keys)
            {
                Console.WriteLine($"{key} - {dictionary[key]}");
            }
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
