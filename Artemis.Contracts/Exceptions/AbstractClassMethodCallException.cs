namespace Artemis.Contracts.Exceptions
{
    public class AbstractClassMethodCallException : Exception
    {
        public AbstractClassMethodCallException() : base()
        {
        }

        public AbstractClassMethodCallException(string methodName, string className)
            : base($"Abstract version of method {methodName} called from {className}!")
        {
        }
    }
}
