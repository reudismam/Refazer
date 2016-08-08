using System;
using System.Drawing;
using System.Text.RegularExpressions;
using Microsoft.CodeAnalysis;

namespace Spg.LocationRefactor.TextRegion
{
    /// <summary>
    /// Text region
    /// </summary>
    public class TRegion
    {
        /// <summary>
        /// Text
        /// </summary>
        public String Text { get; set; }

        /// <summary>
        /// Start position
        /// </summary>
        public int Start { get; set; }

        /// <summary>
        /// Region length
        /// </summary>
        public int Length { get; set; }

        /// <summary>
        /// Parent region
        /// </summary>
        public TRegion Parent { get; set; }

        /// <summary>
        /// Syntax node
        /// </summary>
        public SyntaxNode Node { get; set; }

        /// <summary>
        /// Region color
        /// </summary>
        //public Color Color { get; set; }

        /// <summary>
        /// Source code path
        /// </summary>
        /// <returns>Source code path</returns>
        public string Path { get; set; }

        /// <summary>
        /// Evaluate region
        /// </summary>
        /// <param name="region">Region</param>
        /// <returns>Evaluation</returns>
        public bool IsParent(TRegion region) {
            string text = Regex.Escape(this.Text);
            bool contains = Regex.IsMatch(region.Text, text);
            //bool parent = contains && region.Color != this.Color;
            //return parent;
            return contains;
        }

        public bool IntersectWith(TRegion other)
        {
            //if (!other.Path.ToUpperInvariant().Equals(Path.ToUpperInvariant()))
            //{
            //    return false;
            //}
            bool thisWithOther =  this.Start <= other.Start && other.Start <= this.Start + this.Length;
            bool otherWithThis = other.Start <= this.Start  && this.Start <= other.Start + other.Length;
            return (thisWithOther || otherWithThis);
        }

        /// <summary>
        /// Indicate if other region is inside this region.
        /// </summary>
        /// <param name="other">Other region</param>
        /// <returns>True if other object is inside this region</returns>
        public bool IsInside(TRegion other)
        {
            bool thisWithOther = other.Start <= this.Start && this.Start + this.Length<= other.Start + other.Length;
            return (thisWithOther);
        }

        public override string ToString()
        {
            return Start + " : " + Length + "\n" + Text + "\n" + Path;
        }

        public override int GetHashCode()
        {
            return ToString().GetHashCode();
        }

        public override bool Equals(object obj)
        {
            if (!(obj is TRegion)) return false;

            TRegion other = (TRegion) obj;

            return Start.Equals(other.Start) && Length.Equals(other.Length)
                && Path.ToUpperInvariant().Equals(other.Path.ToUpperInvariant());
        }
    }
}



