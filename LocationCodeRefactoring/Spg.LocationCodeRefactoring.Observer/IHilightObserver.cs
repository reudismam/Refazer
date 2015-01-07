namespace Spg.LocationCodeRefactoring.Observer
{
    public interface IHilightObserver
    {
        /// <summary>
        /// Notify hilight changed
        /// </summary>
        /// <param name="hEvent">Event</param>
        void NotifyHilightChanged(HighlightEvent hEvent);
    }
}
