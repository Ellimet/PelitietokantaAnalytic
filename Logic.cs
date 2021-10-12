using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PelitietokantaAnalytic
{
    static class Logic
    {
        public static T Average<T>(List<T> items) where T : IComparable
        {
            dynamic sum = default(T);
            foreach (var item in items)
                sum += item;    
            return (T)Convert.ChangeType(sum / items.Count, typeof(T));
        }

        public static T Median<T>(this IList<T> list) where T : IComparable<T> => list[((list.Count - 1) / 2)];
        
    }
}
