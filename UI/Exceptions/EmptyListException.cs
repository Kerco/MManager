using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UI.Exceptions
{
    class EmptyListException : Exception
    {
        public EmptyListException()
        {
        }

        public EmptyListException(string message) : base(message)
        {
        }

        public EmptyListException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
