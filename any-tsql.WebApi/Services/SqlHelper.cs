using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Http.Cors;
using any_tsql.WebApi.Models;

namespace any_tsql.WebApi.Services
{
    public static class SqlHelper
    {
        public static async Task<object> GetQueryResult(SqlRequest requestInfo)
        {
            // Simple list of arbitrary objects
            List<object> results = new List<object>();

            using (var conn = new SqlConnection(requestInfo.ConnectionString))
            using (var command = new SqlCommand(requestInfo.QueryString, conn) {CommandType = CommandType.Text})
            {

                command.CommandTimeout = requestInfo.Timeout;
                conn.Open();

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    // If anything was recieved
                    if (reader.HasRows)
                    {
                        // Keep reading while there's anything to read.
                        while (reader.Read())
                        {
                            // Create dictionary of (string -> object) kv pairs for this row
                            Dictionary<string, object> data = new Dictionary<string, object>();

                            int collisionCount = 1;

                            // Iterate the fields in this row.
                            for (int i = 0; i < reader.FieldCount; i++)
                            {
                                // Get the name of the column.
                                string name = reader.GetName(i);

                                // If the dict for this row already has this column name.
                                if (data.ContainsKey(name))
                                {
                                    // We had a collision.
                                    collisionCount++;
                                    // Use collision count to create a unique name.
                                    name = name + collisionCount;
                                }

                                // Add this field to the row dict.
                                data.Add(name, await reader.GetFieldValueAsync<object>(i));
                            }

                            results.Add(new
                            {
                                Value = reader[0],
                                Data = data
                            });
                        }
                    }
                    else
                    {
                        Console.WriteLine("No rows found.");
                    }
                }

                conn.Close();
            }

            return new
            {
                Results = results
            };
        }
    }
}