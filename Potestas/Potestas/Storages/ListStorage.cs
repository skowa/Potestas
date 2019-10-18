using System.Collections.Generic;

namespace Potestas.Storages
{
    public class ListStorage<T> : List<T>, IEnergyObservationStorage<T> where T : IEnergyObservation
    {
        public string Description => "Simple in-memory storage of energy observations";
    }
}
