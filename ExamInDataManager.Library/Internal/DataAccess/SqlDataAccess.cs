using Dapper;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace ExamInDataManager.Library.Internal.DataAccess
{
    internal class SqlDataAccess
    {
        private string GetConnectionString(string name)
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

        public int SaveData<U>(string storedProcedure, U parameters, string connectionStringName)
        {
            string conectionString = GetConnectionString(connectionStringName);

            using (IDbConnection connection = new SqlConnection(conectionString))
            {
                try
                {
                    int rownum = connection.Execute(storedProcedure, parameters,
                        commandType: CommandType.StoredProcedure);

                    return rownum;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }
    }
}
