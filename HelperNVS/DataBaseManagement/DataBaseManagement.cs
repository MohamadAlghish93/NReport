using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;

namespace HelperNVS.DataBaseManagement
{
    public class DataBaseManagement
    {
        public DataBaseManagement()
        {

        }

        public List<long> GetUserPositionAttributes(string fldPosID, string connectionString)
        {
            List<long> AttrList = new List<long>();
            string queryString = "select FLDATTRIBUTES from[TSPOSPRIVILEGE] where FLDPOSID = '@PosID'";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(queryString, connection);
                command.Parameters.AddWithValue("@PosID", fldPosID);
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                try
                {
                    while (reader.Read())
                    {
                        AttrList.Add((long)reader["FLDATTRIBUTES"]);
                    }
                }
                finally
                {
                    // Always call Close when done reading.
                    reader.Close();
                }

                return AttrList;
            }
        }
    }
}
