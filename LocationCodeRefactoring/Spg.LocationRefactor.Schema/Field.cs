using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Spg.LocationRefactor.Schema
{
    [Obsolete("Not used anymore", true)]
    public class Field: IElement
    {
        public String content {get; set;}

        public List<String> examples { get; set; }

        public String color { get; set;}
    }
}
