using System;
using System.Collections.Generic;

namespace Spg.LocationRefactor.Schema
{
    [Obsolete("Not used anymore", true)]
    public class Structure
    {
        public List<IElement> elements {get; set;}

        public List<String> examples { get; set; }

        public String color { get; set; }
    }
}
