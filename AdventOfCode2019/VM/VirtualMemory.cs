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
        private BigInteger[] _store;
        ImmutableDictionary<BigInteger, BigInteger> _storeBig = ImmutableDictionary<BigInteger, BigInteger>.Empty;

        public VirtualMemory(IEnumerable<BigInteger> load)
        {
            var data = load.ToList();
            _store = new BigInteger[data.Count * 2];
            data.CopyTo(_store);
        }

        public BigInteger this[BigInteger address]
        {
            get => address < _store.Length ? 
                _store[(long)address] : _storeBig.TryGetValue(address, out var result) ? result: 0;
            set
            {
                if (address < _store.Length)
                {
                    _store[(int)address] = value;
                }
                _storeBig = _storeBig.SetItem(address, value);
            }
        }

        private VirtualMemory()
        {}

        public VirtualMemory Clone () =>
            new VirtualMemory
            {
                _store = (BigInteger[]) _store.Clone(), 
                _storeBig = _storeBig
            };

        public BigInteger[] Dump => (BigInteger[])_store.Clone();
    }
}