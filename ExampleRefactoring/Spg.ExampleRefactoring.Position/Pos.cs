using System;
using System.Linq;
using Spg.ExampleRefactoring.Comparator;
using Spg.ExampleRefactoring.RegularExpression;
using Spg.ExampleRefactoring.Synthesis;
using Spg.LocationRefactoring.Tok;

namespace Spg.ExampleRefactoring.Position
{
    /// <summary>
    /// Pos operator
    /// </summary>
    public class Pos : IPosition
    {
        /// <summary>
        /// First regular expression
        /// </summary>
        public TokenSeq R1 { get; set; }

        /// <summary>
        /// Second regular expression
        /// </summary>
        public TokenSeq R2 { get; set; }

        /// <summary>
        /// Kth index of match
        /// </summary>
        public int Position { get; set; }


        /// <summary>
        /// Pos expression
        /// </summary>
        /// <param name="r1">Match the left of nodes</param>
        /// <param name="r2">Match the right of nodes</param>
        /// <param name="position">Kth position of match [r1r2]</param>
        public Pos(TokenSeq r1, TokenSeq r2, int position)
        {
            this.R1 = r1;
            this.R2 = r2;
            this.Position = position;
        }

        /// <summary>
        /// Position index of match
        /// </summary>
        /// <param name="s">Node list</param>
        /// <returns>Index</returns>
        public int GetPositionIndex(ListNode s)
        {
            if(s == null || R1 == null || R2 == null)
            {
                return -1;
            }

            if (R1.Tokens == null || R2.Tokens == null)
            {
                return -1;
            }

            return GetPositionIndex(s, this.R1, this.R2, this.Position);
        }

        /// <summary>
        /// Size
        /// </summary>
        /// <returns>Size</returns>
        [Obsolete]
        public int Size()
        { //This need to be refactored following the paper definition. Or definition a new regular expression size metric.
            return 0;
        }

        /// <summary>
        /// Pos expression
        /// </summary>
        /// <param name="s">ListNode</param>
        /// <param name="rExpr1">Match the left of nodes</param>
        /// <param name="rExpr2">Match the right of nodes</param>
        /// <param name="position">Kth position of match [r1r2]</param>
        public static int GetPositionIndex(ListNode s, TokenSeq rExpr1, TokenSeq rExpr2, int position)
        {
            if (s == null || rExpr1 == null || rExpr2 == null)
            {
                return -1;
            }

            TokenSeq seq = ASTProgram.ConcatenateRegularExpression(rExpr1, rExpr2);
            TokenSeq regex = seq;

            var match = new RegexComparer().Matches(s, regex);
            int length = match.Count;

            if (length <= 0 || ((position >= 0 && length - position < 0) || (position < 0 && length + position < 0)))
            {
                return -1;
            }

            int index, posIndex;
            if (position > 0)
            {
                posIndex = position - 1;
            }
            else
            {
                posIndex = length + position;
            }

            if (position == 0)
            {
                return -1;
            }

            index = match[posIndex].Item1;

            //ListNode subNodes = ASTManager.SubNotes(s, index + rExpr1.Length(), s.Length() - (index + rExpr1.Length()));
            //index += rExpr1.Length() + Regex.Matches(subNodes, rExpr2)[0].Item1;

            //ListNode subNodes = ASTManager.SubNotes(s, index, s.Length() - index);
            //int increment = new RegexComparer().Matches(subNodes, rExpr1)[0].Item1;
            //subNodes = ASTManager.SubNotes(s, index + increment, s.Length() - (index + increment));
            //index += increment + Regex.Matches(subNodes, rExpr2)[0].Item1;
            return FindIndex(s, rExpr1, rExpr2, index, match[posIndex]);
        }

        private static int FindIndex(ListNode s, TokenSeq rExpr1, TokenSeq rExpr2, int index, Tuple<int, ListNode> match)
        {
            if (rExpr1.Length() == 0) return index;

            if (rExpr2.Length() == 0) return index + match.Item2.Length();

            ListNode matchNodes = match.Item2;

            //int LengthExpr1 = Regex.Matches(matchNodes, rExpr1)[0].Item1;
            //int LengthExpr2 = Regex.Matches(matchNodes, rExpr2)[0].Item1;
            ListNode mn = Regex.Matches(matchNodes, rExpr1)[0].Item2;
            int lengthExpr1 = index + mn.Length();

            if (rExpr2.Tokens[0].Match(mn.List[mn.Length() - 1]))
            {
                lengthExpr1 -= 1;
            }

            ////int index3 = index + (LengthExpr2 - LengthExpr1);

            //int index2 = index;
            //ListNode subNodes = ASTManager.SubNotes(s, index + rExpr1.Length(), s.Length() - (index + rExpr1.Length()));
            ////try { 
            //    index2 += rExpr1.Length() + Regex.Matches(subNodes, rExpr2)[0].Item1;
            ////} catch(Exception e)
            ////{
            //    TokenSeq seq = ASTProgram.ConcatenateRegularExpression(rExpr1, rExpr2);
            //    TokenSeq regex = seq;
            //    Regex.Matches(s, regex);
            ////}
            ////return index2;

            ////return index3;
            return lengthExpr1;
        }


        /// <summary>
        /// String representation
        /// </summary>
        /// <returns>String representation of this object</returns>
        public override string ToString()
        {
            if(R1 == null || R2 == null)
            {
                return null;
            }

            if (R1.Tokens == null || R2.Tokens == null)
            {
                return null;
            }

            string r1String = this.R1.ToString();
            string r2String = this.R2.ToString();

            if (R1.Tokens.Count() == 0)
            {
                r1String = "Empty";
            }

            if (R2.Tokens.Count() == 0)
            {
                r2String = "Empty";
            }
            return "Pos(vi, " + r1String + ", " + r2String + ", " + this.Position + ")";
        }

        /// <summary>
        /// Equals
        /// </summary>
        /// <param name="obj">Another object</param>
        /// <returns>True if object obj is equals to this instance</returns>
        public override bool Equals(object obj)
        {
            if (!(obj is Pos))
            {
                return false;
            }

            Pos another = obj as Pos;
            return another.R1.Equals(this.R1) && another.R2.Equals(this.R2) && another.Position == this.Position;
        }

        /// <summary>
        /// Hash code
        /// </summary>
        /// <returns>Hash code for this object</returns>
        public override int GetHashCode()
        {
            return this.ToString().GetHashCode();
        }
    }
}


