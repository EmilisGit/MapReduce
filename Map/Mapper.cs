using System.Text.RegularExpressions;

namespace Map;
public class Mapper(string[] keys, string[] userSelectedProperties)
{
    private readonly string[] _keys = [.. keys];
    private readonly string[] _userSelectedProperties = [.. userSelectedProperties];
    private readonly string[] requiredProperties = [.. keys, .. userSelectedProperties];
    private static readonly Regex _entryRegex = new(@"\{\{.*?\}\}", RegexOptions.Compiled | RegexOptions.IgnoreCase);
    private static readonly Regex _keyValuePairRegex =
        new(@"(?<key>\w+(?:\s\w+)*)" + @"[=<>]+" + @"(?<value>[^}]*)",
        RegexOptions.Compiled | RegexOptions.IgnoreCase);

    public IEnumerable<string> Map()
    {
        string? line;
        while ((line = Console.ReadLine()) is not null)
        {
            // Match every block of entries that is between {{ }}
            var entries = _entryRegex.Matches(line);
            foreach (Match entry in entries)
            {
                // Match every entry that has this structure {key=value}
                MatchCollection keyValuePairs = _keyValuePairRegex.Matches(entry.Value);
                if (keyValuePairs.Count == 0) continue;

                Dictionary<string, string> keyValueDict = keyValuePairs
                    .Where(key => requiredProperties.Contains(key.Groups["key"].Value))
                    .ToDictionary(
                        kv => kv.Groups["key"].Value,
                        kv => kv.Groups["value"].Value ?? "1");

                // If there are missing properties, skip that entry
                if (keyValueDict.Keys.Count != requiredProperties.Length)
                    continue;

                var keyDict = ToStringFloatDict(keyValueDict, _keys);
                var valueDict = ToStringFloatDict(keyValueDict, _userSelectedProperties);
                string baseName = string
                    .Join("", keyDict.Select(e => $"{e.Key}={e.Value}_"));

                foreach (var value in valueDict)
                {
                    yield return $"{baseName}{value.Key}\t{value.Value}";
                }
            }
        }
    }

    private static SortedDictionary<string, float> ToStringFloatDict(Dictionary<string, string> keyValueDict, string[] toTake)
    {
        var temp = keyValueDict.Where(e => toTake.Contains(e.Key)).ToDictionary(e => e.Key, e => e.Value);
        SortedDictionary<string, float> stringFloatDict = [];

        // If the value is not a number, append it to the key and change the value to 1
        foreach (var e in temp)
        {
            if (string.IsNullOrWhiteSpace(e.Value))
                continue;
            if (float.TryParse(e.Value, out float number))
            {
                stringFloatDict[e.Key] = number;
            }
            else
            {
                stringFloatDict[$"{e.Key}{e.Value}"] = 1;
            }
        }
        return stringFloatDict;
    }
}