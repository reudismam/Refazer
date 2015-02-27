namespace Spg.LocationRefactor.Predicate
{
    /// <summary>
    /// Create predicates
    /// </summary>
    public class PredicateFactory
    {
        /// <summary>
        /// Create a new predicate
        /// </summary>
        /// <param name="predicate">Predicate</param>
        /// <returns>Predicate</returns>
        public static IPredicate Create(IPredicate predicate) {
            if (predicate is Contains) {
                return new Contains();
            }

            if (predicate is EndsWith){
                return new EndsWith();
            }
            return null;
        }
    }
}
