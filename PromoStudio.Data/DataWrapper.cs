using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Threading.Tasks;
using Dapper;

namespace PromoStudio.Data
{
    /// <summary>
    /// ADO.Net wrapper. Includes core Ado.net methods, as well as Dapper.net implementation for further abstraction.
    /// </summary>
    /// <see cref="http://code.google.com/p/dapper-dot-net/"/>
    public class DataWrapper : IDataWrapper
    {
        #region Private Members

        /// <summary>
        /// Connection String Manager instance
        /// </summary>
        private IConnectionManager ConnectionManager { get; set; }

        #endregion Private Members

        #region Constructors

        public DataWrapper(IConnectionManager connectionManager)
        {
            if (connectionManager == null)
                throw new ArgumentException("connectionManager cannot be null", "connectionManager");

            this.ConnectionManager = connectionManager;
        }

        #endregion Constructors

        #region Core ADO.Net Implementation

        /// <summary>
        /// Execute stored procedure with no return result
        /// </summary>
        /// <param name="storedProcedureName"></param>
        /// <param name="setParametersAction"></param>
        public int ExecuteNonQuery(string storedProcedureName, IEnumerable<IDbDataParameter> parameters)
        {
            using (IDbConnection conn = CreateConnection())
            using (IDbCommand cmd = conn.CreateCommand())
            {
                cmd.CommandText = storedProcedureName;
                cmd.CommandType = CommandType.StoredProcedure;
                SetParameters(cmd, parameters);
                conn.Open();
                return cmd.ExecuteNonQuery();
            }
        }

        /// <summary>
        /// Execute stored procedure with single-value result
        /// </summary>
        /// <param name="storedProcedureName"></param>
        /// <param name="setParametersAction"></param>
        /// <returns></returns>
        public object ExecuteScalar(string storedProcedureName, IEnumerable<IDbDataParameter> parameters)
        {
            using (IDbConnection conn = CreateConnection())
            using (IDbCommand cmd = conn.CreateCommand())
            {
                cmd.CommandText = storedProcedureName;
                cmd.CommandType = CommandType.StoredProcedure;
                SetParameters(cmd, parameters);
                conn.Open();
                return cmd.ExecuteScalar();
            }
        }

        /// <summary>
        /// Execute stored procedure with strongly typed single-value result
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="storedProcedureName"></param>
        /// <param name="setParametersAction"></param>
        /// <returns></returns>
        public T ExecuteScalar<T>(string storedProcedureName, IEnumerable<IDbDataParameter> parameters)
        {
            return (T)Convert.ChangeType(ExecuteScalar(storedProcedureName, parameters), typeof(T));
        }

        /// <summary>
        /// Execute stored procedure and execute action with data reader
        /// </summary>
        /// <param name="storedProcedureName"></param>
        /// <param name="setParametersAction"></param>
        /// <param name="dataReaderAction"></param>
        public void ExecuteReader(string storedProcedureName, IEnumerable<IDbDataParameter> parameters, Action<IDataReader> dataReaderAction)
        {
            using (IDbConnection conn = CreateConnection())
            using (IDbCommand cmd = conn.CreateCommand())
            {
                SetParameters(cmd, parameters);
                cmd.CommandText = storedProcedureName;
                cmd.CommandType = CommandType.StoredProcedure;
                conn.Open();
                using (IDataReader rdr = cmd.ExecuteReader())
                {
                    dataReaderAction.Invoke(rdr);
                }
            }
        }

        #region Utility Methods

        private IDbCommand SetParameters(IDbCommand command, IEnumerable<IDbDataParameter> parameters)
        {
            if (parameters != null)
            {
                foreach (var param in parameters)
                    command.Parameters.Add(param);
            }

            return command;
        }

        #endregion Utility Methods

        #endregion Core ADO.Net Implementation

        #region Dapper Implementation

        #region Query Execution

        /// <summary>
        /// Query stored procedure using a new connection and return hydrated POCO objects
        /// </summary>
        /// <typeparam name="T">Type to be mapped from sql result</typeparam>
        /// <param name="storedProcedure">Name of stored procedure</param>
        /// <param name="commandTimeout">The command timeout for the stored procedure being executed. If null, uses the default command timeout</param>
        /// <param name="dbParams">Object with public instance properties matching stored procedure parameters</param>
        /// <returns>Collection of hydrated POCO objects from sql result</returns>
        public IEnumerable<T> QueryStoredProc<T>(string storedProcedure, int? commandTimeout = null, dynamic dbParams = null)
        {
            using (var connection = OpenConnection())
            {
                return QueryStoredProc<T>(connection, storedProcedure, commandTimeout, dbParams);
            }
        }

