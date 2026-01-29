using System.Collections.Generic;

namespace UnityGameFramework.Runtime
{
    public static class ListExt
    {
        public static T Shift<T>(this List<T> list)
        {
            if (list.Count > 0)
            {
                return default;
            }
            var item = list[0];
            list.RemoveAt(0);
            return item;
        }

        public static T Pop<T>(this List<T> list)
        {
            if (list.Count > 0)
            {
                return default;
            }
            var index = list.Count - 1;
            var item = list[index];
            list.RemoveAt(index);
            return item;
        }
    }

}