// Verophyle.Regexp Copyright © Verophyle Informatics 2015

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Verophyle.Regexp.Node
{
    public class Leaf<TInputClass> : Regexp<TInputClass>
    {
        public int Pos { get; internal set; }

        public Leaf(TInputClass value, ref int pos)
            : base(null, null)
        {
            Input = value;
            Pos = pos++;
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
    } // class Leaf

    public class Epsilon<TInputClass> : Leaf<TInputClass>
    {
        public Epsilon(ref int pos)
            : base(default(TInputClass), ref pos)
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
    } // class Epsilon

    public class End<TInputClass> : Leaf<TInputClass>
    {
        public End(ref int pos)
            : base(default(TInputClass), ref pos)
        {
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
            return new HashSet<int>();
        }

        public override string ToString(string indent)
        {
            return indent + "■";
        }
    }

    public class Dot<TInputClass> : Leaf<TInputClass>
    {
        public Dot(ref int pos)
            : base(default(TInputClass), ref pos)
        {
            var type = typeof(TInputClass);
            var dot_info = type.GetMember("DOT", BindingFlags.Static | BindingFlags.NonPublic);
            if (dot_info != null && dot_info.Length > 0)
            {
                if (dot_info[0].MemberType == MemberTypes.Field)
                    Input = (TInputClass)((FieldInfo)dot_info[0]).GetValue(null);
            }
        }

        public override string ToString(string indent)
        {
            return indent + "•";
        }
    }

    public class Cat<TInputClass> : Regexp<TInputClass>
    {
        public Cat(Regexp<TInputClass> left, Regexp<TInputClass> right)
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

        protected override void CalcFollowPos(IDictionary<int, ISet<int>> follow_pos)
        {
            foreach (int i in Left.LastPos)
            {
                foreach (int j in Right.FirstPos)
                    AddFollowPos(follow_pos, i, j);
            }
        }

        public override string ToString(string indent)
        {
            return string.Format("{0}+\n{1}\n{2}", indent, Left.ToString(indent + "  "), Right.ToString(indent + "  "));
        }
    } // class Cat

    public class Or<TInputClass> : Regexp<TInputClass>
    {
        public Or(Regexp<TInputClass> left, Regexp<TInputClass> right)
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

    } // class Or

    public class Fail<TInputClass> : Or<TInputClass>
    {
        public Fail(TInputClass value, ref int pos)
            : base(new Leaf<TInputClass>(value, ref pos), new Dot<TInputClass>(ref pos))
        {
        }

        protected override bool CalcNullable()
        {
            return false;
        }

        public override void FixNeg(IDictionary<int, ISet<int>> follow_pos, int last_pos)
        {
            foreach (var pos in new[] { ((Leaf<TInputClass>)Left).Pos })
            {
                ISet<int> set;
                if (!follow_pos.TryGetValue(pos, out set))
                {
                    set = new HashSet<int>();
                    follow_pos[pos] = set;
                }
                set.Clear();
                set.Add(last_pos);
            }
        }

        public override string ToString(string indent)
        {
            return string.Format("{0}!\n{1}\n{0}  .", indent, Left.ToString(indent + "  "));
        }
    } // class Fail

    public class Star<TInputClass> : Regexp<TInputClass>
    {
        public Star(Regexp<TInputClass> child)
            : base(child, null)
        {
        }

        protected override bool CalcNullable()
        {
            return true;
        }

        protected override ISet<int> CalcFirstPos()
        {
            return Left.FirstPos;
        }

        protected override ISet<int> CalcLastPos()
        {
            return Left.LastPos;
        }

        protected override void CalcFollowPos(IDictionary<int, ISet<int>> follow_pos)
        {
            foreach (int i in LastPos)
            {
                foreach (int j in FirstPos)
                    AddFollowPos(follow_pos, i, j);
            }
        }

        public override string ToString(string indent)
        {
            return string.Format("{0}*\n{1}", indent, Left.ToString(indent + "  "));
        }
    } // class Star
}
