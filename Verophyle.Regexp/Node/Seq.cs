// Verophyle.Regexp Copyright © Verophyle Informatics 2018

using System.Collections.Generic;

namespace Verophyle.Regexp.Node
{
    public class Seq<T> : Binary<T>
    {
        public Seq(Node<T> left, Node<T> right)
            : base(left, right)
        {
        }

        protected override bool CalcNullable()
        {
            return Left.Nullable && Right.Nullable;
        }

        protected override ISet<int> CalcFirstPos()
        {
            ISet<int> result;

            if (Left.Nullable)
            {
                result = new HashSet<int>(Left.FirstPos);
                result.UnionWith(Right.FirstPos);
            }
            else
            {
                result = Left.FirstPos;
            }

            return result;
        }

        protected override ISet<int> CalcLastPos()
        {
            ISet<int> result;

            if (Right.Nullable)
            {
                result = new HashSet<int>(Left.LastPos);
                result.UnionWith(Right.LastPos);
            }
            else
            {
                result = Right.LastPos;
            }

            return result;
        }

        protected override void CalcFollowPos(IDictionary<int, ISet<int>> followPos)
        {
            foreach (int i in Left.LastPos)
            {
                foreach (int j in Right.FirstPos)
                    AddFollowPos(followPos, i, j);
            }
        }

        public override string ToString(string indent)
        {
            return string.Format("{0}+\n{1}\n{2}", indent, Left.ToString(indent + "  "), Right.ToString(indent + "  "));
        }
    }
}
