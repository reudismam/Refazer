namespace Spg.LocationCodeRefactoring.Observer
{
    public interface ILocationsTransformedObserver
    {
        /// <summary>
        /// Notify locations transformed
        /// </summary>
        /// <param name="ltEvent">Event</param>
        void NotifyLocationsTransformed(LocationsTransformedEvent ltEvent);
    }
}
