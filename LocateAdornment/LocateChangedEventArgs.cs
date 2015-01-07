using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LocateAdornment
{
    internal class LocateChangedEventArgs : EventArgs
    {
        public readonly LocateAdornment LocationAdded;

        public readonly LocateAdornment LocationRemoved;

        public LocateChangedEventArgs(LocateAdornment added, LocateAdornment removed)
        {
            this.LocationAdded = added;
            this.LocationRemoved = removed;
        }
    }
}
