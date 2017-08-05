using System;
using System.Text.RegularExpressions;
using Microsoft.CodeAnalysis;

namespace RefazerObject.Region
{
    /// <summary>
    /// Text region
    /// </summary>
    public class Region
    {
        /// <summary>
        /// Text
        /// </summary>
        public string Text { get; set; }

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
        public Region Parent { get; set; }

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
        public bool IsParent(Region region) {
            string text = Regex.Escape(Text);
            bool contains = Regex.IsMatch(region.Text, text);
            return contains;
        }

        /// <summary>
        /// Verifies if this region intersect with other region
        /// </summary>
        /// <param name="other">Other region</param>
        public bool IntersectWith(Region other)
        {
            bool thisWithOther = Start <= other.Start && other.Start <= Start + Length;
            bool otherWithThis = other.Start <= Start  && Start <= other.Start + other.Length;
            return thisWithOther || otherWithThis;
        }

        /// <summary>
        /// Indicates if other region is inside this region.
        /// </summary>
        /// <param name="other">Other region</param>
        /// <returns>True if other object is inside this region</returns>
        public bool IsInside(Region other)
        {
            bool thisWithOther = other.Start <= Start && Start + Length<= other.Start + other.Length;
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

        /// <summary>
        /// Determines if this object is equal to the other object
        /// </summary>
        /// <param name="obj">Other object</param>
        public override bool Equals(object obj)
        {
            if (!(obj is Region)) return false;
            Region other = (Region) obj;
            return Start.Equals(other.Start) && Length.Equals(other.Length) && Path.ToUpperInvariant().Equals(other.Path.ToUpperInvariant());
        }
    }
}



