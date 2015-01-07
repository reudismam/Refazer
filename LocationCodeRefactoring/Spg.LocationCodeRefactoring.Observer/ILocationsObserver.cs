namespace Spg.LocationCodeRefactoring.Observer
{
    public interface ILocationsObserver
    {
        /// <summary>
        /// Notify locations transformed
        /// </summary>
        /// <param name="ltEvent">Event</param>
        void NotifyLocationsSelected(LocationEvent lEvent);
    }
}
