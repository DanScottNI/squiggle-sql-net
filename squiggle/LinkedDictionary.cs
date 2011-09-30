using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;

namespace ScottGarland.Collections
{
    public class LinkedDictionary<TKey, TValue> : IDictionary<TKey, TValue>, IDictionary
    {
        private const int HashMask = 0x7fffffff;
        private readonly IEqualityComparer<TKey> _comparer;
        private readonly bool _isKeyValueType;
        private HashNode<TKey, TValue>[] _buckets;
        private int _count;
        private HashNode<TKey, TValue> _head;
        private int _rehashThreshold;
        private object _syncRoot;
        private HashNode<TKey, TValue> _tail;

        public LinkedDictionary()
            : this(16, false, EqualityComparer<TKey>.Default)
        {
        }

        public LinkedDictionary(IEqualityComparer<TKey> comparer)
            : this(16, false, comparer)
        {
        }

        public LinkedDictionary(bool accessOrder)
            : this(16, accessOrder, EqualityComparer<TKey>.Default)
        {
        }

        public LinkedDictionary(int initialCapacity)
            : this(initialCapacity, false, EqualityComparer<TKey>.Default)
        {
        }

        public LinkedDictionary(int initialCapacity, IEqualityComparer<TKey> comparer)
            : this(initialCapacity, false, comparer)
        {
        }

        public LinkedDictionary(int initialCapacity, bool accessOrder, IEqualityComparer<TKey> comparer)
        {
            IsAccessOrdered = accessOrder;
            _buckets = new HashNode<TKey, TValue>[initialCapacity];
            _rehashThreshold = (int)(_buckets.Length * 1.75f);
            _comparer = comparer;
            _isKeyValueType = typeof(TKey).IsValueType;
        }

        public bool IsAccessOrdered { get; private set; }

        public IEqualityComparer<TKey> Comparer
        {
            get { return _comparer; }
        }

        public TKey FirstKey
        {
            get
            {
                if (_head == null)
                {
                    throw new InvalidOperationException("Collection is empty");
                }
                return _head.Key;
            }
        }

        public TKey LastKey
        {
            get
            {
                if (_tail == null)
                {
                    throw new InvalidOperationException("Collection is empty");
                }
                return _tail.Key;
            }
        }

        public void AddRange(IEnumerable<KeyValuePair<TKey, TValue>> collection)
        {
            foreach (var kp in collection)
            {
                Add(kp.Key, kp.Value);
            }
        }

        #region IDictionary Members

        void IDictionary.Add(object key, object value)
        {
            Add((TKey)key, (TValue)value);
        }

        void IDictionary.Clear()
        {
            Clear();
        }

        bool IDictionary.Contains(object key)
        {
            return ContainsKey((TKey)key);
        }

        IDictionaryEnumerator IDictionary.GetEnumerator()
        {
            return new KeyValueEnumerator(this);
        }

        bool IDictionary.IsFixedSize
        {
            get { return false; }
        }

        bool IDictionary.IsReadOnly
        {
            get { return IsReadOnly; }
        }

        ICollection IDictionary.Keys
        {
            get { return new KeyCollection(this); }
        }

        void IDictionary.Remove(object key)
        {
            Remove((TKey)key);
        }

        ICollection IDictionary.Values
        {
            get { return new ValueCollection(this); }
        }

        object IDictionary.this[object key]
        {
            get { return this[(TKey)key]; }
            set { this[(TKey)key] = (TValue)value; }
        }

        void ICollection.CopyTo(Array array, int index)
        {
            if (array == null)
            {
                throw new ArgumentNullException("array");
            }
            if ((index < 0) || (index > array.Length))
            {
                throw new ArgumentOutOfRangeException("index");
            }
            if ((array.Length - index) < Count)
            {
                throw new ArgumentException("Array too small");
            }
            var keyValuePairArray = array as KeyValuePair<TKey, TValue>[];
            if (keyValuePairArray != null)
            {
                CopyTo(keyValuePairArray, index);
            }
            else if (array is DictionaryEntry[])
            {
                var entryArray = array as DictionaryEntry[];
                foreach (var entry in this)
                {
                    entryArray[index++] = new DictionaryEntry(entry.Key, entry.Value);
                }
            }
            else
            {
                var objArray = array as object[];
                if (objArray == null)
                {
                    throw new ArgumentException("Invalid Array Type", "array");
                }
                try
                {
                    foreach (var entry in this)
                    {
                        objArray[index++] = new KeyValuePair<TKey, TValue>(entry.Key, entry.Value);
                    }
                }
                catch (ArrayTypeMismatchException)
                {
                    throw new ArgumentException("Invalid Array Type", "array");
                }
            }
        }

