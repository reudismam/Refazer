using Spg.LocationRefactor.TextRegion;
using System.Collections.Generic;
using Spg.LocationRefactor.Location;

namespace Spg.LocationRefactor.Observer
{
    public class HighlightEvent
    {
        public List<CodeLocation> Regions { get; set; }

        public HighlightEvent(List<CodeLocation> regions)
        {
            this.Regions = regions;
        }
    }
}
