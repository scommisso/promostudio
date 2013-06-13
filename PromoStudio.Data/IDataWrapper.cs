using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace PromoStudio.Data
{
    public interface IDataWrapper
    {
        int ExecuteNonQuery(string storedProcedureName, IEnumerable<IDbDataParameter> parameters);
        void ExecuteReader(string storedProcedureName, IEnumerable<IDbDataParameter> parameters, Action<System.Data.IDataReader> dataReaderAction);
        object ExecuteScalar(string storedProcedureName, IEnumerable<IDbDataParameter> parameters);
        T ExecuteScalar<T>(string storedProcedureName, IEnumerable<IDbDataParameter> parameters);
        void ExecuteStoredProc(string storedProcedure, int? commandTimeout = null, dynamic dbParams = null);
        void ExecuteStoredProc(IDbConnection connection, string storedProcedure, int? commandTimeout = null, dynamic dbParams = null, IDbTransaction transaction = null);
        //Task ExecuteStoredProcAsync(string storedProcedure, int? commandTimeout = null, dynamic dbParams = null);
        //Task ExecuteStoredProcAsync(IDbConnection connection, string storedProcedure, int? commandTimeout = null, dynamic dbParams = null, IDbTransaction transaction = null);
        IDbConnection OpenConnection();
        IEnumerable<T> QueryStoredProc<T>(string storedProcedure, int? commandTimeout = null, dynamic dbParams = null);
        IEnumerable<T> QueryStoredProc<T>(IDbConnection connection, string storedProcedure, int? commandTimeout = null, dynamic dbParams = null, IDbTransaction transaction = null);
        Task<IEnumerable<T>> QueryStoredProcAsync<T>(string storedProcedure, int? commandTimeout = null, dynamic dbParams = null);
        Task<IEnumerable<T>> QueryStoredProcAsync<T>(IDbConnection connection, string storedProcedure, int? commandTimeout = null, dynamic dbParams = null, IDbTransaction transaction = null);
    }
}
