using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Potestas.Configuration;
using Potestas.Storages;
using Potestas.Utils;

namespace Potestas.SqlPlugin.Storages
{
    public abstract class BaseSqlStorage<T>: BaseFileStorage<T>, IEnergyObservationStorage<T>, ICollection<T>, IEnumerable<T>, IEnumerable where T : IEnergyObservation
    {
        private readonly string _connectionString;

        protected BaseSqlStorage(IConfiguration configuration)
        {
            _connectionString = configuration.GetValue("connectionString") ?? throw new ArgumentNullException(nameof(configuration));
        }

        public IEnumerator<T> GetEnumerator()
        {
            var (dataSet, adapter, sqlConnection) = this.ConfigureDataSet();
            adapter.Dispose();
            sqlConnection.Dispose();

            foreach (var dataRow in dataSet.Tables[0].AsEnumerable())
            {
                yield return this.DataRowToObservations(dataRow);
            }
        }

        public void Add(T item)
        {
            if (Validator.IsGenericTypeNull(item))
            {
                throw new ArgumentNullException(nameof(item));
            }

            var (dataSet, adapter, sqlConnection) = this.ConfigureDataSet();
            using var insertCommand = new SqlCommand(this.GetInsertQuery(item), sqlConnection);
            adapter.InsertCommand = insertCommand;
            
            dataSet.Tables[0].Rows.Add();
            adapter.Update(dataSet);

            adapter.Dispose();
            sqlConnection.Dispose();
        }

        public void Clear()
        {
            var (dataSet, adapter, sqlConnection) = this.ConfigureDataSet();
            dataSet.Tables[0].Clear();
            adapter.Update(dataSet);

            adapter.Dispose();
            sqlConnection.Dispose();
        }

        public bool Contains(T item) => base.Contains(item, this);

        public void CopyTo(T[] array, int arrayIndex) => base.CopyTo(array, arrayIndex, this);

        public bool Remove(T item)
        {
            if (Validator.IsGenericTypeNull(item))
            {
                return false;
            }

            var isDeleted = false;

            var (dataSet, adapter, sqlConnection) = this.ConfigureDataSet();
            using var deleteCommand = new SqlCommand(this.GetDeleteQuery(item), sqlConnection);
            adapter.DeleteCommand = deleteCommand;

            dataSet.Tables[0].PrimaryKey = new[] {dataSet.Tables[0].Columns["Id"]};
            var dataRow = dataSet.Tables[0].Rows.Find(item.Id);
            if (dataRow != null)
            {
                dataRow.Delete();
                adapter.Update(dataSet);
                isDeleted = true;
            }

            adapter.Dispose();
            sqlConnection.Dispose();

            return isDeleted;
        }

        public int Count
        {
            get
            {
                using var connection = new SqlConnection(_connectionString);
                using var sqlCommand = new SqlCommand(this.GetCountQuery(), connection);
                connection.Open();

                return (int) sqlCommand.ExecuteScalar();
            }
        }

        public bool IsReadOnly { get; } = false;
        public string Description => "The sql storage";

        protected abstract string GetSelectAllQuery();
        protected abstract string GetInsertQuery(T value);
        protected abstract string GetDeleteQuery(T value);
        protected abstract string GetCountQuery();
        protected abstract T DataRowToObservations(DataRow dataRow);

        IEnumerator IEnumerable.GetEnumerator() => this.GetEnumerator();

        private (DataSet, SqlDataAdapter, SqlConnection) ConfigureDataSet()
        {
            var dataSet = new DataSet("Observations");
            string query = this.GetSelectAllQuery();
            
            var sqlConnection = new SqlConnection(_connectionString);
            var adapter = new SqlDataAdapter(query, sqlConnection);
            adapter.Fill(dataSet);

            return (dataSet, adapter, sqlConnection);
        }
    }
}