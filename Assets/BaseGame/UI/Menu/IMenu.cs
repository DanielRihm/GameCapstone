namespace LCPS.SlipForge.UI
{
    public interface IMenu
    {
        // all methods assume that IsValid == true
        public bool IsValid { get; }
        public void Open(bool doStacking = false);
        public void Close();
        public bool IsOpen();
    }
}
