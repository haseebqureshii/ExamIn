using Dapper;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExamInDataManager.Library.Internal.DataAccess
{
    internal class SqlDataAccess
    {
        public string GetConnectionString(string name)
        {
            return ConfigurationManager.ConnectionStrings[name].ConnectionString;
        }

        public List<T> LoadData<T, U>(string storedProcedure, U parameters, string connectionStringName)
        {
            string conectionString = GetConnectionString(connectionStringName);
            
            using (IDbConnection connection = new SqlConnection(conectionString))
            {
                List<T> rows = connection.Query<T>(storedProcedure, parameters, 
                    commandType: CommandType.StoredProcedure).ToList();

                return rows;
            }
        }

        public void SaveData<T>(string storedProcedure, T parameters, string connectionStringName)
        {
            string conectionString = GetConnectionString(connectionStringName);

            using (IDbConnection connection = new SqlConnection(conectionString))
            {
                connection.Execute(storedProcedure, parameters,
                    commandType: CommandType.StoredProcedure);
            }
        }

        public void SaveData<T, U>(string storedProcedure, T param1, U param2, string connectionStringName)
        {
            string conectionString = GetConnectionString(connectionStringName);
            List<object> parameters = new List<object>();
            parameters.Add(param1);
            parameters.Add(param2);

            using (IDbConnection connection = new SqlConnection(conectionString))
            {
                connection.Execute(storedProcedure, parameters,
                    commandType: CommandType.StoredProcedure);
            }
        }
    }
}
