using System.Security.Cryptography;

using BenchmarkDotNet.Attributes;

[MemoryDiagnoser]
public class RepeatDoubleBlockSizeBench
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

    [Benchmark]
    public string RepeatWithCounter()
    {
        return StringRepeatWithCounter(Input, Count);
        static string StringRepeatWithCounter(string str, int count)
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

    [Benchmark(Baseline = true)]
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
    public string RepeatPowerShell()
    {
        return StringRepeatPowerShell(Input, Count);
        static string StringRepeatPowerShell(string str, int count)
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
                        static (dst, args) =>
                        {
                            // https://github.com/PowerShell/PowerShell/blob/ec3840d6a1fffdbaca7173ededb4a4504b2f5b41/src/System.Management.Automation/engine/runtime/Operations/StringOps.cs#L53-L62
                            // Copyright (c) Microsoft Corporation.
                            // Licensed under the MIT License.
                            ReadOnlySpan<char> src = args.str.AsSpan();
                            int length = src.Length;
                            for (int i = 0; i < args.count; i++)
                            {
                                src.CopyTo(dst);
                                dst = dst.Slice(length);
                            }
                        }
                        )
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
