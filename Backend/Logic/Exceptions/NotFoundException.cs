﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Logic.Exceptions
{
    [Serializable]
    public class NotFoundException : DistributedComputingException
    {        
        public NotFoundException()
        {
        }

        public NotFoundException(string? message) : base(message)
        {
        }

        public NotFoundException(string? message, Exception? innerException) : base(message, innerException)
        {
        }

        protected NotFoundException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
        
    }
}
