// Verophyle.Regexp Copyright © Verophyle Informatics 2017

using System.Collections.Generic;

namespace Verophyle.Regexp.Node
{
    public class Star<T> : Node<T>
    {
        public Star(Node<T> child)
            : base()
        {
            Child = child;
        }

        public Node<T> Child { get; internal set; }

        public override IEnumerable<Node<T>> InLCROrder()
        {
            if (Child != null)
                yield return Child;
            yield return this;
        }

        public override void Renumber(ref int pos)
        {
            if (Child != null)
                Child.Renumber(ref pos);
        }

        public override void FixNeg(IDictionary<int, ISet<int>> followPos, int lastPos)
        {
            if (Child != null)
                Child.FixNeg(followPos, lastPos);
        }

        internal override void TraverseFollowPos(IDictionary<int, ISet<int>> followPos)
        {
            if (Child != null)
                Child.TraverseFollowPos(followPos);
            CalcFollowPos(followPos);
        }

        protected override bool CalcNullable()
        {
            return true;
        }

        protected override ISet<int> CalcFirstPos()
        {
            return Child.FirstPos;
        }

        protected override ISet<int> CalcLastPos()
        {
            return Child.LastPos;
        }

        protected override void CalcFollowPos(IDictionary<int, ISet<int>> followPos)
        {
            foreach (int i in LastPos)
            {
                foreach (int j in FirstPos)
                    AddFollowPos(followPos, i, j);
            }
        }

        public override string ToString(string indent)
        {
            return string.Format("{0}*\n{1}", indent, Child.ToString(indent + "  "));
        }
    }
}
