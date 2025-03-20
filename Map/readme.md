MapperBenchmark.BenchmarkSequential: DefaultJob
Runtime = .NET 9.0.2 (9.0.225.6610), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI; GC = Concurrent Workstation
Mean = 50.632 ns, StdErr = 0.688 ns (1.36%), N = 95, StdDev = 6.702 ns
Min = 41.768 ns, Q1 = 44.996 ns, Median = 49.793 ns, Q3 = 55.284 ns, Max = 71.647 ns
IQR = 10.287 ns, LowerFence = 29.565 ns, UpperFence = 70.715 ns
ConfidenceInterval = [48.296 ns; 52.968 ns] (CI 99.9%), Margin = 2.336 ns (4.61% of Mean)
Skewness = 0.76, Kurtosis = 3.02, MValue = 3.38
-------------------- Histogram --------------------
[39.840 ns ; 42.205 ns) | @@
[42.205 ns ; 46.061 ns) | @@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@
[46.061 ns ; 49.945 ns) | @@@@@@@@@@@@@@
[49.945 ns ; 51.651 ns) | @@@@@@
[51.651 ns ; 56.139 ns) | @@@@@@@@@@@@@@@@@@@@
[56.139 ns ; 59.995 ns) | @@@@@@@@@@@@@@
[59.995 ns ; 61.822 ns) | 
[61.822 ns ; 65.678 ns) | @@@@@
[65.678 ns ; 67.895 ns) | 
[67.895 ns ; 71.751 ns) | @@
---------------------------------------------------

MapperBenchmark.BenchmarkParallel: DefaultJob
Runtime = .NET 9.0.2 (9.0.225.6610), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI; GC = Concurrent Workstation
Mean = 47.181 ns, StdErr = 0.598 ns (1.27%), N = 97, StdDev = 5.891 ns
Min = 37.889 ns, Q1 = 42.440 ns, Median = 46.203 ns, Q3 = 50.577 ns, Max = 62.033 ns
IQR = 8.137 ns, LowerFence = 30.234 ns, UpperFence = 62.783 ns
ConfidenceInterval = [45.150 ns; 49.211 ns] (CI 99.9%), Margin = 2.031 ns (4.30% of Mean)
Skewness = 0.77, Kurtosis = 2.72, MValue = 2.47
-------------------- Histogram --------------------
[37.574 ns ; 40.854 ns) | @@@@@@@
[40.854 ns ; 45.175 ns) | @@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@
[45.175 ns ; 48.541 ns) | @@@@@@@@@@@@@@@@@@@@@@@@@@@
[48.541 ns ; 52.428 ns) | @@@@@@@@
[52.428 ns ; 55.794 ns) | @@@@@@@@@@@@
[55.794 ns ; 58.328 ns) | @@
[58.328 ns ; 61.694 ns) | @@@@@@
[61.694 ns ; 63.716 ns) | @
---------------------------------------------------

// * Summary *

BenchmarkDotNet v0.14.0, Ubuntu 24.04.2 LTS (Noble Numbat) WSL
11th Gen Intel Core i7-1165G7 2.80GHz, 1 CPU, 8 logical and 4 physical cores
.NET SDK 9.0.200
  [Host]     : .NET 9.0.2 (9.0.225.6610), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI
  DefaultJob : .NET 9.0.2 (9.0.225.6610), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI


| Method              | Mean     | Error    | StdDev   | Gen0   | Allocated |
|-------------------- |---------:|---------:|---------:|-------:|----------:|
| BenchmarkSequential | 50.63 ns | 2.336 ns | 6.702 ns | 0.0089 |      56 B |
| BenchmarkParallel   | 47.18 ns | 2.031 ns | 5.891 ns | 0.0089 |      56 B |

// * Warnings *
MultimodalDistribution
  MapperBenchmark.BenchmarkSequential: Default -> It seems that the distribution is bimodal (mValue = 3.38)

// * Hints *
Outliers
  MapperBenchmark.BenchmarkSequential: Default -> 5 outliers were removed (82.27 ns..104.80 ns)
  MapperBenchmark.BenchmarkParallel: Default   -> 3 outliers were removed (68.27 ns..78.05 ns)

// * Legends *
  Mean      : Arithmetic mean of all measurements
  Error     : Half of 99.9% confidence interval
  StdDev    : Standard deviation of all measurements
  Gen0      : GC Generation 0 collects per 1000 operations
  Allocated : Allocated memory per single operation (managed only, inclusive, 1KB = 1024B)
  1 ns      : 1 Nanosecond (0.000000001 sec)



  using System.Text.RegularExpressions;

namespace Map;

public class Mapper(string key, string[] properties, Regex entryRegex, Regex keyValuePairRegex)
{
    private readonly string _key = key;
    private readonly string[] _userSelectedProperties = properties;

    // Regex kuris atskiria kiekvieną duomenų įrašą pvz. marsutas=112
    private readonly Regex _entryRegex = entryRegex;

    // Regex kuris atskiria raktą ir vertę, naudojant 2 grupes: key ir value, pvz. key = marsrutas, value = 112
    private readonly Regex _keyValuePairRegex = keyValuePairRegex;

    public IEnumerable<string> MapSequentially()
    {
        string? line;

        while ((line = Console.ReadLine()) is not null)
        {
            var entries = _entryRegex.Matches(line);
            foreach (Match entry in entries)
            {
                MatchCollection keyValuePairs = _keyValuePairRegex.Matches(entry.Value);
                if (keyValuePairs.Count == 0)
                    continue;

                var keyValueDict = keyValuePairs
                    .ToDictionary(
                        key => key.Groups["key"].Value,
                        value => value.Groups["value"].Value);

                var key = keyValueDict.GetValueOrDefault(_key);

                if (key is null)
                    continue;

                foreach (var property in _userSelectedProperties)
                {
                    if (keyValueDict.TryGetValue(property, out string? value))
                    {
                        if (float.TryParse(value, out _))
                        {
                            // If valid, return the key-property pair as usual
                            yield return $"{key}_{property}\t{value}";
                        }
                        else
                        {
                            // If the property is missing or cannot be parsed to float, modify the key and set property to "1"
                            yield return $"{key}_{property}_{value}\t1";
                        }
                    }
                }
            }
        }
    }
}