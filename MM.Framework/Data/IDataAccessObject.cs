using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;

namespace MM.Framework.Data
{
	/// <summary>
	///     Exposes and simplifies common ADO.NET database operations
	/// </summary>
	/// <remarks>
	///     Extracted from some of Brandon's old code
	/// </remarks>
	public interface IDataAccessObject<out T> where T : DbCommand
	{
		#region public

		/// <summary>
		///     Makes a connection and executes a NonQuery on the database
		/// </summary>
		/// <param name="commandText">The SQL Query to Execute</param>
		/// <returns>
		///     The number of rows affected by the query
		/// </returns>
		int Execute(string commandText);

		/// <summary>
		///     Makes a connection and executes a NonQuery on the database
		/// </summary>
		/// <param name="commandText">The SQL Query to Execute</param>
		/// <param name="sqlCommandModifier">
		///     An <see cref="Action" /> to perform on a generated <see cref="DbCommand" />; can be used to add parameters or adjust configuration
		/// </param>
		/// <returns>
		///     The number of rows affected by the query
		/// </returns>
		int Execute(string commandText, Action<T> sqlCommandModifier);

		/// <summary>
		///     Makes a connection and returns the raw <see cref="DataTable" />
		/// </summary>
		/// <param name="commandText">The SQL Query to Execute</param>
		/// <returns>
		///     A raw <see cref="DataTable" /> with the query results
		/// </returns>
		DataTable Read(string commandText);

		/// <summary>
		///     Makes a connection and returns the raw <see cref="DataTable" />
		/// </summary>
		/// <param name="commandText">The SQL Query to Execute</param>
		/// <returns>
		///     A raw <see cref="DataSet" /> with the query results
		/// </returns>
		DataSet ReadDataSet(string commandText);

		/// <summary>
		///     Makes a connection and returns converted results using the specified conversion function
		/// </summary>
		/// <param name="commandText">The SQL Query to Execute</param>
		/// <param name="domainModelFunction">The domain model mapping function</param>
		/// <returns>A rconverted result set</returns>
		IEnumerable<TR> Read<TR>(string commandText, Func<DataRow, TR> domainModelFunction);

		/// <summary>
		///     Makes a connection and returns the raw <see cref="DataTable" />
		/// </summary>
		/// <param name="commandText">The SQL Query to Execute</param>
		/// <param name="sqlCommandModifier">
		///     An <see cref="Action" /> to perform on a generated <see cref="DbCommand" />; can be used to add parameters or adjust configuration
		/// </param>
		/// <returns>
		///     A raw <see cref="DataTable" /> with the query results
		/// </returns>
		DataTable Read(string commandText, Action<T> sqlCommandModifier);

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
		DataSet ReadDataSet(string commandText, Action<T> sqlCommandModifier);

		/// <summary>
		///     Makes a connection and returns converted results using the specified conversion function
		/// </summary>
		/// <param name="commandText">The SQL Query to Execute</param>
		/// <param name="sqlCommandModifier">
		///     An <see cref="Action" /> to perform on a generated <see cref="DbCommand" />; can be used to add parameters or adjust configuration
		/// </param>
		/// <param name="domainModelFunction">The domain model mapping function</param>
		/// <returns>A rconverted result set</returns>
		IEnumerable<TR> Read<TR>(string commandText, Action<T> sqlCommandModifier, Func<DataRow, TR> domainModelFunction);

		#endregion
	}
}