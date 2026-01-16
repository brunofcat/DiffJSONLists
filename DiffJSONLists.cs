using Newtonsoft.Json.Linq;
using System.Text.RegularExpressions;


namespace Ardo.DiffJSONLists
{
    public class DiffJSONLists : IDiffJSONLists
    {

        public string JSON_DiffLists(string leftJsonIn, string rightJsonIn, string keyFieldIn, bool ReturnUnchangedIn, string IgnoreChangedFieldsIn, string ReplaceFieldMatrixIn, bool HumanizeFieldsIn)
        {
            var diffs = new List<object>();

            var leftArray = JArray.Parse(leftJsonIn);
            var rightArray = JArray.Parse(rightJsonIn);

            var leftMap = leftArray
                .Where(x => x[keyFieldIn] != null)
                .ToDictionary(x => x[keyFieldIn]!.ToString(), x => x);

            var rightMap = rightArray
                .Where(x => x[keyFieldIn] != null)
                .ToDictionary(x => x[keyFieldIn]!.ToString(), x => x);


            var ignoreFields = new HashSet<string>(
                (IgnoreChangedFieldsIn ?? string.Empty)
                    .Split(',', StringSplitOptions.RemoveEmptyEntries)
                    .Select(f => f.Trim()),
                StringComparer.OrdinalIgnoreCase
            );


            // Pre-parse ReplaceFieldMatrixIn only once
            Dictionary<string, string> replaceMap;
            if (string.IsNullOrWhiteSpace(ReplaceFieldMatrixIn))
            {
                replaceMap = new Dictionary<string, string>();
            }
            else
            {
                try
                {
                    replaceMap = JArray.Parse(ReplaceFieldMatrixIn)
                        .Where(x => x["From"] != null && x["To"] != null)
                        .ToDictionary(
                            x => x["From"]!.ToString(),
                            x => x["To"]!.ToString()
                        );
                }
                catch
                {
                    // If parsing fails, fallback to empty dictionary
                    replaceMap = new Dictionary<string, string>();
                }
            }


        // Now ResolveFieldName just looks up the dictionary
        string ResolveFieldName(string fieldName)
            {
                if (replaceMap.TryGetValue(fieldName, out var newName))
                    return newName;

                if (HumanizeFieldsIn)
                    return HumanizeRegex.Replace(fieldName, " ");

                return fieldName;
            }


            // Removed + Changed + Unchanged
            foreach (var kv in leftMap)
            {
                if (!rightMap.ContainsKey(kv.Key))
                {
                    diffs.Add(new
                    {
                        Type = "Removed",
                        Key = kv.Key,
                        OldValue = kv.Value
                    });
                    continue;
                }

                var leftObj = kv.Value as JObject;
                var rightObj = rightMap[kv.Key] as JObject;

                if (leftObj == null || rightObj == null)
                    continue;

                var changes = new List<object>();

                foreach (var prop in leftObj.Properties())
                {
                    // Ignore key field
                    if (prop.Name == keyFieldIn)
                        continue;

                    // Ignore configured fields
                    if (ignoreFields.Contains(prop.Name))
                        continue;

                    var rProp = rightObj[prop.Name];

                    // If field exists on both sides and has changed
                    if (rProp != null && !JToken.DeepEquals(prop.Value, rProp))
                    {
                        changes.Add(new
                        {
                            Field = ResolveFieldName(prop.Name),
                            OldValue = prop.Value,
                            NewValue = rProp
                        });
                    }
                }

                if (changes.Any())
                {
                    diffs.Add(new
                    {
                        Type = "Changed",
                        Key = kv.Key,
                        Changes = changes
                    });
                }
                else
                {
                    if (ReturnUnchangedIn)
                    {
                        diffs.Add(new
                        {
                            Type = "Unchanged",
                            Key = kv.Key
                        });
                    }
                }
            }

            // Added
            foreach (var kv in rightMap)
            {
                if (!leftMap.ContainsKey(kv.Key))
                {
                    diffs.Add(new
                    {
                        Type = "Added",
                        Key = kv.Key,
                        NewValue = kv.Value
                    });
                }
            }

            return JArray.FromObject(diffs).ToString();
        }

        private static readonly Regex HumanizeRegex = new Regex(
            @"(?<=[a-z])(?=[A-Z])|(?<=[A-Z])(?=[A-Z][a-z])",
            RegexOptions.Compiled
        );

    }

}
