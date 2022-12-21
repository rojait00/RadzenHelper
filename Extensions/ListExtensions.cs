using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RadzenHelper.Extensions
{
    public static class ListExtensions
    {
        public static void ReplaceValues<T>(this List<T> list, IEnumerable<T> newValues)
        {
            list.Clear();
            list.AddRange(newValues);
        }
        
        public static IEnumerable<T> GetNth<T>(this List<T> list, int n)
        {
            for (int i = 0; i < list.Count; i += n)
                yield return list[i];
        }
    }
}
