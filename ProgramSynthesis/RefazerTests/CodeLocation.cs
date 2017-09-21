using Spg.LocationRefactor.TextRegion;

namespace Spg.LocationRefactor.Location
{
    /// <summary>
    /// Represents a location
    /// </summary>
    public class CodeLocation
    {
        /// <summary>
        /// Source code
        /// </summary>
        /// <returns>source code</returns>
        public string SourceCode { get; set; }

        /// <summary>
        /// Region in the source code
        /// </summary>
        /// <returns>Region</returns>
        public TRegion Region { get; set; }

        /// <summary>
        /// Source class
        /// </summary>
        /// <returns>Source class</returns>
        public string SourceClass { get; set; }

        public override bool Equals(object obj)
        {
            if (!(obj is CodeLocation)) return false;

            CodeLocation other = (CodeLocation) obj;

            return SourceClass.ToUpperInvariant().Equals(other.SourceClass.ToUpperInvariant()) && Region.Start == other.Region.Start && Region.Length == other.Region.Length;
        }

        public override string ToString()
        {
            return SourceClass + "\n" + Region.Start + " : " + Region.Length + "\n";
        }

        public override int GetHashCode()
        {
            return ToString().GetHashCode();
        }

        //public object Clone()
        //{
        //    CodeLocation cdLocation = new CodeLocation();
        //    cdLocation.SourceCode = SourceCode;
        //    cdLocation.Region = Region;
        //    cdLocation.SourceClass = SourceClass;
        //    return cdLocation;
        //}
    }
}
