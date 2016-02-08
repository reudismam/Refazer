using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ExampleProject.Company.AddContextParam
{
    class IArrayTypeReference : ITypeReference
    {
        internal ITypeReference GetElementType(object Context)
        {
            throw new NotImplementedException();
        }
    }
}
