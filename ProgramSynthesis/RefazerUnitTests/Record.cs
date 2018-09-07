using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RefazerUnitTests
{
    public class Record
    {
        public string Commit { get; set; }
        public double Time { get; set; }
        public int Examples { get; set; }
        public int Locations { get; set; }
        public int AcTransformation { get; set; }
        public int Documents { get; set; }
        public string Program { get; set; }
        public double TimeToLearnEdit { get; set; }
        public double TimeToTranformEdit { get; set; }
        public double Mean { get; set; }
    }
}
