using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;

namespace MM.Framework.Data
{
	/// <summary>
	///     Exposes and simplifies common ADO.NET database operations with SQL Server
	/// </summary>
	/// <remarks>
	///     Extracted from some of Brandon's old code; interface provided to ease unit testing
	/// </remarks>
	public interface ISqlDataAccessObject : IDataAccessObject<SqlCommand>
	{
		#region public

		/// <summary>
		///     Tells ADO.NET to clear the Connection Pool for the current Connection String
		/// </summary>
		void ClearConnectionPool();

		#endregion
	}

	/// <summary>
	///     Simplifies interacting with SQL Server via ADO.NET
	/// </summary>
	public class SqlDataAccessObject : ISqlDataAccessObject
	{
		#region Fields

		/// <summary>
		///     The connection string used to establish ADO.NET connections to a SQL Server
		/// </summary>
		/// <remarks>
		///     Read-only because we expect connection pooling to kick in
		/// </remarks>
		private readonly string _connectionString;

		#endregion

		#region Constructors

		/// <summary>
		///     Creates a new instance of <see cref="SqlDataAccessObject" />
		/// </summary>
		/// <param name="connectionString">The connection string used to establish ADO.NET connections to a SQL Server</param>
		public SqlDataAccessObject(string connectionString)
		{
			_connectionString = connectionString;
		}

		#endregion

		#region ISqlDataAccessObject Members

		/// <summary>
		///     Tells ADO.NET to clear the Connection Pool for the current Connection String
		/// </summary>
		public void ClearConnectionPool()
		{
			using(var connection = new SqlConnection(_connectionString))
			{
				connection.Open();

				SqlConnection.ClearPool(connection);
			}
		}

		/// <summary>
		///     Makes a connection and executes a NonQuery on the database
		/// </summary>
		/// <param name="commandText">The SQL Query to Execute</param>
		/// <returns>
		///     The number of rows affected by the query
		/// </returns>
		public int Execute(string commandText)
		{
			return Execute(commandText, null);
		}

		/// <summary>
		///     Makes a connection and executes a NonQuery on the database
		/// </summary>
		/// <param name="commandText">The SQL Query to Execute</param>
		/// <param name="sqlCommandModifier">
		///     An <see cref="Action" /> to perform on a generated <see cref="SqlCommand" />; can be used to add parameters or adjust configuration
		/// </param>
		/// <returns>
		///     The number of rows affected by the query
		/// </returns>
		public int Execute(string commandText, Action<SqlCommand> sqlCommandModifier)
		{
			using(var connection = new SqlConnection(_connectionString))
			{
				using(var cmd = new SqlCommand(commandText))
				{
					if(sqlCommandModifier != null)
					{
						sqlCommandModifier(cmd);
					}

					cmd.Connection = connection;

					// Open Connection
					connection.Open();

					// Execute NonQuery
					var results = cmd.ExecuteNonQuery();

					// Return
					return results;
				}
			}
		}

		/// <summary>
		///     Makes a connection and returns the raw
		///     <see
		///         cref="DataTable" />
		/// </summary>
		/// <param name="commandText">The SQL Query to Execute</param>
		/// <returns>
		///     A raw <see cref="DataTable" /> with the query results
		/// </returns>
		public DataTable Read(string commandText)
		{
			return Read(commandText, null);
		}

		/// <summary>
		///     Makes a connection and returns the raw <see cref="DataTable" />
		/// </summary>
		/// <param name="commandText">The SQL Query to Execute</param>
		/// <returns>
		///     A raw <see cref="DataSet" /> with the query results
		/// </returns>
		public DataSet ReadDataSet(string commandText)
		{
			return ReadDataSet(commandText, null);
		}

		/// <summary>
		///     Makes a connection and returns converted results using the specified conversion function
		/// </summary>
		/// <param name="commandText">The SQL Query to Execute</param>
		/// <param name="domainModelFunction">The domain model mapping function</param>
		/// <returns>A rconverted result set</returns>
		public IEnumerable<TR> Read<TR>(string commandText, Func<DataRow, TR> domainModelFunction)
		{
			var results = Read(commandText);

			return toDomainModel(results, domainModelFunction);
		}

		/// <summary>
		///     Makes a connection and returns the raw
		///     <see
		///         cref="DataTable" />
		/// </summary>
		/// <param name="commandText">The SQL Query to Execute</param>
		/// <param name="sqlCommandModifier">
		///     An <see cref="Action" /> to perform on a generated <see cref="SqlCommand" />; can be used to add parameters or adjust configuration
		/// </param>
		/// <returns>
		///     A raw <see cref="DataTable" /> with the query results
		/// </returns>
		public DataTable Read(string commandText, Action<SqlCommand> sqlCommandModifier)
		{
			using(var connection = new SqlConnection(_connectionString))
			{
				using(var cmd = new SqlCommand(commandText))
				{
					if(sqlCommandModifier != null)
					{
						sqlCommandModifier(cmd);
					}

					cmd.Connection = connection;

					// Create results table
					var results = new DataTable();

					// Open Connection
					connection.Open();

					// Load results of query
					results.Load(cmd.ExecuteReader());

					// Return
					return results;
				}
			}
		}

		/// <summary>
		///     Makes a connection and returns the raw <see cref="DataTable" />
		/// </summary>
		/// <param name="commandText">The SQL Query to Execute</param>
		/// <param name="sqlCommandModifier">
		///     An <see cref="Action" /> to perform on a generated <see cref="DbCommand" />; can be used to add parameters or adjust configuration
		/// </param>
		/// <returns>
		///     A raw <see cref="DataSet" /> with the query results
		/// </returns>
		public DataSet ReadDataSet(string commandText, Action<SqlCommand> sqlCommandModifier)
		{
			using (var connection = new SqlConnection(_connectionString))
			{
				using (var cmd = new SqlCommand(commandText))
				{
					if (sqlCommandModifier != null)
					{
						sqlCommandModifier(cmd);
					}

					cmd.Connection = connection;

					// Create results table
					var results = new DataSet();

					// Open Connection
					connection.Open();

					// Load results of query
					using(var dataAdapter = new SqlDataAdapter(cmd))
					{
						dataAdapter.Fill(results);
					}

					// Return
					return results;
				}
			}
		}

		/// <summary>
		///     Makes a connection and returns converted results using the specified conversion function
		/// </summary>
		/// <param name="commandText">The SQL Query to Execute</param>
		/// <param name="sqlCommandModifier">
		///     An <see cref="Action" /> to perform on a generated <see cref="SqlCommand" />; can be used to add parameters or adjust configuration
		/// </param>
		/// <param name="domainModelFunction">The domain model mapping function</param>
		/// <returns>A rconverted result set</returns>
		public IEnumerable<TR> Read<TR>(string commandText, Action<SqlCommand> sqlCommandModifier,
		                                Func<DataRow, TR> domainModelFunction)
		{
			var results = Read(commandText, sqlCommandModifier);

			return toDomainModel(results, domainModelFunction);
		}

		#endregion

		#region Static Methods

		private static IEnumerable<TR> toDomainModel<TR>(DataTable results, Func<DataRow, TR> domainModelFunction)
		{
			return results.Rows.Cast<DataRow>().Select(domainModelFunction);
		}

		#endregion
	}
}