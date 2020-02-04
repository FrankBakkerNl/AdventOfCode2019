using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Numerics;

namespace AdventOfCode2019.VM
{

    public class VirtualMemory 
    {
        private long[] _store;
        ImmutableDictionary<long, long> _storeBig = ImmutableDictionary<long, long>.Empty;

        public VirtualMemory(IEnumerable<long> load)
        {
            var data = load.ToList();
            _store = new long[data.Count * 10];
            data.CopyTo(_store);
        }

        public long this[long address]
        {
            get => address < _store.Length ? 
                _store[(long)address] : _storeBig.TryGetValue(address, out var result) ? result: 0;
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

        private VirtualMemory()
        {}

        public VirtualMemory Clone () =>
            new VirtualMemory
            {
                _store = (long[]) _store.Clone(), 
                _storeBig = _storeBig
            };

        public long[] Dump => (long[])_store.Clone();
    }
}