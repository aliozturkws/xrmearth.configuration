using System;

namespace XrmEarth.Core.Configuration.Data.Exceptions
{
    [Serializable]
    public class InvalidTypeException : Exception
    {
        public InvalidTypeException()
        {

        }

        public InvalidTypeException(string message) : base(message)
        {

        }

        public InvalidTypeException(string message, Exception innerException) : base(message, innerException)
        {

        }
    }
}
