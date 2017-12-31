// Verophyle.Regexp Copyright © Verophyle Informatics 2018

using System.Collections.Generic;

namespace Verophyle.Regexp.Node
{
    public class Leaf<T> : Node<T>
    {
        public Leaf(IInputSet<T> value, ref int pos)
            : base()
        {
            Input = value;
            Pos = pos++;
        }

        public int Pos { get; internal set; }
        public IInputSet<T> Input { get; internal set; }

        public override void Renumber(ref int pos)
        {
            Pos = pos;
        }

        protected override bool CalcNullable()
        {
            return false;
        }

        protected override ISet<int> CalcFirstPos()
        {
            return new HashSet<int> { Pos };
        }

        protected override ISet<int> CalcLastPos()
        {
            return new HashSet<int> { Pos };
        }

        public override string ToString(string indent)
        {
            return string.Format("{0}{2}", indent, Input);
        }
    }
}
