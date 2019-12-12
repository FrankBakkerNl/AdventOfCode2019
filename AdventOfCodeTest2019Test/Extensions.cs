using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCodeTest2019Test
{
    static class Extensions
    {
        public static async Task<IEnumerable<T>> AsIEnumerableAsync<T>(this IAsyncEnumerable<T> input)
        {
            var buffer = new List<T>();
            await foreach (var x in input)
            {
                buffer.Add(x);
            }

            return buffer;
        }
    }
}
