using System;
using System.Runtime.Serialization;

namespace CbBuild.Xrm.FakeData.Exceptions
{
    [Serializable]
    public class InvalidRuleException : Exception
    {
        public InvalidRuleException()
        {
        }

        public InvalidRuleException(string message) : base(message)
        {
        }

        public InvalidRuleException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected InvalidRuleException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}