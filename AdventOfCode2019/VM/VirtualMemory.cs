using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

namespace AdventOfCode2019.VM
{

    public struct VirtualMemory 
    {
        private readonly long[] _store;
        ImmutableDictionary<long, long> _storeBig;

        public VirtualMemory(IEnumerable<long> load)
        {
            _storeBig = ImmutableDictionary<long, long>.Empty;
            var data = load.ToList();
            _store = new long[data.Count * 10];
            data.CopyTo(_store);
        }

        public long this[long address]
        {
            get => address < _store.Length ? 
                _store[address] : _storeBig.TryGetValue(address, out var result) ? result: 0;
            set
            {
                if (address < _store.Length)
                {
                    _store[(int)address] = value;
                }
                else
                {
                    _storeBig = _storeBig.SetItem(address, value);
                }
            }
        }

        private VirtualMemory(VirtualMemory source)
        {
            _store = (long[])source._store.Clone();
            _storeBig = source._storeBig;
        }

        public VirtualMemory Clone() => new VirtualMemory(this);

        public long[] Dump => (long[])_store.Clone();
    }
}