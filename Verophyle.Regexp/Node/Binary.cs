// Verophyle.Regexp Copyright © Verophyle Informatics 2018

using System.Collections.Generic;

namespace Verophyle.Regexp.Node
{
    public abstract class Binary<T> : Node<T>
    {
        public Binary(Node<T> left, Node<T> right)
            : base()
        {
            Left = left;
            Right = right;
        }

        public Node<T> Left { get; internal set; }
        public Node<T> Right { get; internal set; }

        public override IEnumerable<Node<T>> InLCROrder()
        {
            if (Left != null)
            {
                foreach (var child in Left.InLCROrder())
                    yield return child;
            }
            yield return this;
            if (Right != null)
            {
                foreach (var child in Right.InLCROrder())
                    yield return child;
            }
        }

        public override void Renumber(ref int pos)
        {
            if (Left != null)
                Left.Renumber(ref pos);
            if (Right != null)
                Right.Renumber(ref pos);
        }

        public override void FixNeg(IDictionary<int, ISet<int>> followPos, int lastPos)
        {
            if (Left != null)
                Left.FixNeg(followPos, lastPos);
            if (Right != null)
                Right.FixNeg(followPos, lastPos);
        }

        internal override void TraverseFollowPos(IDictionary<int, ISet<int>> followPos)
        {
            if (Left != null)
                Left.TraverseFollowPos(followPos);
            if (Right != null)
                Right.TraverseFollowPos(followPos);
            CalcFollowPos(followPos);
        }
    }
}
