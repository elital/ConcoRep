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
            _connection.Close();
        }

        public T Select<T>(Func<OracleDataReader, T> handleResult, string statement, params KeyValuePair<string, object>[] parameters)
        {
            CheckConnection();

            var command = new OracleCommand {Connection = _connection, CommandText = statement};

            if (parameters != null)
                foreach (var parameter in parameters)
                    command.Parameters.Add(parameter.Key, parameter.Value);

            var reader = command.ExecuteReader();
            var result = handleResult(reader);
            
            return result;
        }

        public int DmlAction(string statement, params KeyValuePair<string, object>[] parameters)
        {
            CheckConnection();

            var command = new OracleCommand {Connection = _connection, CommandText = statement};

            if (parameters != null)
                foreach (var parameter in parameters)
                    command.Parameters.Add(parameter.Key, parameter.Value);

            int rowsInserted;

            using (var transaction = _connection.BeginTransaction())
            {
                command.Transaction = transaction;

                rowsInserted = command.ExecuteNonQuery();

                transaction.Commit();
            }

            return rowsInserted;
        }

        private void CheckConnection()
        {
            if (_connection == null || _connection.State != ConnectionState.Open)
                throw new NotImplementedException("connection is not open");
        }
    }
}
