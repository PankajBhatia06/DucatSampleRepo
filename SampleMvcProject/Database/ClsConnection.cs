using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Configuration;

namespace SampleMvcProject.Database
{
    public static class ClsConnections
    {
        static string my_connection1 = WebConfigurationManager.ConnectionStrings[1].ConnectionString;
        static SqlConnection my_connection = null;
        internal static SqlConnection Con()
        {
            try
            {
                if (my_connection == null)
                {
                    my_connection = new SqlConnection(my_connection1);
                    my_connection.Open();
                }
                if (my_connection.State == System.Data.ConnectionState.Closed)
                {
                    my_connection.Open();
                }
                return my_connection;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

    }
}