        int ICollection.Count
        {
            get { return Count; }
        }

        bool ICollection.IsSynchronized
        {
            get { return false; }
        }

        object ICollection.SyncRoot
        {
            get
            {
                if (_syncRoot == null)
                {
                    Interlocked.CompareExchange(ref _syncRoot, new object(), null);
                }
                return _syncRoot;
            }
        }

        #endregion

        #region KeyValueEnumerator

        private struct KeyValueEnumerator : IEnumerator<KeyValuePair<TKey, TValue>>, IDictionaryEnumerator
        {
            private readonly LinkedDictionary<TKey, TValue> _dictionary;
            private HashNode<TKey, TValue> _current;

            public KeyValueEnumerator(LinkedDictionary<TKey, TValue> dictionary)
            {
                if (dictionary == null)
                {
                    throw new ArgumentNullException("dictionary");
                }
                _dictionary = dictionary;
                _current = null;
            }

            #region IDictionaryEnumerator Members

            DictionaryEntry IDictionaryEnumerator.Entry
            {
                get
                {
                    if (_current == null)
                    {
                        throw new InvalidOperationException();
                    }
                    return new DictionaryEntry(_current.Key, _current.Value);
                }
            }

            object IDictionaryEnumerator.Key
            {
                get
                {
                    if (_current == null)
                    {
                        throw new InvalidOperationException();
                    }
                    return _current.Key;
                }
            }

            object IDictionaryEnumerator.Value
            {
                get
                {
                    if (_current == null)
                    {
                        throw new InvalidOperationException();
                    }
                    return _current.Value;
                }
            }

            #endregion

            #region IEnumerator<KeyValuePair<TKey,TValue>> Members

            public KeyValuePair<TKey, TValue> Current
            {
                get
                {
                    if (_current == null)
                    {
                        throw new InvalidOperationException();
                    }
                    return new KeyValuePair<TKey, TValue>(_current.Key, _current.Value);
                }
            }

            public void Dispose()
            {
            }

            object IEnumerator.Current
            {
                get { return new DictionaryEntry(_current.Key, _current.Value); }
            }

            bool IEnumerator.MoveNext()
            {
                return MoveNext();
            }

            void IEnumerator.Reset()
            {
                Reset();
            }

            #endregion

            public bool MoveNext()
            {
                if (_dictionary._head == null)
                {
                    return false;
                }
                if (_current == null)
                {
                    _current = _dictionary._head;
                    return true;
                }
                if (_current.NextInOrder != null)
                {
                    _current = _current.NextInOrder;
                    return true;
                }
                return false;
            }

            public void Reset()
            {
                _current = null;
            }
        }

        #endregion

        #region KeyCollection

        private class KeyCollection : ICollection<TKey>, ICollection
        {
            private readonly LinkedDictionary<TKey, TValue> _dictionary;

            public KeyCollection(LinkedDictionary<TKey, TValue> dictionary)
            {
                _dictionary = dictionary;
            }

            #region ICollection Members

            void ICollection.CopyTo(Array array, int index)
            {
                throw new NotImplementedException();
            }

            int ICollection.Count
            {
                get { return Count; }
            }

            bool ICollection.IsSynchronized
            {
                get { return false; }
            }

            object ICollection.SyncRoot
            {
                get { return this; }
            }

            #endregion

            #region ICollection<TKey> Members

            public void Add(TKey item)
            {
                throw new NotImplementedException();
            }

            public void Clear()
            {
                throw new NotImplementedException();
            }

            public bool Contains(TKey item)
            {
                return _dictionary.ContainsKey(item);
            }

            public void CopyTo(TKey[] array, int arrayIndex)
            {
                throw new NotImplementedException();
            }

            public int Count
            {
                get { return _dictionary.Count; }
            }

            public bool IsReadOnly
            {
                get { throw new NotImplementedException(); }
            }

            public bool Remove(TKey item)
            {
                throw new NotImplementedException();
            }

