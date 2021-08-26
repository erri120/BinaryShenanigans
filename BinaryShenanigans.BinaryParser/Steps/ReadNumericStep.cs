namespace BinaryShenanigans.BinaryParser.Steps
{
    internal class ReadNumericStep<T> : ABinaryParserStep<T>
    {
        public override bool ShouldExecute(Parser<T> parser) => true;

        public override void Execute(Parser<T> parser)
        {
            throw new System.NotImplementedException();
        }
    }
}