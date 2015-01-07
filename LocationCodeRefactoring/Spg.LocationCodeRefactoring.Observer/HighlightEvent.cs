using Spg.LocationRefactor.TextRegion;
using System.Collections.Generic;

namespace Spg.LocationCodeRefactoring.Observer
{
    public class HighlightEvent
    {
        public List<TRegion> regions { get; set; }

        public HighlightEvent(List<TRegion> regions)
        {
            this.regions = regions;
        }
    }
}