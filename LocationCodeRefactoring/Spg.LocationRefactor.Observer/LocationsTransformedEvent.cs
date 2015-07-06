using System.Collections.Generic;
using System;
using Spg.LocationRefactor.Transformation;

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

