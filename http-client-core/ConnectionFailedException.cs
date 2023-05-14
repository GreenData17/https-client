using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace http_client_core
{
    public class ConnectionFailedException : Exception
    {
        public ConnectionFailedException(string message) 
            :base("ERROR: " + message) 
        { }
    }
}