        /// <summary>
        /// Query stored procedure using provided connection and return hydrated POCO objects
        /// </summary>
        /// <typeparam name="T">Type to be mapped from sql result</typeparam>
        /// <param name="connection">Open database connection</param>
        /// <param name="storedProcedure">Name of stored procedure</param>
        /// <param name="commandTimeout">The command timeout for the stored procedure being executed. If null, uses the default command timeout</param>
        /// <param name="dbParams">Object with public instance properties matching stored procedure parameters</param>
        /// <param name="transaction">The transaction to use when executing the stored procedure</param>
        /// <returns>Collection of hydrated POCO objects from sql result</returns>
        public IEnumerable<T> QueryStoredProc<T>(IDbConnection connection, string storedProcedure, int? commandTimeout = null, dynamic dbParams = null, IDbTransaction transaction = null)
        {
            return Dapper.SqlMapper.Query<T>(
                cnn: connection,
                sql: storedProcedure,
                param: dbParams,
                commandType: CommandType.StoredProcedure,
                transaction: transaction,
                commandTimeout: commandTimeout);
        }

        #region Multiple Result Sets
        /* Multiple result sets

        /// <summary>
        /// Query stored procedure using a new connection and return multiple result sets
        /// </summary>
        /// <typeparam name="T">Type to be mapped from sql result</typeparam>
        /// <param name="storedProcedure">Name of stored procedure</param>
        /// <param name="processFunction">The function to use to process the GridReader</param>
        /// <param name="commandTimeout">The command timeout for the stored procedure being executed. If null, uses the default command timeout</param>
        /// <param name="dbParams">Object with public instance properties matching stored procedure parameters</param>
        public void QueryStoredProcMultiple(string storedProcedure, Action<Dapper.SqlMapper.GridReader> processFunction, int? commandTimeout = null, dynamic dbParams = null)
        {
            using (var connection = OpenConnection())
            {
                QueryStoredProcMultiple(connection, storedProcedure, processFunction, commandTimeout, dbParams);
            }
        }

        /// <summary>
        /// Query stored procedure using provided connection and return multiple result sets
        /// </summary>
        /// <typeparam name="T">Type to be mapped from sql result</typeparam>
        /// <param name="connection">Open database connection</param>
        /// <param name="storedProcedure">Name of stored procedure</param>
        /// <param name="processFunction">The function to use to process the GridReader</param>
        /// <param name="commandTimeout">The command timeout for the stored procedure being executed. If null, uses the default command timeout</param>
        /// <param name="dbParams">Object with public instance properties matching stored procedure parameters</param>
        /// <param name="transaction">The transaction to use when executing the stored procedure</param>
        public void QueryStoredProcMultiple(IDbConnection connection, string storedProcedure, Action<Dapper.SqlMapper.GridReader> processFunction, int? commandTimeout = null, dynamic dbParams = null, IDbTransaction transaction = null)
        {
            using (var reader = Dapper.SqlMapper.QueryMultiple(
                cnn: connection,
                sql: storedProcedure,
                param: dbParams,
                commandType: CommandType.StoredProcedure,
                transaction: transaction,
                commandTimeout: commandTimeout))
            {
                processFunction(reader);
            }
        }
        
         */
        #endregion

        #endregion Query Execution

        #region Async Query Execution

        /// <summary>
        /// Query stored procedure using a new connection and return hydrated POCO objects
        /// </summary>
        /// <typeparam name="T">Type to be mapped from sql result</typeparam>
        /// <param name="storedProcedure">Name of stored procedure</param>
        /// <param name="commandTimeout">The command timeout for the stored procedure being executed. If null, uses the default command timeout</param>
        /// <param name="dbParams">Object with public instance properties matching stored procedure parameters</param>
        /// <returns>Collection of hydrated POCO objects from sql result</returns>
        public async Task<IEnumerable<T>> QueryStoredProcAsync<T>(string storedProcedure, int? commandTimeout = null, dynamic dbParams = null)
        {
            using (var connection = OpenConnection())
            {
                return await QueryStoredProcAsync<T>(connection, storedProcedure, commandTimeout, dbParams);
            }
        }

        /// <summary>
        /// Query stored procedure using provided connection and return hydrated POCO objects
        /// </summary>
        /// <typeparam name="T">Type to be mapped from sql result</typeparam>
        /// <param name="connection">Open database connection</param>
        /// <param name="storedProcedure">Name of stored procedure</param>
        /// <param name="commandTimeout">The command timeout for the stored procedure being executed. If null, uses the default command timeout</param>
        /// <param name="dbParams">Object with public instance properties matching stored procedure parameters</param>
        /// <param name="transaction">The transaction to use when executing the stored procedure</param>
        /// <returns>Collection of hydrated POCO objects from sql result</returns>
        public async Task<IEnumerable<T>> QueryStoredProcAsync<T>(IDbConnection connection, string storedProcedure, int? commandTimeout = null, dynamic dbParams = null, IDbTransaction transaction = null)
        {
            return await Dapper.SqlMapper.QueryAsync<T>(
                cnn: connection,
                sql: storedProcedure,
                param: dbParams,
                commandType: CommandType.StoredProcedure,
                transaction: transaction,
                commandTimeout: commandTimeout);
        }

