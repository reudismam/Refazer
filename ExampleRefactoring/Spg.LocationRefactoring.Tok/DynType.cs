namespace Spg.LocationRefactoring.Tok
{
    internal class DynType
    {

        public const string STRING = "STRING";
        public const string NUMBER = "NUMBER";
        public const string FULLNAME = "FULL_NAME";
        public string fullName { get; set; }

        private string type { get; set; }

        public DynType(string fullName, string type)
        {
            this.fullName = fullName;
            this.type = type;
        }

        public override bool Equals(object obj)
        {
            if (!(obj is DynType))
            {
                return false;
            }
            DynType other = obj as DynType;

            return this.fullName.Equals(other.fullName) && this.type.Equals(other.type);
        }

        public override string ToString()
        {
            return this.fullName.ToString();
        }

        public override int GetHashCode()
        {
            return this.ToString().GetHashCode();
        }
    }
}