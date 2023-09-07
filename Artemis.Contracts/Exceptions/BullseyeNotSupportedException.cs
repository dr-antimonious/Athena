namespace Artemis.Contracts.Exceptions
{
    public class BullseyeNotSupportedException : NotSupportedException
    {
        public BullseyeNotSupportedException() : base()
        {
        }

        public BullseyeNotSupportedException(string methodName, string className)
            : base($"Abstract version of method {methodName} called from {className}!")
        {
        }
    }
}
