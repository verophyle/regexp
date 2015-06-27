// Verophyle.Regexp Copyright © Verophyle Informatics 2015

using System;
using System.Collections.Generic;

namespace Verophyle.Regexp
{
    /// <summary>
    /// An abstract regular expression with concatenation, alternation, Kleene star and epsilon.
    /// </summary>
    /// <typeparam name="TInputClass">Type of value in the regular expression.</typeparam>
    public abstract class Regexp<TInputClass>
    {
        protected static readonly HashSet<int> EMPTY_SET = new HashSet<int>();

        public TInputClass Input { get; internal set; }

        bool? nullable;
        ISet<int> first_pos, last_pos;
        IDictionary<int, ISet<int>> follow_pos;

        public Regexp<TInputClass> Left { get; internal set; }
        public Regexp<TInputClass> Right { get; internal set; }

        public bool Nullable
        {
            get
            {
                if (nullable == null)
                    nullable = CalcNullable();
                return (bool)nullable;
            }
        }

        public ISet<int> FirstPos { get { return first_pos ?? (first_pos = CalcFirstPos()); } }
        public ISet<int> LastPos { get { return last_pos ?? (last_pos = CalcLastPos()); } }

        public IDictionary<int, ISet<int>> FollowPos
        { 
            get
            {
                if (follow_pos == null)
                {
                    follow_pos = new Dictionary<int, ISet<int>>();
                    TraverseFollowPos(follow_pos);
                }
                return follow_pos;
            } 
        }

        public Regexp(Regexp<TInputClass> left, Regexp<TInputClass> right)
        {
            Left = left;
            Right = right;
        }

        public void Renumber(ref int pos)
        {
            if (Left != null)
                Left.Renumber(ref pos);
            if (Right != null)
                Right.Renumber(ref pos);
        }

        public virtual void FixNeg(IDictionary<int, ISet<int>> follow_pos, int last_pos)
        {
            if (Left != null)
                Left.FixNeg(follow_pos, last_pos);
            if (Right != null)
                Right.FixNeg(follow_pos, last_pos);
        }

        protected abstract bool CalcNullable();
        protected abstract ISet<int> CalcFirstPos();
        protected abstract ISet<int> CalcLastPos();

        protected virtual void CalcFollowPos(IDictionary<int, ISet<int>> follow_pos)
        {
        }

        protected void AddFollowPos(IDictionary<int, ISet<int>> follow_pos, int i, int j)
        {
            ISet<int> set;
            if (!follow_pos.TryGetValue(i, out set))
            {
                set = new HashSet<int>();
                follow_pos[i] = set;
            }
            set.Add(j);
        }

        void TraverseFollowPos(IDictionary<int, ISet<int>> follow_pos)
        {
            if (Left != null)
                Left.TraverseFollowPos(follow_pos);
            if (Right != null)
                Right.TraverseFollowPos(follow_pos);
            CalcFollowPos(follow_pos);
        }

        string str = null;

        public override string ToString()
        {
#if !DEBUG
            if (str == null)
#endif
            str = this.ToString("");
            return str;
        }

        public abstract string ToString(string indent);
    }
}
