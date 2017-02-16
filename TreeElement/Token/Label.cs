using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TreeElement.Token
{
    public class Label
    {
        public string Value { get; set; }

        public Label(string label)
        {
            Value = label;
        }

        public override bool Equals(object obj)
        {
            if (!(obj is Label)) return false;

            Label other = (Label)obj;

            return Value.Equals(other.Value);
        }

        public bool IsLabel(Label label)
        {
            return Equals(label);
        }

        public override int GetHashCode()
        {
            return ToString().GetHashCode();
        }

        public override string ToString()
        {
            return Value;
        }
    }
}