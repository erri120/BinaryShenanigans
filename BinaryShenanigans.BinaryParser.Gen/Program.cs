using System.Threading.Tasks;
using BinaryShenanigans.BinaryParser.Gen.Commands;
using Spectre.Console.Cli;

namespace BinaryShenanigans.BinaryParser.Gen
{
    public static class Program
    {
        public static Task<int> Main(string[] args)
        {
            var app = new CommandApp<GenerateCommand>();
            return app.RunAsync(args);
        }
    }
}
