using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LCPS.SlipForge.Engine
{
    [Serializable]
    public class ObservableDictionary<TKey, TValue> : IObservable<Dictionary<TKey, TValue>>, IEnumerable<KeyValuePair<TKey, TValue>>
    {
        [HideInInspector][SerializeField] private Dictionary<TKey, TValue> value;

        // Event to notify when the dictionary changes
        public event Action<TKey> OnValueChanged;

        public enum ChangeType
        {
            Add,
            Remove,
            Update,
            Clear
        }

        public ObservableDictionary()
        {
            value = new Dictionary<TKey, TValue>();
        }

        public ObservableDictionary(int capacity)
        {
            value = new Dictionary<TKey, TValue>(capacity);
        }

        public ObservableDictionary(Dictionary<TKey, TValue> initialValues)
        {
            value = new Dictionary<TKey, TValue>(initialValues);
        }

        public ObservableDictionary(IEnumerable<KeyValuePair<TKey, TValue>> initialValues)
        {
            value = new Dictionary<TKey, TValue>();
            foreach (var kvp in initialValues)
            {
                value.Add(kvp.Key, kvp.Value);
            }
        }

        // Implement IObservable<T>
        public Dictionary<TKey, TValue> Value
        {
            get => value;
            set
            {
                this.value = new Dictionary<TKey, TValue>(value);
                Notify(default); // Notify that the whole dictionary was changed
            }
        }

        // Implement IEnumerable<KeyValuePair<TKey, TValue>>
        public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
        {
            return value.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        // Notify method to fulfill the IObservable interface requirement
        public void Notify()
        {
            Notify(default); // Default to notifying a full dictionary change
        }

        // Overloaded Notify method for specific changes with a key
        private void Notify(TKey key)
        {
            OnValueChanged?.Invoke(key);
        }

        // Methods to add/remove elements from the dictionary with notification
        public void Add(TKey key, TValue value)
        {
            this.value.Add(key, value);
            Notify(key);
        }

        public bool Remove(TKey key)
        {
            if (this.value.TryGetValue(key, out var _))
            {
                this.value.Remove(key);
                Notify(key);
                return true;
            }
            return false;
        }

        public void Clear()
        {
            this.value.Clear();
            Notify(default);
        }

        public bool TryGetValue(TKey key, out TValue value)
        {
            return this.value.TryGetValue(key, out value);
        }

        public TValue this[TKey key]
        {
            get => value[key];
            set
            {
                this.value[key] = value;
                Notify(key);
            }
        }

        public int Count => value.Count;

        public Dictionary<TKey, TValue> GetDictionary() => value;

        public void SetDictionary(Dictionary<TKey, TValue> newDictionary)
        {
            value = new Dictionary<TKey, TValue>(newDictionary);
            Notify(default); // Notify that the whole dictionary was changed
        }
    }
}