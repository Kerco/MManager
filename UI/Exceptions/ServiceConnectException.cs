using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UI.Exceptions
{
    class ServiceConnectException : Exception
    {
        public ServiceConnectException()
        {
        }

        public ServiceConnectException(string message) : base(message)
        {
        }

        public ServiceConnectException(string message, Exception innerException) : base(message, innerException)
        {
        }


    }
}
