using System;
using System.Collections.Generic;

namespace LCPS.SlipForge.Engine
{
    public static class ObservableExtensions
    {
        public static void Subscribe<T>(this Observable<T> observable, Action<T> listener)
        {
            observable.OnValueChanged += listener;

            // hot start
            listener.Invoke(observable.Value);
        }

        public static void Subscribe<T>(this Observable<T> observable, Action<Observable<T>> listener)
        {
            observable.OnValueChanged += (value) => listener.Invoke(observable);

            // hot start
            listener.Invoke(observable);
        }

        public static void Subscribe<T>(this ObservableList<T> observable, Action<int?, List<T>> listener)
        {
            observable.OnValueChanged += listener;

            listener.Invoke(null, observable.Value);
        }

        public static void UnSubscribe<T>(this Observable<T> observable, Action<T> listener)
        {
            observable.OnValueChanged -= listener;
        }

        public static void UnSubscribe<T>(this Observable<T> observable, Action<Observable<T>> listener)
        {
            observable.OnValueChanged -= (value) => listener.Invoke(observable);
        }

        public static void UnSubscribe<T>(this ObservableList<T> observable, Action<int?, List<T>> listener)
        {
            observable.OnValueChanged -= listener;
        }

        public static void SetValue<T>(this Observable<T> observable, T value)
        {
            observable.Value = value;
        }

        public static void SetObservable<T>(this Observable<T> observable, Func<Observable<T>> value)
        {
            observable = value.Invoke();
        }
    }
}
