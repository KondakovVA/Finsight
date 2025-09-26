using System;

namespace Finsight.Core.Exceptions
{
    public abstract class FinsightException : Exception
    {
        protected FinsightException(string message)
            : base(message)
        {
        }

        protected FinsightException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
