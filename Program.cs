// See https://aka.ms/new-console-template for more information
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;


BenchmarkSwitcher.FromAssembly(typeof(Program).Assembly).Run(args);

[MemoryDiagnoser]
public class BenchMarkClass
{
    // 2 characters / 127 characters / 128 characters
    [Params(
        [
            "👍",
            "👨‍👩‍👧‍👦👨🏻‍👩🏻‍👧🏻‍👦🏻👨🏼‍👩🏼‍👧🏼‍👦🏼👨🏽‍👩🏽‍👧🏽‍👦🏽👨🏽‍👩🏽‍👧🏽‍👦🏽👨🏾‍👩🏾‍👧🏾‍👦🏾👨🏿‍👩🏿‍👧🏿‍👦🏿👍",
            "👨‍👩‍👧‍👦👨🏻‍👩🏻‍👧🏻‍👦🏻👨‍👩‍👧‍👦👨🏼‍👩🏼‍👧🏼‍👦🏼👨‍👩‍👧‍👦👨🏾‍👩🏾‍👧🏾‍👦🏾👨‍👩‍👧‍👦👨🏿‍👩🏿‍👧🏿‍👦🏿🧑‍🤝‍🧑",
        ]
    )]
    public string Input { get; set; } = null!;

    [Params([3, 7, 15, 1023, 16383])]
    public int Count { get; set; }

    [Benchmark(Baseline = true)]
    public string RepeatBase()
    {
        return StringRepeatBase(Input, Count);
        static string StringRepeatBase(string str, int count)
        {
            ArgumentOutOfRangeException.ThrowIfNegativeOrZero(count, nameof(count));
            return (count, str.Length) switch
            {
                (0, _) or (_, 0) => "",
                (1, _) => str,
                (_, 1) => new string(str[0], count),
                _
                    // IMPORTANT: This expression is not licensed by me.
                    => string.Create(
                        str.Length * count,
                        str,
                        (span, value) =>
                        {
                            for (int i = 0; i < count; i++)
                            {
                                value.AsSpan().CopyTo(span);
                                span = span.Slice(value.Length);
                            }
                        }
                    ),
            };
        }
    }

    [Benchmark]
    public string RepeatNoCounter()
    {
        return StringRepeatNoCounter(Input, Count);
        static string StringRepeatNoCounter(string str, int count)
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
                        str,
                        static (outSpan, input) =>
                        {
                            var inSpan = input.AsSpan();
                            for (; outSpan.Length != 0; outSpan = outSpan.Slice(inSpan.Length))
                            {
                                inSpan.CopyTo(outSpan);
                            }
                        }
                    ),
            };
        }
    }

    [Benchmark]
    public string RepeatDoubleBlockSize()
    {
        return StringRepeatDoubleBlockSize(Input, Count);
        static string StringRepeatDoubleBlockSize(string str, int count)
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
}
