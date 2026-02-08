using System.Collections.Generic;

namespace GamePlay.Game
{
    public static class Tools
    {
        public static void Set<T1, T2>(this Dictionary<T1, T2> dic, T1 key, T2 value)
        {
            if (dic.ContainsKey(key))
            {
                dic[key] = value;
            }
            else
            {
                dic.Add(key, value);
            }
        }
    }
}