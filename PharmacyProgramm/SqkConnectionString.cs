using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PharmacyProgramm
{
    class SqkConnectionString
    {
        public static string GetConnectionSqlServer()
        {
            return @"Data Source = DESKTOP-MJM6IQP\SQLEXPRESS;Initial Catalog=Pharmacy;Integrated Security=True";
        }
    }
}
