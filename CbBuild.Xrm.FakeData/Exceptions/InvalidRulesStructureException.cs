using System;
using System.Runtime.Serialization;

namespace CbBuild.Xrm.FakeData.Exceptions
{
    [Serializable]
    public class InvalidRulesStructureException : Exception
    {
        public InvalidRulesStructureException()
        {
        }

        public InvalidRulesStructureException(string message) : base(message)
        {
        }

        public InvalidRulesStructureException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected InvalidRulesStructureException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}