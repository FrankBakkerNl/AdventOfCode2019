using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode2019.Day18Helpers
{
    public class Heap<T>
    {
        private readonly SortedList<int, Queue<T>> _store = new SortedList<int, Queue<T>>();
        private Queue<T> _hotQueue;

        public T Pop()
        {
            if (_hotQueue == null)
            {
                _hotQueue = _store.First().Value;
            }
            var result = _hotQueue.Dequeue();
            if (_hotQueue.Count == 0)
            {
                _store.RemoveAt(0);
                _hotQueue = null;
            }
            return result;
        }

        public void Add(T value, int weight)
        {
            if (!_store.TryGetValue(weight, out var queue))
            {
                queue = new Queue<T>();
                _store.Add(weight, queue);
            }
            queue.Enqueue(value);
        }

        public bool Any() => _store.Count!=0;

    }
}
