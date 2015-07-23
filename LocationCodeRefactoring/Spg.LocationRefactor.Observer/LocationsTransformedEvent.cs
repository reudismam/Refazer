using System.Collections.Generic;
using Spg.LocationRefactor.Transform;

namespace Spg.LocationRefactor.Observer
{
    /// <summary>
    /// Location transformed event
    /// </summary>
    public class LocationsTransformedEvent
    {

        public List<CodeTransformation> transformations { get; set; }

        public LocationsTransformedEvent(List<CodeTransformation> transformations) {
            this.transformations = transformations;
        }
    }
}

