using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace jpwp_bothegate_engine
{
    public class PortsNumberIncorrectException : Exception
    {
        public PortsNumberIncorrectException()
        {
        }

        public PortsNumberIncorrectException(string message)
            : base(message)
        {
        }

        public PortsNumberIncorrectException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}