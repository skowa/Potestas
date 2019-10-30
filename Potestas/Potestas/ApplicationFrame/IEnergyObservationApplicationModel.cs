using System.Collections.Generic;
using System.Reflection;
using Potestas.ApplicationFrame.SourceRegistrations;

namespace Potestas.ApplicationFrame
{
    public interface IEnergyObservationApplicationModel<T> where T : IEnergyObservation
    {
        IReadOnlyCollection<ISourceFactory<T>> SourceFactories { get; }

        IReadOnlyCollection<IProcessingFactory<T>> ProcessingFactories { get; }

        IReadOnlyCollection<ISourceRegistration<T>> RegisteredSources { get; }

        void LoadPlugin(Assembly assembly);

        ISourceRegistration<T> CreateAndRegisterSource(ISourceFactory<T> factory);
    }
}