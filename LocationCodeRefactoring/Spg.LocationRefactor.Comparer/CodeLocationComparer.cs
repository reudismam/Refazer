using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Spg.LocationRefactor.Location;

namespace Spg.LocationRefactor.Comparer
{
    internal class CodeLocationComparer: IComparer
    {
        public int Compare(object x, object y)
        {
            CodeLocation cx = (CodeLocation) x;
            CodeLocation cy = (CodeLocation) y;

            return Comparer(cx, cy);
        }

        public int Comparer(CodeLocation x, CodeLocation y)
        {
            return x.Region.Start - y.Region.Start;
        }
    }
}
