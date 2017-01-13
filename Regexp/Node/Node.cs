// Verophyle.Regexp Copyright © Verophyle Informatics 2017

using System;
using System.Collections.Generic;

namespace Verophyle.Regexp.Node
{
    /// <summary>
    /// An regular expression node with subtypes for concatenation, alternation, Kleene star and epsilon.
    /// </summary>
    public abstract class Node<T>
    {
        bool? nullable;
        ISet<int> firstPos, lastPos;
        IDictionary<int, ISet<int>> followPos;

        public Node()
        {
        }

        public bool Nullable { get { return nullable ?? (bool)(nullable = CalcNullable()); } }
        public ISet<int> FirstPos { get { return firstPos ?? (firstPos = CalcFirstPos()); } }
        public ISet<int> LastPos { get { return lastPos ?? (lastPos = CalcLastPos()); } }

        public IDictionary<int, ISet<int>> FollowPos
        {
            get
            {
                if (followPos == null)
                {
                    followPos = new Dictionary<int, ISet<int>>();
                    TraverseFollowPos(followPos);
                }
                return followPos;
            }
        }

        public virtual IEnumerable<Node<T>> InLCROrder()
        {
            yield return this;
        }

        public virtual void Renumber(ref int pos)
        {
        }

        public virtual void FixNeg(IDictionary<int, ISet<int>> followPos, int lastPos)
        {
        }

        protected abstract bool CalcNullable();
        protected abstract ISet<int> CalcFirstPos();
        protected abstract ISet<int> CalcLastPos();

        protected virtual void CalcFollowPos(IDictionary<int, ISet<int>> followPos)
        {
        }

        internal virtual void TraverseFollowPos(IDictionary<int, ISet<int>> followPos)
        {
        }

        protected void AddFollowPos(IDictionary<int, ISet<int>> followPos, int i, int j)
        {
            ISet<int> set;
            if (!followPos.TryGetValue(i, out set))
            {
                set = new HashSet<int>();
                followPos[i] = set;
            }
            set.Add(j);
        }

        string debugStr = null;

        public override string ToString()
        {
#if !DEBUG
            if (str == null)
#endif
            debugStr = this.ToString("");
            return debugStr;
        }

        public abstract string ToString(string indent);
    }
}