            public IEnumerator<TKey> GetEnumerator()
            {
                return new KeyEnumerator(_dictionary);
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return GetEnumerator();
            }

            #endregion
        }

        #endregion

        #region KeyEnumerator

        private struct KeyEnumerator : IEnumerator<TKey>
        {
            private readonly LinkedDictionary<TKey, TValue> _dictionary;
            private HashNode<TKey, TValue> _current;

            public KeyEnumerator(LinkedDictionary<TKey, TValue> dictionary)
            {
                if (dictionary == null)
                {
                    throw new ArgumentNullException("dictionary");
                }
                _dictionary = dictionary;
                _current = null;
            }

            #region IEnumerator<TKey> Members

            public TKey Current
            {
                get
                {
                    if (_current == null)
                    {
                        throw new InvalidOperationException();
                    }
                    return _current.Key;
                }
            }

            public void Dispose()
            {
            }

            object IEnumerator.Current
            {
                get { return Current; }
            }

            public bool MoveNext()
            {
                if (_dictionary._head == null)
                {
                    return false;
                }
                if (_current == null)
                {
                    _current = _dictionary._head;
                    return true;
                }
                if (_current.NextInOrder != null)
                {
                    _current = _current.NextInOrder;
                    return true;
                }
                return false;
            }

            public void Reset()
            {
                _current = null;
            }

            #endregion
        }

        #endregion

        #region ValueCollection

        private class ValueCollection : ICollection<TValue>, ICollection
        {
            private readonly LinkedDictionary<TKey, TValue> _dictionary;

            public ValueCollection(LinkedDictionary<TKey, TValue> dictionary)
            {
                _dictionary = dictionary;
            }

            #region ICollection Members

            void ICollection.CopyTo(Array array, int index)
            {
                throw new NotImplementedException();
            }

            int ICollection.Count
            {
                get { return Count; }
            }

            bool ICollection.IsSynchronized
            {
                get { return false; }
            }

            object ICollection.SyncRoot
            {
                get { return this; }
            }

            #endregion

            #region ICollection<TValue> Members

            public void Add(TValue item)
            {
                throw new NotImplementedException();
            }

            public void Clear()
            {
                throw new NotImplementedException();
            }

            public bool Contains(TValue item)
            {
                throw new NotImplementedException();
            }

            public void CopyTo(TValue[] array, int arrayIndex)
            {
                throw new NotImplementedException();
            }

            public int Count
            {
                get { return _dictionary.Count; }
            }

            public bool IsReadOnly
            {
                get { return true; }
            }

            public bool Remove(TValue item)
            {
                throw new NotImplementedException();
            }

            public IEnumerator<TValue> GetEnumerator()
            {
                return new ValueEnumerator(_dictionary);
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return GetEnumerator();
            }

            #endregion
        }

        #endregion

        #region ValueEnumerator

        private struct ValueEnumerator : IEnumerator<TValue>
        {
            private readonly LinkedDictionary<TKey, TValue> _dictionary;
            private HashNode<TKey, TValue> _current;
            private TValue _lastValue;
            private bool _lastValuePresent;

            public ValueEnumerator(LinkedDictionary<TKey, TValue> dictionary)
            {
                _dictionary = dictionary;
                _current = null;
                _lastValuePresent = false;
                _lastValue = default(TValue);
            }

            #region IEnumerator<TValue> Members

            public TValue Current
            {
                get
                {
                    if (_current == null)
                    {
                        throw new InvalidOperationException();
                    }
                    if (_lastValuePresent)
                    {
                        return _lastValue;
                    }
                    // TODO: this could be optimized by caching the value.
                    int hash = _dictionary._comparer.GetHashCode(_current.Key);
                    int bucketIndex = (_dictionary._comparer.GetHashCode(_current.Key) & HashMask) % _dictionary._buckets.Length;
                    for (var node = _dictionary._buckets[bucketIndex]; node != null; node = node.Next)
                    {
                        if (hash == (_dictionary._comparer.GetHashCode(node.Key)) && _dictionary._comparer.Equals(node.Key, _current.Key))
                        {
                            _lastValuePresent = true;
                            _lastValue = node.Value;
                            return _lastValue;
                        }
                    }
                    throw new InvalidOperationException();
                }
            }

            public void Dispose()
            {
            }

