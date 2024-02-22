using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace United.Mobile.Model.Shopping
{
    public static class ListExtensions
    {
        public static void Sort<TSource, TValue>(this List<TSource> source, Func<TSource, TValue> selector)
        {
            var comparer = Comparer<TValue>.Default;
            source.Sort((x, y) => comparer.Compare(selector(x), selector(y)));
        }
        public static void SortDescending<TSource, TValue>(this List<TSource> source, Func<TSource, TValue> selector)
        {
            var comparer = Comparer<TValue>.Default;
            source.Sort((x, y) => comparer.Compare(selector(y), selector(x)));
        }
    }
}
