using System.Collections.Generic;

namespace d4160.Core.Extensions
{
    public static class DebugExtensions
    {
        public static void DebugLog<T>(this IList<T> source)
        {
            string t = string.Empty;

            for (int i = 0; i < source.Count; i++)
            {
                t += $"{source[i]}, ";
            }

            t = t.Substring(0, t.Length - 2);

            UnityEngine.Debug.Log(t);
        }
    }
}
