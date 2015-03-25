using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExampleProject.Company.AddContextParam
{
    public class AddContextParam
    {
        private void AppendAssemblyQualifierIfNecessary(StringBuilder sb, ITypeReference typeReference, out bool isAssemQualified, EmitContext context)
        {
            INestedTypeReference nestedType = typeReference.AsNestedTypeReference;
            if (nestedType != null)
            {
                this.AppendAssemblyQualifierIfNecessary(sb, nestedType.GetContainingType(Context), out isAssemQualified, context);
                return;
            }

            IGenericTypeInstanceReference genInst = typeReference.AsGenericTypeInstanceReference;
            if (genInst != null)
            {
                this.AppendAssemblyQualifierIfNecessary(sb, genInst.GenericType, out isAssemQualified, context);
                return;
            }

            IArrayTypeReference arrType = typeReference as IArrayTypeReference;
            if (arrType != null)
            {
                this.AppendAssemblyQualifierIfNecessary(sb, arrType.GetElementType(Context), out isAssemQualified);

            }

            IPointerTypeReference pointer = typeReference as IPointerTypeReference;
            if (pointer != null)
            {
                this.AppendAssemblyQualifierIfNecessary(sb, pointer.GetTargetType(Context), out isAssemQualified);

            }

            IManagedPointerTypeReference reference = typeReference as IManagedPointerTypeReference;
            if (reference != null)
            {
                this.AppendAssemblyQualifierIfNecessary(sb, pointer.GetTargetType(Context), out isAssemQualified);

            }
            isAssemQualified = false;
        }

        private void AppendAssemblyQualifierIfNecessary(StringBuilder sb, ITypeReference typeReference, out bool isAssemQualified)
        {
            throw new NotImplementedException();
        }

        public object Context { get; set; }
    }
}