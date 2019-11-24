using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Potestas.CodeFirst;
using Potestas.Configuration;
using Potestas.Observations;
using Potestas.OrmPlugin.Mappers;
using Potestas.SqlHelper;
using Potestas.Storages;
using Microsoft.EntityFrameworkCore;

namespace Potestas.OrmPlugin.Storages
{
    public class FlashObservationsSqlStorageEF : BaseFileStorage<FlashObservation>, IEnergyObservationStorage<FlashObservation>
    {
        private readonly string _connectionString;

        public FlashObservationsSqlStorageEF(IConfiguration configuration)
        {
            _connectionString = configuration.GetValue("connectionStringEF") ?? throw new ArgumentNullException(nameof(configuration));
        }

        public IEnumerator<FlashObservation> GetEnumerator()
        {
            using var context = new ObservationsContext(_connectionString);
            foreach (var flashObservation in context.FlashObservations)
            {
                yield return flashObservation.ToFlashObservation();
            }
        }

        public void Add(FlashObservation item)
        {
            using var context = new ObservationsContext(_connectionString);
            context.FlashObservations.Add(item.ToFlashObservationDTO());
            context.SaveChanges();
        }

        public void Clear()
        {
            using var context = new ObservationsContext(_connectionString);
            context.Database.ExecuteSqlRaw(FlashObservationQueries.CreateDeleteAllQuery());
        }

        public bool Contains(FlashObservation item)
        {
            using var context = new ObservationsContext(_connectionString);
            return context.FlashObservations.Any(f => f.ToFlashObservation() == item);
        }

        public void CopyTo(FlashObservation[] array, int arrayIndex) => this.CopyTo(array, arrayIndex, this);

        public bool Remove(FlashObservation item)
        {
            using var context = new ObservationsContext(_connectionString);
            var itemToDelete = context.FlashObservations.FirstOrDefault(f => f.Id == item.Id);
            if (itemToDelete != null)
            {
                context.Remove(itemToDelete);
                context.SaveChanges();

                return true;
            }

            return false;
        }

        public int Count
        {
            get
            {
                using var context = new ObservationsContext(_connectionString);
                return context.FlashObservations.Count();
            }
        }

        public bool IsReadOnly  => false;
        public string Description => "Saves to database using EF";

        IEnumerator IEnumerable.GetEnumerator() => this.GetEnumerator();
    }
}