# Binary Shenanigans

## Benchmarks

### Host Info

``` ini
BenchmarkDotNet=v0.13.1, OS=Windows 10.0.19043.1165 (21H1/May2021Update)
Intel Core i7-7700K CPU 4.20GHz (Kaby Lake), 1 CPU, 8 logical and 4 physical cores
.NET SDK=6.0.100-preview.7.21379.14
  [Host]     : .NET 6.0.0 (6.0.21.37719), X64 RyuJIT
  DefaultJob : .NET 6.0.0 (6.0.21.37719), X64 RyuJIT
```

### Span vs Memory vs ArraySegment

We all now that `Span<T>` is fast and you should use it but what if you can't? Well in that case you have `Memory<T>` which is similar to `Span<T>` except it is not a ref struct, however the drawback with `Memory<T>` is speed which the following benchmarks illustrates.

|                     Method |      Mean |     Error |    StdDev | Allocated |
|--------------------------- |----------:|----------:|----------:|----------:|
|               SpanCreation | 0.2019 ns | 0.0182 ns | 0.0170 ns |         - |
|             MemoryCreation | 1.4579 ns | 0.0525 ns | 0.0491 ns |         - |
|       ArraySegmentCreation | 1.4486 ns | 0.0316 ns | 0.0296 ns |         - |
|               MemoryToSpan | 1.5121 ns | 0.0588 ns | 0.0722 ns |         - |
|         ArraySegmentToSpan | 0.3475 ns | 0.0338 ns | 0.0440 ns |         - |
| ArraySegmentToReadOnlySpan | 0.2787 ns | 0.0334 ns | 0.0357 ns |         - |
|               ReadFromSpan | 0.3449 ns | 0.0327 ns | 0.0622 ns |         - |
|         ReadFromMemorySpan | 1.3612 ns | 0.0490 ns | 0.0434 ns |         - |
|   ReadFromArraySegmentSpan | 0.2880 ns | 0.0195 ns | 0.0183 ns |         - |

### MemoryStream function comparisons

|                       Method |     Mean |    Error |   StdDev |  Gen 0 | Allocated |
|----------------------------- |---------:|---------:|---------:|-------:|----------:|
|                    GetBuffer | 12.75 ns | 0.317 ns | 0.296 ns | 0.0153 |      64 B |
|                 TryGetBuffer | 17.12 ns | 0.365 ns | 0.512 ns | 0.0153 |      64 B |
| TryGetBufferFromArraySegment | 15.22 ns | 0.367 ns | 0.423 ns | 0.0153 |      64 B |
|                  SetPosition | 15.87 ns | 0.346 ns | 0.398 ns | 0.0153 |      64 B |
|                         Seek | 16.68 ns | 0.289 ns | 0.271 ns | 0.0153 |      64 B |

### Expression Benchmarks

Expressions used more and more for tasks where basic Reflection is not applicable. The problem is speed when creating complex or even simple expression trees and compiling them into a lambda function. This benchmark compares the speed and memory usage of functions with the goal of creating a new instance of a very simple class. The main takeaway of this is that compiled expressions are very fast and can come close to manually written code however the compilation step is very expensive.

|                            Method |          Mean |         Error |        StdDev |     Ratio |  RatioSD |  Gen 0 |  Gen 1 | Allocated |
|---------------------------------- |--------------:|--------------:|--------------:|----------:|---------:|-------:|-------:|----------:|
|               CreateClassManually |      3.010 ns |     0.1343 ns |     0.3677 ns |      1.00 |     0.00 | 0.0077 |      - |      32 B |
|          CreateClassWithActivator |     10.055 ns |     0.2634 ns |     0.2587 ns |      3.46 |     0.39 | 0.0076 |      - |      32 B |
|         CreateClassWithExpression | 55,188.627 ns | 1,091.2055 ns | 1,792.8818 ns | 18,701.80 | 1,924.03 | 1.2817 | 0.6104 |   5,519 B |
| CreateClassWithCompiledExpression |      3.436 ns |     0.1259 ns |     0.2918 ns |      1.15 |     0.16 | 0.0076 |      - |      32 B |

### BinaryReader Extensions Benchmarks

