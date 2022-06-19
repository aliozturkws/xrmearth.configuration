using System;

namespace XrmEarth.Core.Configuration.Data.Exceptions
{
    [Serializable]
    public class InitializerCoreException : Exception
    {
        public InitializerCoreException()
        {

        }

        public InitializerCoreException(string message) : base(message)
        {

        }

        public InitializerCoreException(string message, Exception innerException) : base(message, innerException)
        {

        }
    }
}
