using BenchmarkDotNet.Attributes;

[MemoryDiagnoser]
public class VsLinqTest
{
    // 2 characters / 128 characters
    [Params(
        [
            "👍",
            "👨‍👩‍👧‍👦👨🏻‍👩🏻‍👧🏻‍👦🏻👨‍👩‍👧‍👦👨🏼‍👩🏼‍👧🏼‍👦🏼👨‍👩‍👧‍👦👨🏾‍👩🏾‍👧🏾‍👦🏾👨‍👩‍👧‍👦👨🏿‍👩🏿‍👧🏿‍👦🏿🧑‍🤝‍🧑",
        ]
    )]
    public string Input { get; set; } = null!;

    [Params([3, 16383])]
    public int Count { get; set; }

    [Benchmark(Baseline = true)]
    public string StringCreateFastest()
    {
        return StringRepeat(Input, Count);
        static string StringRepeat(string str, int count)
        {
            ArgumentOutOfRangeException.ThrowIfNegativeOrZero(count, nameof(count));
            return (count, str.Length) switch
            {
                (0, _) or (_, 0) => "",
                (1, _) => str,
                (_, 1) => new string(str[0], count),
                _
                    => string.Create(
                        checked(str.Length * count),
                        (str, count),
                        static (outSpan, args) =>
                        {
                            var inSpan = args.str.AsSpan();
                            inSpan.CopyTo(outSpan);
                            var len = inSpan.Length;
                            var firstSpan = outSpan[..len];
                            var copyFromSpan = firstSpan;
                            var copyToSpan = outSpan[len..];
                            for (
                                var bit =
                                    1
                                    << (
                                        30
                                        - System.Numerics.BitOperations.LeadingZeroCount(
                                            (uint)args.count
                                        )
                                    );
                                bit != 0;
                                bit >>= 1
                            )
                            {
                                copyFromSpan.CopyTo(copyToSpan);
                                var oldLength = copyFromSpan.Length;
                                copyToSpan = copyToSpan[oldLength..];
                                copyFromSpan = outSpan[..(oldLength << 1)];
                                if ((bit & args.count) != 0)
                                {
                                    firstSpan.CopyTo(copyToSpan);
                                    copyToSpan = copyToSpan[len..];
                                }
                            }
                        }
                    ),
            };
        }
    }

    [Benchmark]
    public string Linq()
    {
        return StringRepeat(Input, Count);
        static string StringRepeat(string str, int count)
        {
            ArgumentOutOfRangeException.ThrowIfNegativeOrZero(count, nameof(count));
            return (count, str.Length) switch
            {
                (0, _) or (_, 0) => "",
                (1, _) => str,
                (_, 1) => new string(str[0], count),
                _ => string.Concat(Enumerable.Repeat(str, count)),
            };
        }
    }
}
