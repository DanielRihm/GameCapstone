using System;
using UnityEngine;

namespace LCPS.SlipForge.Engine
{
    [HideInInspector]
    [Serializable]
    public class Observable<T> : IObservable<T>
    {
        [HideInInspector][SerializeField] private T value;

        // Backing field for the event
        private event Action<T> _onValueChanged;

        // We want observers to be hot loaded
        public event Action<T> OnValueChanged
        {
            add
            {
                _onValueChanged += value;
                value.Invoke(this.value);
            }
            remove
            {
                _onValueChanged -= value;
            }
        }

        public Observable(T initialValue = default)
        {
            value = initialValue;
        }

        public T Value
        {
            get => value;
            set
            {
                if (!Equals(this.value, value))
                {
                    this.value = value;
                    Notify();
                }
            }
        }

        public void Notify()
        {
            _onValueChanged?.Invoke(this.value);
        }
    }
}
