namespace BinaryShenanigans.BinaryParser.Steps
{
    internal abstract class ABinaryParserStep<T>
    {
        public abstract bool ShouldExecute(Parser<T> parser);
        public abstract void Execute(Parser<T> parser);
    }
}