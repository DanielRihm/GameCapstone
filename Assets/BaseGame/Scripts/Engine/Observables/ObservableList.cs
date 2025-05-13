namespace LCPS.SlipForge.Engine
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    [Serializable]
    public class ObservableList<T> : IObservable<List<T>>, IEnumerable<T>
    {
        [HideInInspector][SerializeField] private List<T> value;

        // Event to notify when the entire list changes or when an element changes
        // The parameters are: (index, newListState)
        public event Action<int?, List<T>> OnValueChanged;

        private int? _index;

        public ObservableList()
        {
            value = new List<T>();
        }

        public ObservableList(int capacity)
        {
            value = new List<T>(capacity);
        }

        public ObservableList(List<T> initialValues)
        {
            value = new List<T>(initialValues);
        }

        public ObservableList(IEnumerable<T> initialValues)
        {
            value = new List<T>(initialValues);
        }

        // Implement IObservable<T>
        public List<T> Value
        {
            get => value;
            set
            {
                this.value = new List<T>(value);
                Notify(null); // Notify with null index to indicate whole list change
            }
        }

        // Notify method to fulfill the IObservable interface requirement
        public void Notify()
        {
            Notify(null); // Default to notifying a full list change
        }

        // Overloaded Notify method for specific changes with an index
        private void Notify(int? index)
        {
            // Use this overload when a specific change occurs (e.g., adding or removing an element)
            OnValueChanged?.Invoke(index, value);
        }

        // Methods to add/remove elements from the list with notification
        public void Add(T item)
        {
            value.Add(item);
            Notify(value.Count - 1); // Notify with the added item's index
        }

        public void RemoveAt(int index)
        {
            value.RemoveAt(index);
            Notify(index); // Notify about the removal
        }

        public void Clear()
        {
            value.Clear();
            Notify(null); // Notify that the entire list was cleared
        }

        public int Count => value.Count;

        public List<T> GetList() => value;

        public void SetList(List<T> newList)
        {
            value = new List<T>(newList);
            Notify(null); // Notify that the whole list was changed
        }

        public IEnumerator<T> GetEnumerator()
        {
            return value.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }

}