|       Method |     Mean |    Error |   StdDev |   Median |  Gen 0 | Allocated |
|------------- |---------:|---------:|---------:|---------:|-------:|----------:|
|    ReadInt16 | 55.16 ns | 1.235 ns | 3.584 ns | 54.26 ns | 0.0612 |     256 B |
|  ReadInt16BE | 58.23 ns | 1.161 ns | 1.589 ns | 58.08 ns | 0.0612 |     256 B |
|    ReadInt32 | 50.96 ns | 0.834 ns | 0.739 ns | 50.75 ns | 0.0612 |     256 B |
|  ReadInt32BE | 58.43 ns | 1.195 ns | 1.554 ns | 58.24 ns | 0.0612 |     256 B |
|    ReadInt64 | 52.51 ns | 0.836 ns | 0.741 ns | 52.65 ns | 0.0612 |     256 B |
|  ReadInt64BE | 58.26 ns | 1.190 ns | 1.273 ns | 58.36 ns | 0.0612 |     256 B |
|   ReadUInt16 | 50.76 ns | 0.741 ns | 0.619 ns | 50.68 ns | 0.0612 |     256 B |
| ReadUInt16BE | 58.29 ns | 1.042 ns | 1.200 ns | 58.29 ns | 0.0612 |     256 B |
|   ReadUInt32 | 51.73 ns | 0.830 ns | 0.776 ns | 51.59 ns | 0.0612 |     256 B |
| ReadUInt32BE | 58.49 ns | 1.188 ns | 1.272 ns | 58.56 ns | 0.0612 |     256 B |
|   ReadUInt64 | 51.77 ns | 0.959 ns | 0.850 ns | 51.92 ns | 0.0612 |     256 B |
| ReadUInt64BE | 56.32 ns | 0.574 ns | 0.509 ns | 56.25 ns | 0.0612 |     256 B |
|   ReadDouble | 53.32 ns | 1.073 ns | 1.792 ns | 52.48 ns | 0.0612 |     256 B |
| ReadDoubleBE | 55.66 ns | 0.603 ns | 0.504 ns | 55.50 ns | 0.0612 |     256 B |
|   ReadSingle | 52.60 ns | 0.793 ns | 0.703 ns | 52.56 ns | 0.0612 |     256 B |
| ReadSingleBE | 60.51 ns | 0.682 ns | 0.638 ns | 60.62 ns | 0.0612 |     256 B |
|     ReadHalf | 49.41 ns | 0.815 ns | 0.836 ns | 49.09 ns | 0.0612 |     256 B |
|   ReadHalfBE | 49.27 ns | 0.447 ns | 0.419 ns | 49.30 ns | 0.0612 |     256 B |


### BinaryReader Extensions with different Streams

These benchmarks show the differences in speed depending on the BaseStream of the `BinaryReader`. We can not access the internal function `InternalReadSpan` which the `BinaryReader` uses for fast access to the private `MemoryStream` buffer which is only accessible if you expose it in the constructor and use the `GetBuffer` or `TryGetBuffer` functions.

|                                           Method |          Mean |         Error |        StdDev |  Gen 0 |  Gen 1 |  Gen 2 | Allocated |
|------------------------------------------------- |--------------:|--------------:|--------------:|-------:|-------:|-------:|----------:|
|     &#39;ReadInt16 with exposed MemoryStream buffer&#39; |      49.60 ns |      0.498 ns |      0.442 ns | 0.0612 |      - |      - |     256 B |
|   &#39;ReadInt16 with unexposed MemoryStream buffer&#39; |      51.32 ns |      1.061 ns |      1.682 ns | 0.0612 |      - |      - |     256 B |
|   &#39;ReadInt16BE with exposed MemoryStream buffer&#39; |      56.50 ns |      0.901 ns |      0.753 ns | 0.0612 |      - |      - |     256 B |
| &#39;ReadInt16BE with unexposed MemoryStream buffer&#39; |      62.08 ns |      0.431 ns |      0.403 ns | 0.0688 |      - |      - |     288 B |
|                      &#39;ReadInt16 with FileStream&#39; | 249,688.18 ns | 10,119.523 ns | 29,678.808 ns | 1.2207 | 0.4883 |      - |   5,312 B |
|                    &#39;ReadInt16BE with FileStream&#39; | 246,166.55 ns | 11,260.338 ns | 33,024.621 ns | 1.2207 | 0.4883 | 0.2441 |   5,344 B |

### SpanReader vs BufferedReader vs BinaryReader

|                        Method |      Mean |     Error |    StdDev |  Gen 0 | Allocated |
|------------------------------ |----------:|----------:|----------:|-------:|----------:|
|               UsingSpanReader |  5.639 ns | 0.1394 ns | 0.2171 ns | 0.0077 |      32 B |
|           UsingBufferedReader |  7.187 ns | 0.1719 ns | 0.4017 ns | 0.0096 |      40 B |
| UsingMemoryStreamBinaryReader | 50.287 ns | 0.7685 ns | 0.6813 ns | 0.0535 |     224 B |
