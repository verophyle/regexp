// Verophyle.Regexp Copyright © Verophyle Informatics 2018

using System.Collections.Generic;

namespace Verophyle.Regexp.Node
{
    public class Disj<T> : Binary<T>
    {
        public Disj(Node<T> left, Node<T> right)
            : base(left, right)
        {
        }

        protected override bool CalcNullable()
        {
            return Left.Nullable || Right.Nullable;
        }

        protected override ISet<int> CalcFirstPos()
        {
            var result = new HashSet<int>(Left.FirstPos);
            result.UnionWith(Right.FirstPos);
            return result;
        }

        protected override ISet<int> CalcLastPos()
        {
            var result = new HashSet<int>(Left.LastPos);
            result.UnionWith(Right.LastPos);
            return result;
        }

        public override string ToString(string indent)
        {
            return string.Format("{0}|\n{1}\n{2}", indent, Left.ToString(indent + "  "), Right.ToString(indent + "  "));
        }
    }
}
