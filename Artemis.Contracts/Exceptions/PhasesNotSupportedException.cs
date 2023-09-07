namespace Artemis.Contracts.Exceptions
{
    public class PhasesNotSupportedException : NotSupportedException
    {
        public PhasesNotSupportedException() : base()
        {
        }

        public PhasesNotSupportedException(string methodName, string className)
            : base($"Abstract version of method {methodName} called from {className}!")
        {
        }
    }
}
