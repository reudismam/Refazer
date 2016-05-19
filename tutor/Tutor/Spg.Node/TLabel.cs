namespace Tutor.Spg.Node
{
    public class TLabel
    {
        object Label { get; set; }

        public TLabel(object label)
        {
            Label = label;
        }

        public override bool Equals(object obj)
        {
            if (!(obj is TLabel)) return false;

            TLabel other = (TLabel) obj;

            return Label.Equals(other.Label);
        }

        public override int GetHashCode()
        {
            return ToString().GetHashCode();
        }

        public override string ToString()
        {
            return Label.ToString();
        }
    }
}
