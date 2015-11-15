using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using Oracle.ManagedDataAccess.Client;

namespace Concord.Dal
{
    public class OracleDataLayer
    {
        private static OracleDataLayer _instance;
        public static OracleDataLayer Instance
        {
            get { return _instance ?? (_instance = new OracleDataLayer()); }
            set { _instance = value; }
        }

        private readonly string _connectionString;

        private OracleDataLayer()
        {
            _connectionString = ConfigurationManager.ConnectionStrings["ET"].ConnectionString;
        }
        
        public T Select<T>(Func<OracleDataReader, T> handleResult, string statement, params string[][] parameters)
        {
            using (var connection = new OracleConnection { ConnectionString = _connectionString })
            {
                connection.Open();
                var command = new OracleCommand { Connection = connection, CommandText = statement };

                if (parameters != null)
                    foreach (var parameter in parameters)
                        command.Parameters.Add(parameter[0], parameter[1]);

                var reader = command.ExecuteReader();
                var result = handleResult(reader);

                connection.Close();

                return result;
            }
        }

        public int DmlAction(string statement, params string[][] parameters)
        {
            using (var connection = new OracleConnection { ConnectionString = _connectionString })
            {
                connection.Open();
                var command = new OracleCommand {Connection = connection, CommandText = statement};

                if (parameters != null)
                    foreach (var parameter in parameters)
                        command.Parameters.Add(parameter[0], parameter[1]);

                var rowsInserted = 0;

                using (var transaction = connection.BeginTransaction())
                {
                    command.Transaction = transaction;

                    rowsInserted = command.ExecuteNonQuery();

                    transaction.Commit();
                }
                
                connection.Close();

                return rowsInserted;
            }
        }
    }
}
