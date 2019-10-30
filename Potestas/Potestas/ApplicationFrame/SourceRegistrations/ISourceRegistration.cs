using System.Collections.Generic;
using System.Threading.Tasks;
using Potestas.ApplicationFrame.ProcessingGroups;

namespace Potestas.ApplicationFrame.SourceRegistrations
{
    public interface ISourceRegistration<T> where T : IEnergyObservation
    {
        SourceStatus Status { get; }

        IReadOnlyCollection<IProcessingGroup<T>> ProcessingUnits { get; }

        Task Start();

        void Stop();

        void Unregister();

        IProcessingGroup<T> AttachProcessingGroup(IProcessingFactory<T> factory);
    }
}