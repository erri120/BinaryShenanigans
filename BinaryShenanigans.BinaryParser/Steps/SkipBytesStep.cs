namespace BinaryShenanigans.BinaryParser.Steps
{
    internal class SkipBytesStep<T> : ABinaryParserStep<T>
    {
        private readonly ulong _count;
        
        public SkipBytesStep(ulong count)
        {
            _count = count;
        }

        public override bool ShouldExecute(Parser<T> parser) => true;

        public override void Execute(Parser<T> parser)
        {
            parser.SkipBytes(_count);
        }
    }
}