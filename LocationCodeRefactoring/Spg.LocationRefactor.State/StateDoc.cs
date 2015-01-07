using System;
using System.Collections.Generic;

namespace Spg.LocationRefactor.State
{
    [Obsolete("Not used anymore", true)]
    public class StateDoc
    {
        public Dictionary<String, List<String>> highlight {get; set;}

        public StateDoc(Dictionary<String, List<String>> highlight)
        {
            this.highlight = highlight;
        }
    }
}
