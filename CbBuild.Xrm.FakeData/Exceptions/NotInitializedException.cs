using System;
using System.Runtime.Serialization;

namespace CbBuild.Xrm.FakeData.Exceptions
{
    [Serializable]
    public class NotInitializedException : Exception
    {
        public NotInitializedException() : base()
        {
        }

        public NotInitializedException(string message) : base(message)
        {
        }

        public NotInitializedException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected NotInitializedException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}