            object IEnumerator.Current
            {
                get { return Current; }
            }

            public bool MoveNext()
            {
                _lastValuePresent = false;
                if (_dictionary._head == null)
                {
                    return false;
                }
                if (_current == null)
                {
                    _current = _dictionary._head;
                    return true;
                }
                if (_current.NextInOrder != null)
                {
                    _current = _current.NextInOrder;
                    return true;
                }
                return false;
            }

            public void Reset()
            {
                _lastValuePresent = false;
                _current = null;
            }

            #endregion
        }

        #endregion

        #region IDictionary<TKey,TValue> Members

        public virtual void Add(TKey key, TValue value)
        {
            if (!_isKeyValueType && ReferenceEquals(key, null))
            {
                throw new ArgumentNullException("key");
            }
            Insert(key, value, true);
        }

        public virtual bool ContainsKey(TKey key)
        {
            if (!_isKeyValueType && ReferenceEquals(key, null))
            {
                throw new ArgumentNullException("key");
            }

            int hash = _comparer.GetHashCode(key);
            int bucketIndex = (hash & HashMask) % _buckets.Length;
            for (var node = _buckets[bucketIndex]; node != null; node = node.Next)
            {
                if (hash == (_comparer.GetHashCode(node.Key)) && _comparer.Equals(node.Key, key))
                {
                    if (IsAccessOrdered)
                    {
                        TouchNode(node);
                    }
                    return true;
                }
            }
            return false;
        }

        public ICollection<TKey> Keys
        {
            get { return new KeyCollection(this); }
        }

        public virtual bool Remove(TKey key)
        {
            if (!_isKeyValueType && ReferenceEquals(key, null))
            {
                throw new ArgumentNullException("key");
            }

            int bucketIndex = (_comparer.GetHashCode(key) & HashMask) % _buckets.Length;

            int startCount = _count;
            HashNode<TKey, TValue> previous = null;
            int hash = _comparer.GetHashCode(key);
            var node = _buckets[bucketIndex];
            while (node != null)
            {
                // Supports duplicate key deletion, even though I don't support dup key insertions
                if (hash == (_comparer.GetHashCode(node.Key)) && _comparer.Equals(node.Key, key))
                {
                    UnlinkNode(node);
                    if (previous == null)
                    {
                        _buckets[bucketIndex] = node.Next;
                    }
                    else
                    {
                        previous.Next = node.Next;
                    }
                    _count--;
                }
                previous = node;
                node = node.Next;
            }
            return startCount != _count;
        }

        public virtual bool TryGetValue(TKey key, out TValue value)
        {
            if (!_isKeyValueType && ReferenceEquals(key, null))
            {
                throw new ArgumentNullException("key");
            }

            int hash = _comparer.GetHashCode(key);
            int bucketIndex = (hash & HashMask) % _buckets.Length;
            for (var node = _buckets[bucketIndex]; node != null; node = node.Next)
            {
                if (hash == (_comparer.GetHashCode(node.Key)) && _comparer.Equals(node.Key, key))
                {
                    value = node.Value;
                    if (IsAccessOrdered)
                    {
                        TouchNode(node);
                    }
                    return true;
                }
            }
            value = default(TValue);
            return false;
        }

        public ICollection<TValue> Values
        {
            get { return new ValueCollection(this); }
        }

        public TValue this[TKey key]
        {
            get
            {
                TValue value;
                if (TryGetValue(key, out value))
                {
                    return value;
                }
                return default(TValue);
            }
            set { Insert(key, value, false); }
        }

        public virtual void Add(KeyValuePair<TKey, TValue> item)
        {
            Add(item.Key, item.Value);
        }

        public virtual void Clear()
        {
            _buckets = new HashNode<TKey, TValue>[_buckets.Length];
            _count = 0;
            _head = _tail = null;
        }

        public bool Contains(KeyValuePair<TKey, TValue> item)
        {
            return ContainsKey(item.Key);
        }

        public void CopyTo(KeyValuePair<TKey, TValue>[] array, int index)
        {
            if (array == null)
            {
                throw new ArgumentNullException("array");
            }
            if (index < 0 || index > array.Length)
            {
                throw new ArgumentOutOfRangeException("index");
            }
            if ((array.Length - index) < Count)
            {
                throw new ArgumentException("Array too small");
            }
            foreach (var pair in this)
            {
                if (index < array.Length)
                {
                    array[index++] = new KeyValuePair<TKey, TValue>(pair.Key, pair.Value);
                }
            }
        }

