// Verophyle.Regexp Copyright © Verophyle Informatics 2017

namespace Verophyle.Regexp.Node
{
    public class Dot<T> : Leaf<T>
    {
        public Dot(IInputSet<T> dotSet, ref int pos)
            : base(dotSet, ref pos)
        {
        }

        public override string ToString(string indent)
        {
            return indent + "•";
        }
    }
}
