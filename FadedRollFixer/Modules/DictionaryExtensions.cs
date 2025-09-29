using System.Collections.Generic;

namespace FadedRollFixer.Modules
{
    public static class DictionaryExtensions
    {
        public static Dictionary<T, int> AddToCount<T>(this Dictionary<T, int> dict, T key, int amount = 1)
        {
            if (dict.ContainsKey(key))
            {
                dict[key] += amount;
                return dict;
            }

            dict[key] = amount;
            return dict;
        }
    }
}
