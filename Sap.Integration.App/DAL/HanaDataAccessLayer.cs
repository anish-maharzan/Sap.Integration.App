using Newtonsoft.Json;
using Sap.Data.Hana;
using System;
using System.Collections.Generic;
using System.Data;

namespace Sap.Integration.App.DAL
{
    public class HanaDataAccessLayer
    {
        private readonly string _connectionString;

        public HanaDataAccessLayer(string connectionString)
        {
            _connectionString = connectionString;
        }
        public DataTable ExecuteQuery(string query, List<HanaParameter> parameters = null, bool isStoredProcedure = false)
        {
            using (HanaConnection connection = new HanaConnection(_connectionString))
            {
                using (HanaCommand command = new HanaCommand(query, connection))
                {
                    if (isStoredProcedure)
                    {
                        command.CommandType = CommandType.StoredProcedure;
                    }

                    if (parameters != null && parameters.Count > 0)
                    {
                        command.Parameters.AddRange(parameters.ToArray());
                    }
                    connection.Open();
                    using (HanaDataAdapter adapter = new HanaDataAdapter(command))
                    {
                        DataTable dataTable = new DataTable();
                        adapter.Fill(dataTable);
                        return dataTable;
                    }
                }
            }
        }

        public T ExecuteQuery<T>(string query, List<HanaParameter> parameters = null, bool isStoredProcedure = false)
        {
            using (HanaConnection connection = new HanaConnection(_connectionString))
            {
                using (HanaCommand command = new HanaCommand(query, connection))
                {
                    if (isStoredProcedure)
                    {
                        command.CommandType = CommandType.StoredProcedure;
                    }
                    if (parameters != null && parameters.Count > 0)
                    {
                        command.Parameters.AddRange(parameters.ToArray());
                    }
                    connection.Open();
                    using (HanaDataAdapter adapter = new HanaDataAdapter(command))
                    {
                        DataTable dataTable = new DataTable();
                        adapter.Fill(dataTable);
                        var result = JsonConvert.DeserializeObject<T>(JsonConvert.SerializeObject(dataTable));
                        return result;
                    }
                }
            }
        }

        public void ExecuteNonQuery(string query, List<HanaParameter> parameters = null, bool isStoredProcedure = false)
        {
            using (HanaConnection connection = new HanaConnection(_connectionString))
            {
                using (HanaCommand command = new HanaCommand(query, connection))
                {
                    if (isStoredProcedure)
                    {
                        command.CommandType = CommandType.StoredProcedure;
                    }
                    if (parameters != null && parameters.Count > 0)
                    {
                        command.Parameters.AddRange(parameters.ToArray());
                    }
                    connection.Open();
                    command.ExecuteNonQuery();
                }
            }
        }

        public void BulkInsert(string tblName, DataTable dt)
        {
            using (HanaConnection dbConn = new HanaConnection(_connectionString))
            {
                dbConn.Open();
                using (HanaBulkCopy bulkCopy = new HanaBulkCopy(dbConn))
                {
                    bulkCopy.DestinationTableName = tblName;
                    try
                    {
                        foreach (DataColumn clmn in dt.Columns)
                        {
                            bulkCopy.ColumnMappings.Add(clmn.ColumnName, clmn.ColumnName);
                        }

                        bulkCopy.WriteToServer(dt);
                    }
                    catch (Exception ex)
                    {
                        //myLogger.Error("Fail to upload session data. ", ex);
                        throw ex;
                    }
                }
            }
        }
    }

}
