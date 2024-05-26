# String Repeat Benchmarks

Benchmarks for candidate algorithms for `System.String.Repeat`.

## Recommended usage

```bash
dotnet run -c Release -- --statisticalTest 5% -f "*"
```

- `--statisticalTest 5%` performs the Mann-Whitney U test with a 5% significance level.
- If you want to run a specific benchmark, you might want to replace `-f "*"` with `-f "<benchmark name>*"`. `-f "*"` runs all the benchmarks, which results in the consumption of a large amount of time.
- If you feel like this benchmark takes too long to run, you might want to add `--maxIterationCount 50` (or less value) to the command above.
- If you want to add or change runtimes, you might want to add `-r <baseline runtime> <another runtime #1> <another runtime #2>`.
- If you want to preserve past results, you might want to add `--noOverwrite` to the command above.

### Generated results

The results are stored in `./BenchmarkDotNet.Artifacts/results/`.

## Results

### vs naive `string.Create` implementations

Command:

```bash
dotnet run -c Release -- --runtimes net8.0 nativeaot8.0 --statisticalTest 5% -f "RepeatDoubleBlockSizeBench*"
```

Results:

```

BenchmarkDotNet v0.13.12, Windows 11 (10.0.22631.3593/23H2/2023Update/SunValley3)
Intel Core i7-9700 CPU 3.00GHz, 1 CPU, 8 logical and 8 physical cores
.NET SDK 8.0.300-preview.24203.14
  [Host]     : .NET 8.0.5 (8.0.524.21615), X64 RyuJIT AVX2
  DefaultJob : .NET 8.0.5 (8.0.524.21615), X64 RyuJIT AVX2


```
| Method                | Input              | Count | Mean            | Error         | StdDev        | Ratio | MannWhitney(5%) | RatioSD | Gen0     | Gen1     | Gen2     | Allocated | Alloc Ratio |
|---------------------- |------------------- |------ |----------------:|--------------:|--------------:|------:|---------------- |--------:|---------:|---------:|---------:|----------:|------------:|
| **RepeatWithCounter**     | **👍**                 | **3**     |        **22.93 ns** |      **0.236 ns** |      **0.197 ns** |  **1.56** | **Slower**          |    **0.02** |   **0.0204** |        **-** |        **-** |     **128 B** |        **3.20** |
| RepeatNoCounter       | 👍                 | 3     |        14.67 ns |      0.112 ns |      0.100 ns |  1.00 | Base            |    0.00 |   0.0063 |        - |        - |      40 B |        1.00 |
| RepeatDoubleBlockSize | 👍                 | 3     |        18.13 ns |      0.186 ns |      0.174 ns |  1.24 | Slower          |    0.01 |   0.0063 |        - |        - |      40 B |        1.00 |
|                       |                    |       |                 |               |               |       |                 |         |          |          |          |           |             |
| **RepeatWithCounter**     | **👍**                 | **7**     |        **33.49 ns** |      **0.254 ns** |      **0.226 ns** |  **1.30** | **Slower**          |    **0.01** |   **0.0229** |        **-** |        **-** |     **144 B** |        **2.57** |
| RepeatNoCounter       | 👍                 | 7     |        25.76 ns |      0.188 ns |      0.167 ns |  1.00 | Base            |    0.00 |   0.0089 |        - |        - |      56 B |        1.00 |
| RepeatDoubleBlockSize | 👍                 | 7     |        25.84 ns |      0.153 ns |      0.143 ns |  1.00 | Same            |    0.01 |   0.0089 |        - |        - |      56 B |        1.00 |
|                       |                    |       |                 |               |               |       |                 |         |          |          |          |           |             |
| **RepeatWithCounter**     | **👍**                 | **15**    |        **55.19 ns** |      **0.145 ns** |      **0.129 ns** |  **1.16** | **Slower**          |    **0.01** |   **0.0280** |        **-** |        **-** |     **176 B** |        **2.00** |
| RepeatNoCounter       | 👍                 | 15    |        47.54 ns |      0.257 ns |      0.240 ns |  1.00 | Base            |    0.00 |   0.0140 |        - |        - |      88 B |        1.00 |
| RepeatDoubleBlockSize | 👍                 | 15    |        34.24 ns |      0.106 ns |      0.100 ns |  0.72 | Faster          |    0.00 |   0.0140 |        - |        - |      88 B |        1.00 |
|                       |                    |       |                 |               |               |       |                 |         |          |          |          |           |             |
| **RepeatWithCounter**     | **👍**                 | **1023**  |     **2,842.29 ns** |      **7.374 ns** |      **6.158 ns** |  **1.02** | **Same**            |    **0.00** |   **0.6676** |        **-** |        **-** |    **4208 B** |        **1.02** |
| RepeatNoCounter       | 👍                 | 1023  |     2,799.93 ns |     15.562 ns |     13.796 ns |  1.00 | Base            |    0.00 |   0.6561 |        - |        - |    4120 B |        1.00 |
| RepeatDoubleBlockSize | 👍                 | 1023  |       236.11 ns |      3.028 ns |      2.684 ns |  0.08 | Faster          |    0.00 |   0.6561 |        - |        - |    4120 B |        1.00 |
|                       |                    |       |                 |               |               |       |                 |         |          |          |          |           |             |
| **RepeatWithCounter**     | **👍**                 | **16383** |    **43,518.69 ns** |    **125.063 ns** |    **116.984 ns** |  **1.00** | **Same**            |    **0.00** |  **10.3760** |        **-** |        **-** |   **65648 B** |        **1.00** |
| RepeatNoCounter       | 👍                 | 16383 |    43,718.76 ns |    144.522 ns |    135.186 ns |  1.00 | Base            |    0.00 |  10.3760 |        - |        - |   65560 B |        1.00 |
| RepeatDoubleBlockSize | 👍                 | 16383 |     2,696.70 ns |     53.694 ns |     52.735 ns |  0.06 | Faster          |    0.00 |  10.4141 |        - |        - |   65560 B |        1.00 |
|                       |                    |       |                 |               |               |       |                 |         |          |          |          |           |             |
| **RepeatWithCounter**     | **👨‍(...)🏿👍 [127]** | **3**     |        **55.48 ns** |      **0.203 ns** |      **0.180 ns** |  **1.16** | **Slower**          |    **0.01** |   **0.1390** |        **-** |        **-** |     **872 B** |        **1.11** |
| RepeatNoCounter       | 👨‍(...)🏿👍 [127] | 3     |        47.82 ns |      0.514 ns |      0.455 ns |  1.00 | Base            |    0.00 |   0.1249 |        - |        - |     784 B |        1.00 |
| RepeatDoubleBlockSize | 👨‍(...)🏿👍 [127] | 3     |        51.91 ns |      0.139 ns |      0.124 ns |  1.09 | Slower          |    0.01 |   0.1249 |        - |        - |     784 B |        1.00 |
|                       |                    |       |                 |               |               |       |                 |         |          |          |          |           |             |
| **RepeatWithCounter**     | **👨‍(...)🏿👍 [127]** | **7**     |       **111.86 ns** |      **0.539 ns** |      **0.478 ns** |  **1.06** | **Slower**          |    **0.00** |   **0.3009** |        **-** |        **-** |    **1888 B** |        **1.05** |
| RepeatNoCounter       | 👨‍(...)🏿👍 [127] | 7     |       105.56 ns |      0.391 ns |      0.347 ns |  1.00 | Base            |    0.00 |   0.2869 |        - |        - |    1800 B |        1.00 |
| RepeatDoubleBlockSize | 👨‍(...)🏿👍 [127] | 7     |       103.10 ns |      1.126 ns |      1.054 ns |  0.98 | Same            |    0.01 |   0.2869 |        - |        - |    1800 B |        1.00 |
|                       |                    |       |                 |               |               |       |                 |         |          |          |          |           |             |
| **RepeatWithCounter**     | **👨‍(...)🏿👍 [127]** | **15**    |       **221.60 ns** |      **0.708 ns** |      **0.662 ns** |  **1.04** | **Same**            |    **0.01** |   **0.6249** |        **-** |        **-** |    **3920 B** |        **1.02** |
| RepeatNoCounter       | 👨‍(...)🏿👍 [127] | 15    |       213.74 ns |      1.812 ns |      1.606 ns |  1.00 | Base            |    0.00 |   0.6108 |        - |        - |    3832 B |        1.00 |
| RepeatDoubleBlockSize | 👨‍(...)🏿👍 [127] | 15    |       193.22 ns |      0.850 ns |      0.754 ns |  0.90 | Faster          |    0.01 |   0.6108 |        - |        - |    3832 B |        1.00 |
|                       |                    |       |                 |               |               |       |                 |         |          |          |          |           |             |
| **RepeatWithCounter**     | **👨‍(...)🏿👍 [127]** | **1023**  |    **90,454.42 ns** |  **1,804.951 ns** |  **2,346.946 ns** |  **1.00** | **Same**            |    **0.03** |  **76.9043** |  **76.9043** |  **76.9043** |  **259978 B** |        **1.00** |
| RepeatNoCounter       | 👨‍(...)🏿👍 [127] | 1023  |    89,836.23 ns |  1,737.684 ns |  1,859.302 ns |  1.00 | Base            |    0.00 |  76.9043 |  76.9043 |  76.9043 |  259890 B |        1.00 |
| RepeatDoubleBlockSize | 👨‍(...)🏿👍 [127] | 1023  |    54,875.29 ns |  1,043.264 ns |  1,071.355 ns |  0.61 | Faster          |    0.01 |  76.9043 |  76.9043 |  76.9043 |  259890 B |        1.00 |
|                       |                    |       |                 |               |               |       |                 |         |          |          |          |           |             |
| **RepeatWithCounter**     | **👨‍(...)🏿👍 [127]** | **16383** | **1,410,675.18 ns** | **28,087.310 ns** | **44,549.450 ns** |  **1.00** | **Same**            |    **0.02** | **998.0469** | **998.0469** | **998.0469** | **4161728 B** |        **1.00** |
| RepeatNoCounter       | 👨‍(...)🏿👍 [127] | 16383 | 1,404,378.14 ns | 28,022.675 ns | 43,627.929 ns |  1.00 | Base            |    0.00 | 998.0469 | 998.0469 | 998.0469 | 4161640 B |        1.00 |
| RepeatDoubleBlockSize | 👨‍(...)🏿👍 [127] | 16383 |   810,295.29 ns | 15,833.747 ns | 15,550.857 ns |  0.58 | Faster          |    0.02 | 999.0234 | 999.0234 | 999.0234 | 4161640 B |        1.00 |
|                       |                    |       |                 |               |               |       |                 |         |          |          |          |           |             |
| **RepeatWithCounter**     | **👨‍(...)‍🧑 [128]**  | **3**     |        **56.98 ns** |      **0.318 ns** |      **0.266 ns** |  **1.20** | **Slower**          |    **0.01** |   **0.1402** |        **-** |        **-** |     **880 B** |        **1.11** |
| RepeatNoCounter       | 👨‍(...)‍🧑 [128]  | 3     |        47.63 ns |      0.326 ns |      0.289 ns |  1.00 | Base            |    0.00 |   0.1262 |        - |        - |     792 B |        1.00 |
| RepeatDoubleBlockSize | 👨‍(...)‍🧑 [128]  | 3     |        52.64 ns |      0.226 ns |      0.189 ns |  1.11 | Slower          |    0.01 |   0.1262 |        - |        - |     792 B |        1.00 |
|                       |                    |       |                 |               |               |       |                 |         |          |          |          |           |             |
| **RepeatWithCounter**     | **👨‍(...)‍🧑 [128]**  | **7**     |       **115.35 ns** |      **0.525 ns** |      **0.466 ns** |  **1.10** | **Slower**          |    **0.01** |   **0.3034** |        **-** |        **-** |    **1904 B** |        **1.05** |
| RepeatNoCounter       | 👨‍(...)‍🧑 [128]  | 7     |       104.63 ns |      0.963 ns |      0.854 ns |  1.00 | Base            |    0.00 |   0.2894 |        - |        - |    1816 B |        1.00 |
| RepeatDoubleBlockSize | 👨‍(...)‍🧑 [128]  | 7     |       103.67 ns |      1.521 ns |      1.349 ns |  0.99 | Same            |    0.01 |   0.2894 |        - |        - |    1816 B |        1.00 |
|                       |                    |       |                 |               |               |       |                 |         |          |          |          |           |             |
| **RepeatWithCounter**     | **👨‍(...)‍🧑 [128]**  | **15**    |       **236.88 ns** |      **1.065 ns** |      **0.889 ns** |  **1.06** | **Slower**          |    **0.01** |   **0.6294** |        **-** |        **-** |    **3952 B** |        **1.02** |
| RepeatNoCounter       | 👨‍(...)‍🧑 [128]  | 15    |       222.94 ns |      1.305 ns |      1.090 ns |  1.00 | Base            |    0.00 |   0.6156 |        - |        - |    3864 B |        1.00 |
| RepeatDoubleBlockSize | 👨‍(...)‍🧑 [128]  | 15    |       194.35 ns |      0.754 ns |      0.669 ns |  0.87 | Faster          |    0.01 |   0.6156 |        - |        - |    3864 B |        1.00 |
|                       |                    |       |                 |               |               |       |                 |         |          |          |          |           |             |
| **RepeatWithCounter**     | **👨‍(...)‍🧑 [128]**  | **1023**  |    **90,920.08 ns** |  **1,794.115 ns** |  **2,332.856 ns** |  **1.00** | **Same**            |    **0.04** |  **76.9043** |  **76.9043** |  **76.9043** |  **262026 B** |        **1.00** |
| RepeatNoCounter       | 👨‍(...)‍🧑 [128]  | 1023  |    91,311.83 ns |  1,652.045 ns |  2,369.313 ns |  1.00 | Base            |    0.00 |  76.9043 |  76.9043 |  76.9043 |  261938 B |        1.00 |
| RepeatDoubleBlockSize | 👨‍(...)‍🧑 [128]  | 1023  |    55,103.75 ns |  1,097.718 ns |  1,174.546 ns |  0.60 | Faster          |    0.02 |  76.9043 |  76.9043 |  76.9043 |  261938 B |        1.00 |
|                       |                    |       |                 |               |               |       |                 |         |          |          |          |           |             |
| **RepeatWithCounter**     | **👨‍(...)‍🧑 [128]**  | **16383** | **1,421,301.32 ns** | **28,223.169 ns** | **44,764.937 ns** |  **0.99** | **Same**            |    **0.04** | **998.0469** | **998.0469** | **998.0469** | **4194496 B** |        **1.00** |
| RepeatNoCounter       | 👨‍(...)‍🧑 [128]  | 16383 | 1,429,694.28 ns | 28,501.615 ns | 39,013.333 ns |  1.00 | Base            |    0.00 | 998.0469 | 998.0469 | 998.0469 | 4194408 B |        1.00 |
| RepeatDoubleBlockSize | 👨‍(...)‍🧑 [128]  | 16383 |   794,637.41 ns | 15,844.339 ns | 17,610.941 ns |  0.56 | Faster          |    0.02 | 999.0234 | 999.0234 | 999.0234 | 4194408 B |        1.00 |