        public int Count
        {
            get { return _count; }
        }

        public bool IsReadOnly
        {
            get { return false; }
        }

        public bool Remove(KeyValuePair<TKey, TValue> item)
        {
            return Remove(item.Key);
        }

        public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
        {
            return new KeyValueEnumerator(this);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        #endregion

        private void Insert(TKey key, TValue value, bool addOnly)
        {
            int hash = _comparer.GetHashCode(key);
            int bucketIndex = (hash & HashMask) % _buckets.Length;
            for (var node = _buckets[bucketIndex]; node != null; node = node.Next)
            {
                if (hash == (_comparer.GetHashCode(node.Key)) && _comparer.Equals(node.Key, key))
                {
                    if (addOnly)
                    {
                        throw new ArgumentException("An item with the same key has already been added.", "key");
                    }
                    node.Key = key;
                    node.Value = value;
                    TouchNode(node);
                }
            }

            var newNode = new HashNode<TKey, TValue> { Key = key, Value = value, Next = _buckets[bucketIndex] };
            _buckets[bucketIndex] = newNode;
            TouchNode(newNode);

            if (++_count > _rehashThreshold)
            {
                GrowCapacity();
            }
        }

        private void GrowCapacity()
        {
            var newBuckets = new HashNode<TKey, TValue>[(_buckets.Length * 2) + 1];
            _rehashThreshold = (int)(_buckets.Length * 1.75f);

            for (int i = 0; i < _buckets.Length; i++)
            {
                var current = _buckets[i];
                while (current != null)
                {
                    int bucketIndex = (_comparer.GetHashCode(current.Key) & HashMask) % newBuckets.Length;
                    var dest = newBuckets[bucketIndex];

                    // Have we already used this new bucket?
                    if (dest != null)
                    {
                        // Append current node to the end
                        while (dest.Next != null)
                        {
                            dest = dest.Next;
                        }
                        dest.Next = current;
                    }
                    else
                    {
                        newBuckets[bucketIndex] = current;
                    }

                    // Advance to next in current items, rehashing may move some items off existing bucket lists.
                    var next = current.Next;
                    current.Next = null;
                    current = next;
                }
            }
            _buckets = newBuckets;
        }

        private void TouchNode(HashNode<TKey, TValue> node)
        {
            if (_head == null)
            {
                _head = _tail = node;
                return;
            }
            if (_head == node)
            {
                return;
            }

            if (node.PreviousInOrder == null && node.NextInOrder == null)
            {
                var temp = _head;
                _head = node;
                _head.NextInOrder = temp;
                temp.PreviousInOrder = _head;
            }
            else
            {
                if (node.NextInOrder != null)
                {
                    node.NextInOrder.PreviousInOrder = node.PreviousInOrder;
                }
                if (node.PreviousInOrder != null)
                {
                    node.PreviousInOrder.NextInOrder = node.NextInOrder;
                }

                var temp = _head;
                _head = node;
                _head.NextInOrder = temp;
                temp.PreviousInOrder = _head;
            }
        }

        private void UnlinkNode(HashNode<TKey, TValue> node)
        {
            if (node == null || (node.PreviousInOrder == null && node.NextInOrder == null))
            {
                return;
            }
            if (node == _head && node == _tail)
            {
                _head = _tail = null;
            }
            else if (node == _head)
            {
                _head = _head.NextInOrder;
            }
            else if (node == _tail)
            {
                _tail = _tail.PreviousInOrder;
            }
            else
            {
                if (node.NextInOrder != null)
                {
                    node.NextInOrder.PreviousInOrder = node.PreviousInOrder;
                }
                if (node.PreviousInOrder != null)
                {
                    node.PreviousInOrder.NextInOrder = node.NextInOrder;
                }
            }
            node.NextInOrder = node.PreviousInOrder = null;
        }

        #region Nested type: HashNode

        private class HashNode<TK, TV>
        {
            public TK Key;
            public HashNode<TK, TV> Next;
            public HashNode<TK, TV> NextInOrder;
            public HashNode<TK, TV> PreviousInOrder;
            public TV Value;
        }

        #endregion
    }
}