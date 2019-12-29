using System;
using System.Data.Common;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace Potestas.Web
{
	public class SqlServerHealthCheck : IHealthCheck
	{
		private static readonly string DefaultTestQuery = "SELECT 1";


		public SqlServerHealthCheck(string connectionString) : this(connectionString, DefaultTestQuery)
		{
		}

		public SqlServerHealthCheck(string connectionString, string testQuery)
		{
			if (string.IsNullOrWhiteSpace(connectionString))
			{
				throw new ArgumentNullException(nameof(connectionString));
			}

			if (string.IsNullOrWhiteSpace(testQuery))
			{
				throw new ArgumentNullException(nameof(testQuery));
			}

			ConnectionString = connectionString;
			TestQuery = testQuery;
		}

		public string ConnectionString { get; }

		public string TestQuery { get; }

		public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context,
			CancellationToken cancellationToken = new CancellationToken())
		{
			await using (var connection = new SqlConnection(ConnectionString))
			{
				try
				{
					await connection.OpenAsync(cancellationToken);

					var command = connection.CreateCommand();
					command.CommandText = TestQuery;

					await command.ExecuteNonQueryAsync(cancellationToken);
				}
				catch (DbException ex)
				{
					return new HealthCheckResult(status: context.Registration.FailureStatus, exception: ex);
				}
			}

			return HealthCheckResult.Healthy();
		}
	}
}