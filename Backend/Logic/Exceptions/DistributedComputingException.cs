using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Logic.Exceptions
{
    public class DistributedComputingException : Exception
    {
        public DistributedComputingException()
        {
        }

        public DistributedComputingException(string? message) : base(message)
        {
        }

        public DistributedComputingException(string? message, Exception? innerException) : base(message, innerException)
        {
        }

        protected DistributedComputingException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
