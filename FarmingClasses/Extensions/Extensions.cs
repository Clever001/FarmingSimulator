using FarmingClasses.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FarmingClasses.Extensions;
public static class Extensions {
    public static List<KVP<T1, T2>> ToCustomKVP<T1, T2>(this List<KeyValuePair<T1, T2>> collection) {
        List<KVP<T1, T2>> result = new();
        foreach (var kvp in collection) { result.Add(new KVP<T1, T2>(kvp.Key, kvp.Value)); }
        return result;
    }

    public static List<KeyValuePair<T1, T2>> FromCustomKVP<T1, T2>(this List<KVP<T1, T2>> collection) {
        List<KeyValuePair<T1, T2>> result = new();
        foreach (var kvp in collection) {
            result.Add(new KeyValuePair<T1, T2>(kvp.Key ?? throw new ArgumentNullException(nameof(kvp.Key)), kvp.Value ?? throw new ArgumentNullException(nameof(kvp.Value))));
        }
        return result;
    }
}
