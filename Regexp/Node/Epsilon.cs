// Verophyle.Regexp Copyright © Verophyle Informatics 2017

using System.Collections.Generic;

namespace Verophyle.Regexp.Node
{
    public class Epsilon<TInput> : Leaf<TInput>
    {
        public Epsilon(ref int pos)
            : base(null, ref pos)
        {
        }

        protected override bool CalcNullable()
        {
            return true;
        }

        protected override ISet<int> CalcFirstPos()
        {
            return new HashSet<int>();
        }

        protected override ISet<int> CalcLastPos()
        {
            return new HashSet<int>();
        }

        public override string ToString(string indent)
        {
            return indent + "ε";
        }
    }
}
