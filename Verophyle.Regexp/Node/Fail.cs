// Verophyle.Regexp Copyright © Verophyle Informatics 2018

using System.Collections.Generic;

namespace Verophyle.Regexp.Node
{
    public class Fail<T> : Disj<T>
    {
        public Fail(IInputSet<T> value, ref int pos)
            : base(new Leaf<T>(value, ref pos), new Dot<T>(new InputSet.DotSet<T>(), ref pos))
        {
        }

        protected override bool CalcNullable()
        {
            return false;
        }

        public override void FixNeg(IDictionary<int, ISet<int>> followPos, int lastPos)
        {
            foreach (var pos in new[] { ((Leaf<T>)Left).Pos })
            {
                ISet<int> set;
                if (!followPos.TryGetValue(pos, out set))
                {
                    set = new HashSet<int>();
                    followPos[pos] = set;
                }
                set.Clear();
                set.Add(lastPos);
            }
        }

        public override string ToString(string indent)
        {
            return string.Format("{0}!\n{1}\n{0}  .", indent, Left.ToString(indent + "  "));
        }
    }
}
