using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data.Linq;

namespace DHT.Nodes
{
    public class NodeStore : INodeStore
    {
        private readonly IDictionary<string, string> store;

        public NodeStore()
        {
            this.store = new ConcurrentDictionary<string, string>();
        }

        public bool AddValue(string key, string value)
        {
            if (this.store.ContainsKey(key))
            {
                throw new DuplicateKeyException(key);
            }

            this.store.Add(key, value);

            // We should also return false if the store is full
            return true;
        }

        public bool ContainsKey(string key)
        {
            return this.store.ContainsKey(key);
        }

        public string GetValue(string key)
        {
            string value = null;

            this.store.TryGetValue(key, out value);

            return value;
        }

        public bool RemoveValue(string key)
        {
            return this.store.Remove(key);
        }
    }
}
