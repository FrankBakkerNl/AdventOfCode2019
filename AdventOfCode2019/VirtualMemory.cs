using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;

namespace AdventOfCode2019
{
    public class VirtualMemory : IEnumerable<BigInteger>
    {
        public VirtualMemory(IEnumerable<int> load) : this(load.Select(i=>(BigInteger)i))
        {}

        public VirtualMemory(IEnumerable<BigInteger> load)
        {
            _store = load.Select((v, i) => (i, v)).ToDictionary(t => (BigInteger)t.i, t => t.v);
        }

        readonly Dictionary<BigInteger, BigInteger> _store;

        public BigInteger this[BigInteger address]
        {
            get => _store.TryGetValue(address, out var res) ? res : 0;
            set => _store[address] = value;
        }

        public IEnumerator<BigInteger> GetEnumerator() => _store.OrderBy(kv=>kv.Key).Select(kv=>kv.Value).GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}