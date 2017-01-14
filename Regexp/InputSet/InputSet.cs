// Verophyle.Regexp Copyright © Verophyle Informatics 2017

using System;
using System.Collections.Generic;
using System.Linq;

namespace Verophyle.Regexp
{
    public interface IInputSet<T>
    {
        int TransitionIndex { get; }
        ISet<T> Inputs { get; }
        ISet<int> InputCodes { get; }
    }

    namespace InputSet
    {
        public abstract class InputSet<T> : IInputSet<T>
        {
            static int NEXT_TRANSITION_INDEX = 0;
            static IInputSet<T> dotSet;

            protected ISet<T> inputs;
            protected ISet<int> inputCodes;

            internal InputSet()
            {
                TransitionIndex = NEXT_TRANSITION_INDEX++;
            }

            public InputSet(T input)
                : this()
            {
                Inputs.Add(input);
            }

            public InputSet(IEnumerable<T> inputs)
                : this()
            {
                foreach (var input in inputs)
                    Inputs.Add(input);
            }

            public InputSet(IEnumerable<IInputSet<T>> inputs)
                : this()
            {
                foreach (var cl in inputs)
                    Inputs.UnionWith(cl.Inputs);
            }

            public int TransitionIndex { get; private set; }

            public virtual ISet<T> Inputs
            {
                get { return inputs ?? (inputs = new HashSet<T>()); }
                internal set { inputs = value; }
            }

            public virtual ISet<int> InputCodes
            {
                get { return inputCodes ?? (inputCodes = new HashSet<int>()); }
                internal set { inputCodes = value; }
            }

            public override bool Equals(object obj)
            {
                var cl = obj as IInputSet<T>;
                if (cl != null && cl.InputCodes != null)
                    return InputCodes.SetEquals(cl.InputCodes);
                return false;
            }

            public override int GetHashCode()
            {
                return Inputs.GetHashCode();
            }

            string debugStr = null;

            public override string ToString()
            {
#if !DEBUG
            if (debugStr == null)
#endif
                debugStr = string.Format("{0}: {1}{2}{3}",
                     TransitionIndex,
                     Inputs.Count > 0 ? "{" : string.Empty,
                     System.Text.RegularExpressions.Regex.Escape(string.Join(",", Inputs.Select(i => i.ToString()))),
                     Inputs.Count > 0 ? "}" : string.Empty);

                return debugStr;
            }
        }
    }
}
