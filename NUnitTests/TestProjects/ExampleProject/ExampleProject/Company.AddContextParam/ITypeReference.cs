using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ExampleProject.Company.AddContextParam
{
    class ITypeReference
    {
        public INestedTypeReference AsNestedTypeReference { get; set; }

        public IGenericTypeInstanceReference AsGenericTypeInstanceReference { get; set; }
    }
}
