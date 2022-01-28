using System;
using System.Collections.Generic;
using System.Linq;

namespace Tealium
{
    public class AnyCollectionKey
    {
        protected AnyCollectionKey()
        {
        }
    }

    /// <summary>
    /// Use as a collection that automatically generates it's keys. Key are specific to this class and cannot be reused with other instances.
    /// 
    /// This class is not intended to be used in a multi thread environment. Add proper lock mechanism to avoid thread races.
    /// </summary>
    /// <typeparam name="T"> The type included in the collection </typeparam>
    public class KeyedCollection<T> where T: class
    {
        readonly string id = Guid.NewGuid().ToString();
        int count = 0;
        readonly Dictionary<string, T> dict = new Dictionary<string, T>();
        public KeyedCollection()
        {
        }

        public CollectionSpecificKey<T> Add(T obj)
        {
            count += 1;
            string key = id + count.ToString();
            dict[key] = obj;
            return new CollectionSpecificKey<T>(key);
        }

        public bool Contains(CollectionSpecificKey<T> key)
        {
            return dict.ContainsKey(key.key);
        }

        public T Get(CollectionSpecificKey<T> key)
        {
            return dict[key.key];
        }

        public bool Remove(CollectionSpecificKey<T> key)
        {
            return dict.Remove(key.key);
        }

        public void Clear()
        {
            dict.Clear();
        }

        public ICollection<CollectionSpecificKey<T>> Keys => dict.Keys.Select(key => new CollectionSpecificKey<T>(key)).ToList();
        
    }

    public class CollectionSpecificKey<T> : AnyCollectionKey where T : class
    {
        readonly internal string key;

        internal CollectionSpecificKey(string key)
        {
            this.key = key;
        }
    }

}
