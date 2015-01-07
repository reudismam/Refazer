using Spg.LocationRefactor.Program;
using System.Collections.Generic;
using System;
using LocationCodeRefactoring.Spg.LocationRefactor.Transformation;

namespace Spg.LocationCodeRefactoring.Observer
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