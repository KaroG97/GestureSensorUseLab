using System;
using System.Collections;
using System.Collections.Generic;


namespace FTS.Common
{
    class Memory<T> where T : struct, IComparable
    {
        private Dictionary<string, T[]> _buffers = new Dictionary<string, T[]>();

        public Memory(Dictionary<string, int> defines)
        {
            foreach (var container in defines)
                AddBuffer(container.Key, container.Value);
        }

        public void AddBuffer(string name, int length)
        {
            if (!_buffers.ContainsKey(name))
                _buffers.Add(name, new T[length]);
        }

        public void RemoveBuffer(string name)
        {
            _buffers.Remove(name);
        }

        public T[] Get(string name)
        {
            if (_buffers.ContainsKey(name))
                return _buffers[name];
            return null;
        }
    }
}

