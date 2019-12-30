using System.Collections;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Numerics;

namespace AdventOfCode2019.VM
{
    public class VirtualMemory
    {
        public VirtualMemory(IEnumerable<BigInteger> load)
        {
            _store = load.Select((v, i) => (i, v)).ToImmutableDictionary(t => (BigInteger)t.i, t => t.v);
        }

        ImmutableDictionary<BigInteger, BigInteger> _store;

        public BigInteger this[BigInteger address]
        {
            get => _store.TryGetValue(address, out var res) ? res : 0;
            set => _store = _store.SetItem(address, value);
        }

        private VirtualMemory()
        {}

        public VirtualMemory Clone ()
        {
            var clone = new VirtualMemory();
            clone._store = _store;
            return clone;
        }

        public BigInteger[] Dump => Enumerable.Range(0, (int) _store.Keys.Max()+1).Select(a => _store[a]).ToArray();
    }
}