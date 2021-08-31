namespace BinaryShenanigans.BinaryParser.Steps
{
    internal abstract class AStep
    {
        public abstract StepType StepType { get; }
    }

    internal enum StepType
    {
        ReadNumerics
    }
}
