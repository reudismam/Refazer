using Spg.LocationRefactor.TextRegion;
using System.Collections.Generic;

namespace Spg.LocationRefactor.Observer
{
    public class HighlightEvent
    {
        public List<TRegion> Regions { get; set; }

        public HighlightEvent(List<TRegion> regions)
        {
            this.Regions = regions;
        }
    }
}
