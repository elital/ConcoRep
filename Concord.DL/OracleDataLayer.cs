using System;
using System.Configuration;
using Oracle.DataAccess.Client;

namespace Concord.DL
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
        
        public T Select<T>(Func<OracleDataReader, T> handleResult, string statement)
        {
            using (var connection = new OracleConnection { ConnectionString = _connectionString })
            {
                connection.Open();

                var command = new OracleCommand {Connection = connection, CommandText = statement};
                var reader = command.ExecuteReader();
                var result = handleResult(reader);

                connection.Close();

                return result;
            }
        }

        public int DmlAction(string statement)
        {
            using (var connection = new OracleConnection { ConnectionString = _connectionString })
            {
                connection.Open();

                var command = new OracleCommand {Connection = connection, CommandText = statement};
                int rowsInserted = command.ExecuteNonQuery();
                
                connection.Close();

                return rowsInserted;
            }
        }
    }
}
