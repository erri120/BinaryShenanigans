namespace BinaryShenanigans.BinaryParser.Interfaces
{
    public interface IBinaryParserConfiguration<T>
    {
        IBinaryParserBuilder<T> Configure();
    }
}
