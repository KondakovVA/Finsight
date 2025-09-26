namespace Finsight.Core.Exceptions
{
    public sealed class ValidationException : FinsightException
    {
        public ValidationException(string message)
            : base(message)
        {
        }
    }
}
