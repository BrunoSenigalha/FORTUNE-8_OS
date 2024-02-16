using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FORTUNE_8OS.Utilitaries
{
    internal class ConnectionDatabase
    {
        public static string ConnectionString()
        {
            return "Server=localhost\\sqlexpress; Initial Catalog=FORTUNE8_OS_DB; Integrated Security=True; TrustServerCertificate=True";
        }
    }
}
