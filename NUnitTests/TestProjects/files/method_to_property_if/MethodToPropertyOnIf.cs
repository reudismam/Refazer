using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

//1113fd3db14fd23fc081e90f27f4ddafad7b244d
namespace ExampleProject.Company.MethodToPropertyOnIf
{
    public class MethodToPropertyOnIf
    {
        internal int GetMethodDefOrRefCodedIndex(IMethodReference methodReference)
        {
            if (!treatRefAsPotentialTypeSpec || !typeReference.IsTypeSpecification())
            {
                return (this.GetTypeRefIndex(typeReference) << 2) | 1;
            }

            if (!typeReference.IsTypeSpecification())
            {
                this.GetTypeRefIndex(typeReference);
            }

            if (!typeReference.IsTypeSpecification())
            {
                return 0x01000000 | this.GetTypeRefIndex(typeReference);
            }

            return -1;
        }

        private int GetTypeRefIndex(object typeReference)
        {
            throw new NotImplementedException();
        }

        private bool IsTypeSpecification(object typeReference)
        {
            throw new NotImplementedException();
        }

        public bool treatRefAsPotentialTypeSpec
        {
            get;
            set;
        }

        public TypeReference typeReference
        {
            get;
            set;
        }
    }
}