using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using Oracle.ManagedDataAccess.Client;

namespace Concord.Dal
{
    public class OracleDataLayer
    {
        private OracleConnection _connection;
        private readonly string _connectionString;
        private OracleTransaction _transaction;

        private static OracleDataLayer _instance;
        public static OracleDataLayer Instance => _instance ?? (_instance = new OracleDataLayer());

        private OracleDataLayer()
        {
            _connectionString = ConfigurationManager.ConnectionStrings["ET"].ConnectionString;
        }

        public void Connect()
        {
            _connection = new OracleConnection(_connectionString);
            _connection.Open();
        }

        public void Disconnect()
        {
            Rollback();
            _connection.Close();
            _connection.Dispose();
        }

        public T Select<T>(Func<OracleDataReader, T> handleResult, string statement, params KeyValuePair<string, object>[] parameters)
        {
            CheckConnection();

            using (var command = new OracleCommand {Connection = _connection, CommandText = statement})
            {
                if (parameters != null)
                    foreach (var parameter in parameters)
                        command.Parameters.Add(parameter.Key, parameter.Value);

                using (var reader = command.ExecuteReader())
                    return handleResult(reader);
            }
        }

        public int DmlAction(string statement, params KeyValuePair<string, object>[] parameters)
        {
            CheckConnection();

            using (var command = new OracleCommand {Connection = _connection, CommandText = statement})
            {
                if (parameters != null)
                    foreach (var parameter in parameters)
                        command.Parameters.Add(parameter.Key, parameter.Value);

                command.Transaction = _transaction ?? (_transaction = _connection.BeginTransaction());
                return command.ExecuteNonQuery();
            }
        }

        internal void Commit()
        {
            _transaction?.Commit();
            _transaction?.Dispose();
            _transaction = null;
        }

        internal void Rollback()
        {
            _transaction?.Rollback();
            _transaction?.Dispose();
            _transaction = null;
        }

        private void CheckConnection()
        {
            if (_connection == null || _connection.State != ConnectionState.Open)
                throw new NotImplementedException("connection is not open");
        }
    }
}
