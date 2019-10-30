namespace Potestas.ApplicationFrame.ProcessingGroups
{
    public interface IProcessingGroup<T> where T : IEnergyObservation
    {
        IEnergyObservationProcessor<T> Processor { get; }

        IEnergyObservationStorage<T> Storage { get; }

        IEnergyObservationAnalizer Analizer { get; }

        void Detach();
    }
}