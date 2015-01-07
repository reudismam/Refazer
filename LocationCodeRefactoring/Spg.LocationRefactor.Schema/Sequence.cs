using System.Collections.Generic;
using System;

namespace Spg.LocationRefactor.Schema
{
    [Obsolete("Not used anymore", true)]
    public class Sequence: IElement
    {
        public List<Field> fields;

    }
}