        #endregion Async Query Execution

        #region Method Execution

        /// <summary>
        /// Execute stored procedure using a new connection
        /// </summary>
        /// <param name="storedProcedure">Name of stored procedure</param>
        /// <param name="dbParams">Object with public instance properties matching stored procedure parameters</param>
        /// <param name="commandTimeout">The command timeout for the stored procedure being executed. If null, uses the default command timeout</param>
        /// <returns>ADO.Net affected count - may not be accurate, depending on Stored Proc implementation</returns>
        public void ExecuteStoredProc(string storedProcedure, int? commandTimeout = null, dynamic dbParams = null)
        {
            using (var connection = OpenConnection())
            {
                ExecuteStoredProc(connection, storedProcedure, commandTimeout, dbParams);
            }
        }

        /// <summary>
        /// Execute stored procedure using provided connection
        /// </summary>
        /// <param name="connection">Open database connection</param>
        /// <param name="storedProcedure">Name of stored procedure</param>
        /// <param name="commandTimeout">The command timeout for the stored procedure being executed. If null, uses the default command timeout</param>
        /// <param name="dbParams">Object with public instance properties matching stored procedure parameters</param>
        /// <param name="transaction">The transaction to use when executing the stored procedure</param>
        /// <returns>ADO.Net affected count - may not be accurate, depending on Stored Proc implementation</returns>
        public void ExecuteStoredProc(IDbConnection connection, string storedProcedure, int? commandTimeout = null, dynamic dbParams = null, IDbTransaction transaction = null)
        {
            Dapper.SqlMapper.Execute(
                cnn: connection,
                sql: storedProcedure,
                param: dbParams,
                commandType: CommandType.StoredProcedure,
                transaction: transaction,
                commandTimeout: commandTimeout);
        }

        #endregion Method Execution

        #region Async Method Execution

        ///// <summary>
        ///// Execute stored procedure using a new connection
        ///// </summary>
        ///// <param name="storedProcedure">Name of stored procedure</param>
        ///// <param name="dbParams">Object with public instance properties matching stored procedure parameters</param>
        ///// <param name="commandTimeout">The command timeout for the stored procedure being executed. If null, uses the default command timeout</param>
        ///// <returns>ADO.Net affected count - may not be accurate, depending on Stored Proc implementation</returns>
        //public async Task ExecuteStoredProcAsync(string storedProcedure, int? commandTimeout = null, dynamic dbParams = null)
        //{
        //    using (var connection = OpenConnection())
        //    {
        //        await ExecuteStoredProcAsync(connection, storedProcedure, commandTimeout, dbParams);
        //    }
        //}

        ///// <summary>
        ///// Execute stored procedure using provided connection
        ///// </summary>
        ///// <param name="connection">Open database connection</param>
        ///// <param name="storedProcedure">Name of stored procedure</param>
        ///// <param name="commandTimeout">The command timeout for the stored procedure being executed. If null, uses the default command timeout</param>
        ///// <param name="dbParams">Object with public instance properties matching stored procedure parameters</param>
        ///// <param name="transaction">The transaction to use when executing the stored procedure</param>
        ///// <returns>ADO.Net affected count - may not be accurate, depending on Stored Proc implementation</returns>
        //public async Task ExecuteStoredProcAsync(IDbConnection connection, string storedProcedure, int? commandTimeout = null, dynamic dbParams = null, IDbTransaction transaction = null)
        //{
        //    await Dapper.SqlMapper.Execute(
        //        cnn: connection,
        //        sql: storedProcedure,
        //        param: dbParams,
        //        commandType: CommandType.StoredProcedure,
        //        transaction: transaction,
        //        commandTimeout: commandTimeout);
        //}

        #endregion Async Method Execution

        #endregion Dapper Implementation

        #region Connection Management

        #region Public Methods

        /// <summary>
        /// Create and open a database connection
        /// </summary>
        public IDbConnection OpenConnection()
        {
            var conn = this.CreateConnection();
            conn.Open();
            return conn;
        }

        #endregion Public Methods

        #region Private Methods

        /// <summary>
        /// Create SQL Connection object
        /// </summary>
        private IDbConnection CreateConnection()
        {
            return ConnectionManager.GetConnection();
        }

        #endregion Private Methods

        #endregion Connection Management
    }
}