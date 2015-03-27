using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ExampleProject.Company.AddContextParam
{
    class IPointerTypeReference : ITypeReference
    {
        internal ITypeReference GetTargetType(object Context)
        {
            throw new NotImplementedException();
        }
    }
}
