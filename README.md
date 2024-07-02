# String Repeat Benchmarks

Benchmarks for candidate algorithms for `System.String.Repeat`.

## Recommended usage

```bash
dotnet run -c Release -- --statisticalTest 5% -f "*"
```

- `--statisticalTest 5%` performs the Mann-Whitney U test with a 5% significance level.
  - If you change it to 10%, slighter differences in results will be treated as statistically significant.
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
dotnet run -c Release -- --runtimes net8.0 nativeaot8.0 --statisticalTest 5% -f "RepeatDoubleBlockSizeBench*" --noOverwrite -m
```

Results:

```

BenchmarkDotNet v0.13.12, Windows 11 (10.0.22631.3593/23H2/2023Update/SunValley3)
Intel Core i7-9700 CPU 3.00GHz, 1 CPU, 8 logical and 8 physical cores
.NET SDK 8.0.300-preview.24203.14
  [Host]     : .NET 8.0.6 (8.0.624.26715), X64 RyuJIT AVX2
  Job-JZCTOK : .NET 8.0.6 (8.0.624.26715), X64 RyuJIT AVX2
  Job-ARTGDG : .NET 8.0.2, X64 NativeAOT AVX2


```
| Method                | Runtime       | Input              | Count | Mean            | Error         | StdDev        | Ratio | MannWhitney(5%) | RatioSD | Gen0     | Gen1     | Gen2     | Allocated | Alloc Ratio |
|---------------------- |-------------- |------------------- |------ |----------------:|--------------:|--------------:|------:|---------------- |--------:|---------:|---------:|---------:|----------:|------------:|
| **RepeatWithCounter**     | **.NET 8.0**      | **👍**                 | **3**     |        **22.51 ns** |      **0.477 ns** |      **0.423 ns** |  **1.55** | **Slower**          |    **0.03** |   **0.0204** |        **-** |        **-** |     **128 B** |        **3.20** |
| RepeatNoCounter       | .NET 8.0      | 👍                 | 3     |        14.51 ns |      0.219 ns |      0.194 ns |  1.00 | Base            |    0.00 |   0.0063 |        - |        - |      40 B |        1.00 |
| RepeatPowerShell      | .NET 8.0      | 👍                 | 3     |        16.76 ns |      0.158 ns |      0.132 ns |  1.16 | Slower          |    0.02 |   0.0063 |        - |        - |      40 B |        1.00 |
| RepeatDoubleBlockSize | .NET 8.0      | 👍                 | 3     |        17.74 ns |      0.304 ns |      0.455 ns |  1.22 | Slower          |    0.04 |   0.0063 |        - |        - |      40 B |        1.00 |
| RepeatWithCounter     | NativeAOT 8.0 | 👍                 | 3     |        27.16 ns |      0.508 ns |      0.475 ns |  1.87 | Slower          |    0.02 |   0.0179 |        - |        - |     112 B |        2.80 |
| RepeatNoCounter       | NativeAOT 8.0 | 👍                 | 3     |        17.75 ns |      0.074 ns |      0.058 ns |  1.22 | Slower          |    0.02 |   0.0063 |        - |        - |      40 B |        1.00 |
| RepeatPowerShell      | NativeAOT 8.0 | 👍                 | 3     |        19.38 ns |      0.435 ns |      0.565 ns |  1.34 | Slower          |    0.04 |   0.0063 |        - |        - |      40 B |        1.00 |
| RepeatDoubleBlockSize | NativeAOT 8.0 | 👍                 | 3     |        20.51 ns |      0.142 ns |      0.133 ns |  1.41 | Slower          |    0.02 |   0.0063 |        - |        - |      40 B |        1.00 |
|                       |               |                    |       |                 |               |               |       |                 |         |          |          |          |           |             |
| **RepeatWithCounter**     | **.NET 8.0**      | **👍**                 | **7**     |        **32.67 ns** |      **0.371 ns** |      **0.290 ns** |  **1.29** | **Slower**          |    **0.01** |   **0.0229** |        **-** |        **-** |     **144 B** |        **2.57** |
| RepeatNoCounter       | .NET 8.0      | 👍                 | 7     |        25.44 ns |      0.152 ns |      0.142 ns |  1.00 | Base            |    0.00 |   0.0089 |        - |        - |      56 B |        1.00 |
| RepeatPowerShell      | .NET 8.0      | 👍                 | 7     |        28.28 ns |      0.226 ns |      0.177 ns |  1.11 | Slower          |    0.01 |   0.0089 |        - |        - |      56 B |        1.00 |
| RepeatDoubleBlockSize | .NET 8.0      | 👍                 | 7     |        25.93 ns |      0.117 ns |      0.109 ns |  1.02 | Same            |    0.01 |   0.0089 |        - |        - |      56 B |        1.00 |
| RepeatWithCounter     | NativeAOT 8.0 | 👍                 | 7     |        41.40 ns |      0.162 ns |      0.143 ns |  1.63 | Slower          |    0.01 |   0.0204 |        - |        - |     128 B |        2.29 |
| RepeatNoCounter       | NativeAOT 8.0 | 👍                 | 7     |        28.66 ns |      0.089 ns |      0.075 ns |  1.13 | Slower          |    0.01 |   0.0089 |        - |        - |      56 B |        1.00 |
| RepeatPowerShell      | NativeAOT 8.0 | 👍                 | 7     |        30.81 ns |      0.106 ns |      0.094 ns |  1.21 | Slower          |    0.01 |   0.0089 |        - |        - |      56 B |        1.00 |
| RepeatDoubleBlockSize | NativeAOT 8.0 | 👍                 | 7     |        27.50 ns |      0.157 ns |      0.147 ns |  1.08 | Slower          |    0.01 |   0.0089 |        - |        - |      56 B |        1.00 |
|                       |               |                    |       |                 |               |               |       |                 |         |          |          |          |           |             |
| **RepeatWithCounter**     | **.NET 8.0**      | **👍**                 | **15**    |        **54.41 ns** |      **0.171 ns** |      **0.133 ns** |  **1.17** | **Slower**          |    **0.00** |   **0.0280** |        **-** |        **-** |     **176 B** |        **2.00** |
| RepeatNoCounter       | .NET 8.0      | 👍                 | 15    |        46.42 ns |      0.156 ns |      0.139 ns |  1.00 | Base            |    0.00 |   0.0140 |        - |        - |      88 B |        1.00 |
| RepeatPowerShell      | .NET 8.0      | 👍                 | 15    |        51.56 ns |      0.121 ns |      0.113 ns |  1.11 | Slower          |    0.00 |   0.0140 |        - |        - |      88 B |        1.00 |
| RepeatDoubleBlockSize | .NET 8.0      | 👍                 | 15    |        33.64 ns |      0.361 ns |      0.320 ns |  0.72 | Faster          |    0.01 |   0.0140 |        - |        - |      88 B |        1.00 |
| RepeatWithCounter     | NativeAOT 8.0 | 👍                 | 15    |        75.29 ns |      0.265 ns |      0.248 ns |  1.62 | Slower          |    0.01 |   0.0254 |        - |        - |     160 B |        1.82 |
| RepeatNoCounter       | NativeAOT 8.0 | 👍                 | 15    |        49.84 ns |      0.196 ns |      0.153 ns |  1.07 | Slower          |    0.00 |   0.0140 |        - |        - |      88 B |        1.00 |
| RepeatPowerShell      | NativeAOT 8.0 | 👍                 | 15    |        54.86 ns |      0.752 ns |      0.704 ns |  1.18 | Slower          |    0.02 |   0.0140 |        - |        - |      88 B |        1.00 |
| RepeatDoubleBlockSize | NativeAOT 8.0 | 👍                 | 15    |        34.65 ns |      0.134 ns |      0.118 ns |  0.75 | Faster          |    0.00 |   0.0140 |        - |        - |      88 B |        1.00 |
|                       |               |                    |       |                 |               |               |       |                 |         |          |          |          |           |             |
| **RepeatWithCounter**     | **.NET 8.0**      | **👍**                 | **1023**  |     **2,791.55 ns** |      **7.098 ns** |      **6.292 ns** |  **1.00** | **Same**            |    **0.01** |   **0.6676** |        **-** |        **-** |    **4208 B** |        **1.02** |
| RepeatNoCounter       | .NET 8.0      | 👍                 | 1023  |     2,793.20 ns |     46.157 ns |     40.917 ns |  1.00 | Base            |    0.00 |   0.6561 |        - |        - |    4120 B |        1.00 |
| RepeatPowerShell      | .NET 8.0      | 👍                 | 1023  |     2,973.99 ns |     13.018 ns |     11.540 ns |  1.06 | Slower          |    0.02 |   0.6561 |        - |        - |    4120 B |        1.00 |
| RepeatDoubleBlockSize | .NET 8.0      | 👍                 | 1023  |       228.47 ns |      1.963 ns |      1.639 ns |  0.08 | Faster          |    0.00 |   0.6564 |        - |        - |    4120 B |        1.00 |
| RepeatWithCounter     | NativeAOT 8.0 | 👍                 | 1023  |     3,522.25 ns |      5.887 ns |      4.596 ns |  1.26 | Slower          |    0.02 |   0.6676 |        - |        - |    4192 B |        1.02 |
| RepeatNoCounter       | NativeAOT 8.0 | 👍                 | 1023  |     2,792.20 ns |      9.839 ns |      8.216 ns |  1.00 | Same            |    0.01 |   0.6561 |        - |        - |    4120 B |        1.00 |
| RepeatPowerShell      | NativeAOT 8.0 | 👍                 | 1023  |     3,055.63 ns |     24.467 ns |     20.431 ns |  1.09 | Slower          |    0.02 |   0.6561 |        - |        - |    4120 B |        1.00 |
| RepeatDoubleBlockSize | NativeAOT 8.0 | 👍                 | 1023  |       220.51 ns |      1.196 ns |      0.999 ns |  0.08 | Faster          |    0.00 |   0.6564 |        - |        - |    4120 B |        1.00 |
|                       |               |                    |       |                 |               |               |       |                 |         |          |          |          |           |             |
| **RepeatWithCounter**     | **.NET 8.0**      | **👍**                 | **16383** |    **42,728.05 ns** |    **159.021 ns** |    **148.749 ns** |  **1.00** | **Same**            |    **0.00** |  **10.3760** |        **-** |        **-** |   **65648 B** |        **1.00** |
| RepeatNoCounter       | .NET 8.0      | 👍                 | 16383 |    42,829.89 ns |    158.795 ns |    140.768 ns |  1.00 | Base            |    0.00 |  10.3760 |        - |        - |   65560 B |        1.00 |
| RepeatPowerShell      | .NET 8.0      | 👍                 | 16383 |    46,095.59 ns |    144.517 ns |    135.182 ns |  1.08 | Slower          |    0.01 |  10.3760 |        - |        - |   65560 B |        1.00 |
| RepeatDoubleBlockSize | .NET 8.0      | 👍                 | 16383 |     2,626.04 ns |      8.441 ns |      7.483 ns |  0.06 | Faster          |    0.00 |  10.4141 |        - |        - |   65560 B |        1.00 |
| RepeatWithCounter     | NativeAOT 8.0 | 👍                 | 16383 |    54,912.13 ns |    349.847 ns |    310.130 ns |  1.28 | Slower          |    0.01 |  10.3760 |        - |        - |   65632 B |        1.00 |
| RepeatNoCounter       | NativeAOT 8.0 | 👍                 | 16383 |    43,340.96 ns |    114.596 ns |    107.193 ns |  1.01 | Same            |    0.00 |  10.3760 |        - |        - |   65560 B |        1.00 |
| RepeatPowerShell      | NativeAOT 8.0 | 👍                 | 16383 |    47,097.00 ns |    115.188 ns |    107.747 ns |  1.10 | Slower          |    0.00 |  10.3760 |        - |        - |   65560 B |        1.00 |
| RepeatDoubleBlockSize | NativeAOT 8.0 | 👍                 | 16383 |     2,564.98 ns |     24.929 ns |     22.099 ns |  0.06 | Faster          |    0.00 |  10.4141 |        - |        - |   65560 B |        1.00 |
|                       |               |                    |       |                 |               |               |       |                 |         |          |          |          |           |             |
| **RepeatWithCounter**     | **.NET 8.0**      | **👨‍(...)🏿👍 [127]** | **3**     |        **54.22 ns** |      **0.216 ns** |      **0.192 ns** |  **1.16** | **Slower**          |    **0.01** |   **0.1390** |        **-** |        **-** |     **872 B** |        **1.11** |
| RepeatNoCounter       | .NET 8.0      | 👨‍(...)🏿👍 [127] | 3     |        46.86 ns |      0.316 ns |      0.281 ns |  1.00 | Base            |    0.00 |   0.1249 |        - |        - |     784 B |        1.00 |
| RepeatPowerShell      | .NET 8.0      | 👨‍(...)🏿👍 [127] | 3     |        49.16 ns |      0.764 ns |      0.715 ns |  1.05 | Same            |    0.02 |   0.1249 |        - |        - |     784 B |        1.00 |
| RepeatDoubleBlockSize | .NET 8.0      | 👨‍(...)🏿👍 [127] | 3     |        50.09 ns |      0.223 ns |      0.174 ns |  1.07 | Slower          |    0.01 |   0.1249 |        - |        - |     784 B |        1.00 |
| RepeatWithCounter     | NativeAOT 8.0 | 👨‍(...)🏿👍 [127] | 3     |        59.22 ns |      0.347 ns |      0.307 ns |  1.26 | Slower          |    0.01 |   0.1364 |        - |        - |     856 B |        1.09 |
| RepeatNoCounter       | NativeAOT 8.0 | 👨‍(...)🏿👍 [127] | 3     |        48.00 ns |      0.984 ns |      0.921 ns |  1.03 | Same            |    0.02 |   0.1249 |        - |        - |     784 B |        1.00 |
| RepeatPowerShell      | NativeAOT 8.0 | 👨‍(...)🏿👍 [127] | 3     |        48.47 ns |      0.182 ns |      0.170 ns |  1.03 | Same            |    0.01 |   0.1249 |        - |        - |     784 B |        1.00 |
| RepeatDoubleBlockSize | NativeAOT 8.0 | 👨‍(...)🏿👍 [127] | 3     |        50.61 ns |      0.376 ns |      0.334 ns |  1.08 | Slower          |    0.01 |   0.1249 |        - |        - |     784 B |        1.00 |
|                       |               |                    |       |                 |               |               |       |                 |         |          |          |          |           |             |
| **RepeatWithCounter**     | **.NET 8.0**      | **👨‍(...)🏿👍 [127]** | **7**     |       **109.20 ns** |      **0.474 ns** |      **0.420 ns** |  **1.08** | **Slower**          |    **0.01** |   **0.3009** |        **-** |        **-** |    **1888 B** |        **1.05** |
| RepeatNoCounter       | .NET 8.0      | 👨‍(...)🏿👍 [127] | 7     |       101.28 ns |      0.474 ns |      0.396 ns |  1.00 | Base            |    0.00 |   0.2869 |        - |        - |    1800 B |        1.00 |
| RepeatPowerShell      | .NET 8.0      | 👨‍(...)🏿👍 [127] | 7     |       105.70 ns |      1.287 ns |      1.140 ns |  1.04 | Same            |    0.01 |   0.2869 |        - |        - |    1800 B |        1.00 |
| RepeatDoubleBlockSize | .NET 8.0      | 👨‍(...)🏿👍 [127] | 7     |        99.05 ns |      0.373 ns |      0.331 ns |  0.98 | Same            |    0.01 |   0.2869 |        - |        - |    1800 B |        1.00 |
| RepeatWithCounter     | NativeAOT 8.0 | 👨‍(...)🏿👍 [127] | 7     |       112.99 ns |      1.343 ns |      1.257 ns |  1.11 | Slower          |    0.01 |   0.2984 |        - |        - |    1872 B |        1.04 |
| RepeatNoCounter       | NativeAOT 8.0 | 👨‍(...)🏿👍 [127] | 7     |        98.73 ns |      0.279 ns |      0.233 ns |  0.97 | Same            |    0.00 |   0.2869 |        - |        - |    1800 B |        1.00 |
| RepeatPowerShell      | NativeAOT 8.0 | 👨‍(...)🏿👍 [127] | 7     |       100.56 ns |      0.283 ns |      0.265 ns |  0.99 | Same            |    0.00 |   0.2869 |        - |        - |    1800 B |        1.00 |
| RepeatDoubleBlockSize | NativeAOT 8.0 | 👨‍(...)🏿👍 [127] | 7     |        99.89 ns |      0.881 ns |      0.824 ns |  0.99 | Same            |    0.01 |   0.2869 |        - |        - |    1800 B |        1.00 |
|                       |               |                    |       |                 |               |               |       |                 |         |          |          |          |           |             |
| **RepeatWithCounter**     | **.NET 8.0**      | **👨‍(...)🏿👍 [127]** | **15**    |       **215.98 ns** |      **1.855 ns** |      **1.645 ns** |  **1.04** | **Same**            |    **0.01** |   **0.6249** |        **-** |        **-** |    **3920 B** |        **1.02** |
| RepeatNoCounter       | .NET 8.0      | 👨‍(...)🏿👍 [127] | 15    |       207.51 ns |      1.758 ns |      1.644 ns |  1.00 | Base            |    0.00 |   0.6108 |        - |        - |    3832 B |        1.00 |
| RepeatPowerShell      | .NET 8.0      | 👨‍(...)🏿👍 [127] | 15    |       214.36 ns |      2.488 ns |      2.206 ns |  1.03 | Same            |    0.01 |   0.6108 |        - |        - |    3832 B |        1.00 |
| RepeatDoubleBlockSize | .NET 8.0      | 👨‍(...)🏿👍 [127] | 15    |       185.89 ns |      0.673 ns |      0.562 ns |  0.90 | Faster          |    0.01 |   0.6108 |        - |        - |    3832 B |        1.00 |
| RepeatWithCounter     | NativeAOT 8.0 | 👨‍(...)🏿👍 [127] | 15    |       229.69 ns |      1.187 ns |      0.991 ns |  1.11 | Slower          |    0.01 |   0.6223 |        - |        - |    3904 B |        1.02 |
| RepeatNoCounter       | NativeAOT 8.0 | 👨‍(...)🏿👍 [127] | 15    |       213.25 ns |      0.753 ns |      0.667 ns |  1.03 | Same            |    0.01 |   0.6108 |        - |        - |    3832 B |        1.00 |
| RepeatPowerShell      | NativeAOT 8.0 | 👨‍(...)🏿👍 [127] | 15    |       216.04 ns |      1.044 ns |      0.977 ns |  1.04 | Same            |    0.01 |   0.6108 |        - |        - |    3832 B |        1.00 |
| RepeatDoubleBlockSize | NativeAOT 8.0 | 👨‍(...)🏿👍 [127] | 15    |       186.10 ns |      2.269 ns |      2.122 ns |  0.90 | Faster          |    0.01 |   0.6108 |        - |        - |    3832 B |        1.00 |
|                       |               |                    |       |                 |               |               |       |                 |         |          |          |          |           |             |
| **RepeatWithCounter**     | **.NET 8.0**      | **👨‍(...)🏿👍 [127]** | **1023**  |    **88,244.74 ns** |  **1,748.785 ns** |  **1,795.874 ns** |  **1.00** | **Same**            |    **0.04** |  **76.9043** |  **76.9043** |  **76.9043** |  **259978 B** |        **1.00** |
| RepeatNoCounter       | .NET 8.0      | 👨‍(...)🏿👍 [127] | 1023  |    88,626.18 ns |  1,732.366 ns |  2,190.888 ns |  1.00 | Base            |    0.00 |  76.9043 |  76.9043 |  76.9043 |  259890 B |        1.00 |
| RepeatPowerShell      | .NET 8.0      | 👨‍(...)🏿👍 [127] | 1023  |    88,745.90 ns |  1,722.190 ns |  2,178.019 ns |  1.00 | Same            |    0.04 |  76.9043 |  76.9043 |  76.9043 |  259890 B |        1.00 |
| RepeatDoubleBlockSize | .NET 8.0      | 👨‍(...)🏿👍 [127] | 1023  |    54,530.03 ns |    948.998 ns |    887.694 ns |  0.62 | Faster          |    0.02 |  76.9043 |  76.9043 |  76.9043 |  259890 B |        1.00 |
| RepeatWithCounter     | NativeAOT 8.0 | 👨‍(...)🏿👍 [127] | 1023  |   115,257.02 ns |  2,054.126 ns |  1,921.431 ns |  1.30 | Slower          |    0.04 |  76.9043 |  76.9043 |  76.9043 |  259962 B |        1.00 |
| RepeatNoCounter       | NativeAOT 8.0 | 👨‍(...)🏿👍 [127] | 1023  |   115,474.51 ns |  2,187.006 ns |  2,147.932 ns |  1.31 | Slower          |    0.04 |  76.9043 |  76.9043 |  76.9043 |  259890 B |        1.00 |
| RepeatPowerShell      | NativeAOT 8.0 | 👨‍(...)🏿👍 [127] | 1023  |   114,236.56 ns |  1,146.981 ns |  1,016.769 ns |  1.29 | Slower          |    0.04 |  76.9043 |  76.9043 |  76.9043 |  259890 B |        1.00 |
| RepeatDoubleBlockSize | NativeAOT 8.0 | 👨‍(...)🏿👍 [127] | 1023  |    81,698.98 ns |    842.517 ns |    746.870 ns |  0.92 | Faster          |    0.03 |  76.9043 |  76.9043 |  76.9043 |  259890 B |        1.00 |
|                       |               |                    |       |                 |               |               |       |                 |         |          |          |          |           |             |
| **RepeatWithCounter**     | **.NET 8.0**      | **👨‍(...)🏿👍 [127]** | **16383** | **1,389,739.71 ns** | **26,451.350 ns** | **28,302.648 ns** |  **1.00** | **Same**            |    **0.03** | **998.0469** | **998.0469** | **998.0469** | **4161728 B** |        **1.00** |
| RepeatNoCounter       | .NET 8.0      | 👨‍(...)🏿👍 [127] | 16383 | 1,390,918.63 ns | 27,066.098 ns | 31,169.359 ns |  1.00 | Base            |    0.00 | 998.0469 | 998.0469 | 998.0469 | 4161640 B |        1.00 |
| RepeatPowerShell      | .NET 8.0      | 👨‍(...)🏿👍 [127] | 16383 | 1,383,114.93 ns | 20,202.666 ns | 17,909.137 ns |  0.99 | Same            |    0.02 | 998.0469 | 998.0469 | 998.0469 | 4161640 B |        1.00 |
| RepeatDoubleBlockSize | .NET 8.0      | 👨‍(...)🏿👍 [127] | 16383 |   791,492.66 ns | 15,417.546 ns | 17,754.869 ns |  0.57 | Faster          |    0.02 | 999.0234 | 999.0234 | 999.0234 | 4161640 B |        1.00 |
| RepeatWithCounter     | NativeAOT 8.0 | 👨‍(...)🏿👍 [127] | 16383 |   493,923.97 ns |  5,936.271 ns |  5,262.349 ns |  0.36 | Faster          |    0.01 | 334.9609 | 333.9844 | 333.9844 | 4164121 B |        1.00 |
| RepeatNoCounter       | NativeAOT 8.0 | 👨‍(...)🏿👍 [127] | 16383 |   500,248.58 ns |  7,666.315 ns |  6,401.720 ns |  0.36 | Faster          |    0.01 | 333.0078 | 333.0078 | 333.0078 | 4161400 B |        1.00 |
| RepeatPowerShell      | NativeAOT 8.0 | 👨‍(...)🏿👍 [127] | 16383 |   504,931.73 ns | 10,097.732 ns | 10,369.627 ns |  0.36 | Faster          |    0.01 | 334.9609 | 334.9609 | 334.9609 | 4161409 B |        1.00 |
| RepeatDoubleBlockSize | NativeAOT 8.0 | 👨‍(...)🏿👍 [127] | 16383 |   327,906.69 ns |  3,848.176 ns |  3,599.587 ns |  0.24 | Faster          |    0.01 | 333.0078 | 333.0078 | 333.0078 | 4161400 B |        1.00 |
|                       |               |                    |       |                 |               |               |       |                 |         |          |          |          |           |             |
| **RepeatWithCounter**     | **.NET 8.0**      | **👨‍(...)‍🧑 [128]**  | **3**     |        **54.22 ns** |      **0.490 ns** |      **0.458 ns** |  **1.18** | **Slower**          |    **0.01** |   **0.1402** |        **-** |        **-** |     **880 B** |        **1.11** |
| RepeatNoCounter       | .NET 8.0      | 👨‍(...)‍🧑 [128]  | 3     |        46.07 ns |      0.325 ns |      0.288 ns |  1.00 | Base            |    0.00 |   0.1262 |        - |        - |     792 B |        1.00 |
| RepeatPowerShell      | .NET 8.0      | 👨‍(...)‍🧑 [128]  | 3     |        49.86 ns |      0.305 ns |      0.255 ns |  1.08 | Slower          |    0.01 |   0.1262 |        - |        - |     792 B |        1.00 |
| RepeatDoubleBlockSize | .NET 8.0      | 👨‍(...)‍🧑 [128]  | 3     |        49.97 ns |      0.508 ns |      0.424 ns |  1.08 | Slower          |    0.01 |   0.1262 |        - |        - |     792 B |        1.00 |
| RepeatWithCounter     | NativeAOT 8.0 | 👨‍(...)‍🧑 [128]  | 3     |        59.56 ns |      0.199 ns |      0.166 ns |  1.29 | Slower          |    0.01 |   0.1377 |        - |        - |     864 B |        1.09 |
| RepeatNoCounter       | NativeAOT 8.0 | 👨‍(...)‍🧑 [128]  | 3     |        48.45 ns |      0.301 ns |      0.282 ns |  1.05 | Same            |    0.01 |   0.1262 |        - |        - |     792 B |        1.00 |
| RepeatPowerShell      | NativeAOT 8.0 | 👨‍(...)‍🧑 [128]  | 3     |        49.72 ns |      0.522 ns |      0.463 ns |  1.08 | Slower          |    0.01 |   0.1262 |        - |        - |     792 B |        1.00 |
| RepeatDoubleBlockSize | NativeAOT 8.0 | 👨‍(...)‍🧑 [128]  | 3     |        52.12 ns |      0.186 ns |      0.165 ns |  1.13 | Slower          |    0.01 |   0.1262 |        - |        - |     792 B |        1.00 |
|                       |               |                    |       |                 |               |               |       |                 |         |          |          |          |           |             |
| **RepeatWithCounter**     | **.NET 8.0**      | **👨‍(...)‍🧑 [128]**  | **7**     |       **110.54 ns** |      **1.242 ns** |      **1.101 ns** |  **1.09** | **Slower**          |    **0.01** |   **0.3034** |        **-** |        **-** |    **1904 B** |        **1.05** |
| RepeatNoCounter       | .NET 8.0      | 👨‍(...)‍🧑 [128]  | 7     |       101.19 ns |      0.640 ns |      0.500 ns |  1.00 | Base            |    0.00 |   0.2894 |        - |        - |    1816 B |        1.00 |
| RepeatPowerShell      | .NET 8.0      | 👨‍(...)‍🧑 [128]  | 7     |       104.45 ns |      0.937 ns |      0.876 ns |  1.03 | Same            |    0.01 |   0.2894 |        - |        - |    1816 B |        1.00 |
| RepeatDoubleBlockSize | .NET 8.0      | 👨‍(...)‍🧑 [128]  | 7     |        98.92 ns |      0.651 ns |      0.577 ns |  0.98 | Same            |    0.01 |   0.2894 |        - |        - |    1816 B |        1.00 |
| RepeatWithCounter     | NativeAOT 8.0 | 👨‍(...)‍🧑 [128]  | 7     |       115.59 ns |      0.437 ns |      0.409 ns |  1.14 | Slower          |    0.00 |   0.3009 |        - |        - |    1888 B |        1.04 |
| RepeatNoCounter       | NativeAOT 8.0 | 👨‍(...)‍🧑 [128]  | 7     |       103.32 ns |      1.960 ns |      1.738 ns |  1.02 | Same            |    0.02 |   0.2894 |        - |        - |    1816 B |        1.00 |
| RepeatPowerShell      | NativeAOT 8.0 | 👨‍(...)‍🧑 [128]  | 7     |       103.30 ns |      0.362 ns |      0.339 ns |  1.02 | Same            |    0.01 |   0.2894 |        - |        - |    1816 B |        1.00 |
| RepeatDoubleBlockSize | NativeAOT 8.0 | 👨‍(...)‍🧑 [128]  | 7     |        98.98 ns |      0.515 ns |      0.430 ns |  0.98 | Same            |    0.01 |   0.2894 |        - |        - |    1816 B |        1.00 |
|                       |               |                    |       |                 |               |               |       |                 |         |          |          |          |           |             |
| **RepeatWithCounter**     | **.NET 8.0**      | **👨‍(...)‍🧑 [128]**  | **15**    |       **230.04 ns** |      **1.644 ns** |      **1.373 ns** |  **1.07** | **Slower**          |    **0.01** |   **0.6297** |        **-** |        **-** |    **3952 B** |        **1.02** |
| RepeatNoCounter       | .NET 8.0      | 👨‍(...)‍🧑 [128]  | 15    |       215.77 ns |      1.011 ns |      0.896 ns |  1.00 | Base            |    0.00 |   0.6156 |        - |        - |    3864 B |        1.00 |
| RepeatPowerShell      | .NET 8.0      | 👨‍(...)‍🧑 [128]  | 15    |       231.05 ns |      3.001 ns |      2.506 ns |  1.07 | Slower          |    0.01 |   0.6156 |        - |        - |    3864 B |        1.00 |
| RepeatDoubleBlockSize | .NET 8.0      | 👨‍(...)‍🧑 [128]  | 15    |       186.81 ns |      1.322 ns |      1.172 ns |  0.87 | Faster          |    0.00 |   0.6156 |        - |        - |    3864 B |        1.00 |
| RepeatWithCounter     | NativeAOT 8.0 | 👨‍(...)‍🧑 [128]  | 15    |       236.23 ns |      1.163 ns |      1.031 ns |  1.09 | Slower          |    0.01 |   0.6270 |        - |        - |    3936 B |        1.02 |
| RepeatNoCounter       | NativeAOT 8.0 | 👨‍(...)‍🧑 [128]  | 15    |       221.40 ns |      3.628 ns |      3.394 ns |  1.03 | Same            |    0.02 |   0.6156 |        - |        - |    3864 B |        1.00 |
| RepeatPowerShell      | NativeAOT 8.0 | 👨‍(...)‍🧑 [128]  | 15    |       221.89 ns |      0.661 ns |      0.586 ns |  1.03 | Same            |    0.00 |   0.6156 |        - |        - |    3864 B |        1.00 |
| RepeatDoubleBlockSize | NativeAOT 8.0 | 👨‍(...)‍🧑 [128]  | 15    |       185.92 ns |      0.997 ns |      0.884 ns |  0.86 | Faster          |    0.01 |   0.6156 |        - |        - |    3864 B |        1.00 |
|                       |               |                    |       |                 |               |               |       |                 |         |          |          |          |           |             |
| **RepeatWithCounter**     | **.NET 8.0**      | **👨‍(...)‍🧑 [128]**  | **1023**  |    **89,431.76 ns** |  **1,699.502 ns** |  **1,506.565 ns** |  **1.00** | **Same**            |    **0.03** |  **76.9043** |  **76.9043** |  **76.9043** |  **262026 B** |        **1.00** |
| RepeatNoCounter       | .NET 8.0      | 👨‍(...)‍🧑 [128]  | 1023  |    89,386.67 ns |  1,662.064 ns |  1,706.817 ns |  1.00 | Base            |    0.00 |  76.9043 |  76.9043 |  76.9043 |  261938 B |        1.00 |
| RepeatPowerShell      | .NET 8.0      | 👨‍(...)‍🧑 [128]  | 1023  |    89,910.52 ns |  1,768.553 ns |  2,299.619 ns |  1.00 | Same            |    0.02 |  76.9043 |  76.9043 |  76.9043 |  261938 B |        1.00 |
| RepeatDoubleBlockSize | .NET 8.0      | 👨‍(...)‍🧑 [128]  | 1023  |    54,444.22 ns |  1,073.332 ns |  1,054.155 ns |  0.61 | Faster          |    0.02 |  76.9043 |  76.9043 |  76.9043 |  261938 B |        1.00 |
| RepeatWithCounter     | NativeAOT 8.0 | 👨‍(...)‍🧑 [128]  | 1023  |   115,894.15 ns |  1,405.363 ns |  1,314.578 ns |  1.30 | Slower          |    0.03 |  76.9043 |  76.9043 |  76.9043 |  262010 B |        1.00 |
| RepeatNoCounter       | NativeAOT 8.0 | 👨‍(...)‍🧑 [128]  | 1023  |   117,082.65 ns |  1,761.895 ns |  1,648.078 ns |  1.31 | Slower          |    0.03 |  76.9043 |  76.9043 |  76.9043 |  261938 B |        1.00 |
| RepeatPowerShell      | NativeAOT 8.0 | 👨‍(...)‍🧑 [128]  | 1023  |   116,128.08 ns |  2,184.744 ns |  2,145.711 ns |  1.30 | Slower          |    0.04 |  76.9043 |  76.9043 |  76.9043 |  261938 B |        1.00 |
| RepeatDoubleBlockSize | NativeAOT 8.0 | 👨‍(...)‍🧑 [128]  | 1023  |    81,128.60 ns |    730.758 ns |    683.551 ns |  0.91 | Faster          |    0.02 |  76.9043 |  76.9043 |  76.9043 |  261938 B |        1.00 |
|                       |               |                    |       |                 |               |               |       |                 |         |          |          |          |           |             |
| **RepeatWithCounter**     | **.NET 8.0**      | **👨‍(...)‍🧑 [128]**  | **16383** | **1,399,817.24 ns** | **26,867.646 ns** | **31,984.021 ns** |  **0.99** | **Same**            |    **0.04** | **998.0469** | **998.0469** | **998.0469** | **4194496 B** |        **1.00** |
| RepeatNoCounter       | .NET 8.0      | 👨‍(...)‍🧑 [128]  | 16383 | 1,414,177.04 ns | 27,807.035 ns | 50,846.736 ns |  1.00 | Base            |    0.00 | 998.0469 | 998.0469 | 998.0469 | 4194408 B |        1.00 |
| RepeatPowerShell      | .NET 8.0      | 👨‍(...)‍🧑 [128]  | 16383 | 1,394,582.13 ns | 27,244.001 ns | 32,432.045 ns |  0.98 | Same            |    0.04 | 998.0469 | 998.0469 | 998.0469 | 4194408 B |        1.00 |
| RepeatDoubleBlockSize | .NET 8.0      | 👨‍(...)‍🧑 [128]  | 16383 |   785,381.64 ns | 15,239.599 ns | 18,141.659 ns |  0.55 | Faster          |    0.03 | 999.0234 | 999.0234 | 999.0234 | 4194408 B |        1.00 |
| RepeatWithCounter     | NativeAOT 8.0 | 👨‍(...)‍🧑 [128]  | 16383 |   496,399.64 ns |  5,213.360 ns |  4,621.508 ns |  0.35 | Faster          |    0.01 | 335.9375 | 334.9609 | 334.9609 | 4196905 B |        1.00 |
| RepeatNoCounter       | NativeAOT 8.0 | 👨‍(...)‍🧑 [128]  | 16383 |   501,330.75 ns |  9,597.614 ns | 12,479.610 ns |  0.35 | Faster          |    0.02 | 334.9609 | 334.9609 | 334.9609 | 4194177 B |        1.00 |
| RepeatPowerShell      | NativeAOT 8.0 | 👨‍(...)‍🧑 [128]  | 16383 |   511,113.93 ns |  9,936.489 ns |  9,758.961 ns |  0.36 | Faster          |    0.01 | 333.9844 | 333.9844 | 333.9844 | 4194169 B |        1.00 |
| RepeatDoubleBlockSize | NativeAOT 8.0 | 👨‍(...)‍🧑 [128]  | 16383 |   326,533.34 ns |  3,364.491 ns |  3,147.147 ns |  0.23 | Faster          |    0.01 | 333.4961 | 333.4961 | 333.4961 | 4194168 B |        1.00 |

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
