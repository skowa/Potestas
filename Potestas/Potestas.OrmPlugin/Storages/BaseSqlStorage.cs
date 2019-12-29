using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.SqlClient;
using Potestas.Configuration;
using Potestas.Storages;
using Potestas.Utils;
using Dapper;

namespace Potestas.OrmPlugin.Storages
{
    public abstract class BaseSqlStorage<T> : BaseStorage<T>, IEnergyObservationStorage<T>, ICollection<T>, IEnumerable<T>, IEnumerable where T : IEnergyObservation
    {
        private readonly string _connectionString;

        protected BaseSqlStorage(IConfiguration configuration)
        {
            _connectionString = configuration.GetValue("connectionString") ?? throw new ArgumentNullException(nameof(configuration));
        }

        public IEnumerator<T> GetEnumerator()
        {
            using var sqlConnection = new SqlConnection(_connectionString);
            foreach (var energyObservation in sqlConnection.Query<T>(this.GetSelectAllQuery()))
            {
                yield return energyObservation;
            }
        }

        public void Add(T item)
        {
            if (Validator.IsGenericTypeNull(item))
            {
                throw new ArgumentNullException(nameof(item));
            }

            using var sqlConnection = new SqlConnection(_connectionString);
            sqlConnection.Execute(this.GetInsertQuery(), item);
        }

        public void Clear()
        {
           using var sqlConnection = new SqlConnection(_connectionString);
           sqlConnection.Execute(this.GetDeleteAllQuery());
        }

        public bool Contains(T item) => base.Contains(item, this);

        public void CopyTo(T[] array, int arrayIndex) => base.CopyTo(array, arrayIndex, this);

        public bool Remove(T item)
        {
            if (Validator.IsGenericTypeNull(item))
            {
                return false;
            }

            using var connection = new SqlConnection(_connectionString);
            int affectedRows = connection.Execute(this.GetDeleteQuery(), new {item.Id});

            return affectedRows > 0;
        }

        public int Count
        {
            get
            {
                using var connection = new SqlConnection(_connectionString);
                var count = connection.ExecuteScalar<int>(this.GetCountQuery());

                return count;
            }
        }

        public bool IsReadOnly { get; } = false;
        public string Description => "The sql storage using dapper";

        protected abstract string GetSelectAllQuery();
        protected abstract string GetInsertQuery();
        protected abstract string GetDeleteQuery();
        protected abstract string GetCountQuery();
        protected abstract string GetDeleteAllQuery();

        IEnumerator IEnumerable.GetEnumerator() => this.GetEnumerator();
    }
}