### vs LINQ implementation

```bash
dotnet run -c Release -- --statisticalTest 5% -f "VSLinqTest*" --noOverwrite --maxIterationCount 40
```

```

BenchmarkDotNet v0.13.12, Windows 11 (10.0.22631.3593/23H2/2023Update/SunValley3)
Intel Core i7-9700 CPU 3.00GHz, 1 CPU, 8 logical and 8 physical cores
.NET SDK 8.0.300-preview.24203.14
  [Host]     : .NET 8.0.5 (8.0.524.21615), X64 RyuJIT AVX2
  Job-DBCPAT : .NET 8.0.5 (8.0.524.21615), X64 RyuJIT AVX2

MaxIterationCount=40  

```
| Method              | Input             | Count | Mean            | Error         | StdDev        | Ratio | MannWhitney(5%) | RatioSD |
|-------------------- |------------------ |------ |----------------:|--------------:|--------------:|------:|---------------- |--------:|
| **StringCreateFastest** | **👍**                | **3**     |        **18.82 ns** |      **0.408 ns** |      **0.382 ns** |  **1.00** | **Base**            |    **0.00** |
| Linq                | 👍                | 3     |        36.84 ns |      0.426 ns |      0.356 ns |  1.95 | Slower          |    0.04 |
|                     |                   |       |                 |               |               |       |                 |         |
| **StringCreateFastest** | **👍**                | **16383** |     **2,710.49 ns** |     **46.661 ns** |     **45.828 ns** |  **1.00** | **Base**            |    **0.00** |
| Linq                | 👍                | 16383 |    94,059.70 ns |  1,139.643 ns |  1,010.264 ns | 34.76 | Slower          |    0.65 |
|                     |                   |       |                 |               |               |       |                 |         |
| **StringCreateFastest** | **👨‍(...)‍🧑 [128]** | **3**     |        **53.35 ns** |      **1.088 ns** |      **1.069 ns** |  **1.00** | **Base**            |    **0.00** |
| Linq                | 👨‍(...)‍🧑 [128] | 3     |       123.64 ns |      2.620 ns |      4.590 ns |  2.25 | Slower          |    0.10 |
|                     |                   |       |                 |               |               |       |                 |         |
| **StringCreateFastest** | **👨‍(...)‍🧑 [128]** | **16383** |   **816,125.05 ns** |  **8,544.552 ns** |  **7,992.579 ns** |  **1.00** | **Base**            |    **0.00** |
| Linq                | 👨‍(...)‍🧑 [128] | 16383 | 1,285,999.49 ns | 24,760.555 ns | 29,475.679 ns |  1.58 | Slower          |    0.04 |
