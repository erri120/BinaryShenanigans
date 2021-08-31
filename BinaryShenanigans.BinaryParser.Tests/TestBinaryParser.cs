using System.IO;
using System.Text;
using System.Threading.Tasks;
using BinaryShenanigans.BinaryParser.Interfaces;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Testing;
using Microsoft.CodeAnalysis.Text;
using Xunit;
using VerifyCS = BinaryShenanigans.BinaryParser.Tests.CSharpSourceGeneratorVerifier<BinaryShenanigans.BinaryParser.Generator.BinaryParserGenerator>;

namespace BinaryShenanigans.BinaryParser.Tests
{
    /*public class SomeClass
    {
        public uint UInt32Property { get; set; }
        public int Int32Property { get; set; }
    }

    public class SomeClassConfiguration : IBinaryParserConfiguration<SomeClass>
    {
        public IBinaryParser<SomeClass> Configure()
        {
            return BinaryParserBuilder.Configure<SomeClass>()
                .ReadUInt32(x => x.UInt32Property)
                .ReadInt32(x => x.Int32Property)
                .CreateParser();
        }
    }*/

    public class TestBinaryParser
    {
        [Fact]
        public async Task TestGenerator()
        {
            const string someClass = @"
namespace BinaryShenanigans.BinaryParser.Tests {
    public class SomeClass
    {
        public uint UInt32Property { get; set; }
        public int Int32Property { get; set; }
    }
}";
            const string configuration = @"
using BinaryShenanigans.BinaryParser.Interfaces;

namespace BinaryShenanigans.BinaryParser.Tests {
    public class SomeClassConfiguration : IBinaryParserConfiguration<SomeClass>
    {
        public IBinaryParser<SomeClass> Configure()
        {
            return BinaryParserBuilder.Configure<SomeClass>()
                .ReadUInt32(x => x.UInt32Property)
                .ReadInt32(x => x.Int32Property)
                .CreateParser();
        }
    }
}";
            //https://github.com/cezarypiatek/RoslynTestKit/blob/master/src/RoslynTestKit/ReferenceSource.cs
            /*var metadataReference = MetadataReference.CreateFromFile(typeof(BinaryParserBuilder).Assembly.Location);
            await new VerifyCS.Test
            {
                TestState =
                {
                    Sources =
                    {
                        ("SomeClass.cs", SourceText.From(someClass, Encoding.UTF8, SourceHashAlgorithm.Sha256)),
                        ("SomeClassConfiguration.cs", SourceText.From(configuration, Encoding.UTF8, SourceHashAlgorithm.Sha256))
                    },
                    ReferenceAssemblies = ReferenceAssemblies.Net.Net60,
                    AdditionalReferences = { metadataReference }
                }
            }.RunAsync();*/
        }
    }
}
