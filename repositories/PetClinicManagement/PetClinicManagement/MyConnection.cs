using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetClinicManagement
{
    class MyConnection
    {
        public SqlConnection connection;
        public MyConnection()
        {
            connection = new SqlConnection(ConfigurationManager.ConnectionStrings["CC"].ConnectionString);
        }
        public static string type;
    }
}
