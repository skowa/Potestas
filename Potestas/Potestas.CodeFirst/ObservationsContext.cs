using System;
using Microsoft.EntityFrameworkCore;
using Potestas.CodeFirst.Entities;
using Potestas.Configuration;

namespace Potestas.CodeFirst
{
    public class ObservationsContext : DbContext
    {
        private readonly string _connectionString;

        public ObservationsContext(string connectionString)
        {
            _connectionString = connectionString;
        }

        public DbSet<FlashObservation> FlashObservations { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(_connectionString, x => x.UseNetTopologySuite());

            base.OnConfiguring(optionsBuilder);
        }
    }
}