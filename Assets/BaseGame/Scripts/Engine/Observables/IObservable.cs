namespace LCPS.SlipForge.Engine
{
    public interface IObservable<T> : IObservable
    {
        T Value { get; set; }
    }

    public interface IObservable
    {
        void Notify();
    }
}
