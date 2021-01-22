using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cheetas3.EU.Application.Common.Exceptions
{
    public class ServiceDownException : Exception
    {
        public ServiceDownException()
            : base()
        {
        }

        public ServiceDownException(string message)
            : base(message)
        {
        }
    }
}
