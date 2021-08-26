using BenchmarkDotNet.Running;
using BinaryShenanigans.Benchmarks.Benchmarks;

// general benchmarks
BenchmarkRunner.Run<SpanMemoryArraySegmentBenchmarks>();
BenchmarkRunner.Run<MemoryStreamBenchmarks>();
BenchmarkRunner.Run<ExpressionBenchmarks>();
BenchmarkRunner.Run<EncodingBenchmarks>();

// extensions benchmarks
BenchmarkRunner.Run<BinaryReaderExtensionsBenchmarks>();
BenchmarkRunner.Run<BinaryReaderExtensionsStreamBenchmarks>();

// vs benchmarks
BenchmarkRunner.Run<BufferedReaderMemoryStreamBenchmarks>();
