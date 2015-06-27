// Verophyle.Regexp Copyright © Verophyle Informatics 2015

using System;
using System.Collections.Generic;
using System.Linq;

namespace Verophyle.Regexp
{
    public abstract class InputClass<TInput>
    {
        public abstract ISet<int> InputCodes { get; }

        internal ISet<TInput> Inputs { get; private set; }
        public int TransitionIndex { get; private set; }

        static int NEXT_TRANSITION_INDEX = 0;

        internal InputClass()
        {
            TransitionIndex = NEXT_TRANSITION_INDEX++;
            Inputs = new HashSet<TInput>();
        }

        public InputClass(TInput input)
            : this()
        {
            Inputs.Add(input);
        }

        public InputClass(IEnumerable<TInput> inputs)
            : this()
        {
            foreach (var input in inputs)
                Inputs.Add(input);
        }

        public InputClass(IEnumerable<InputClass<TInput>> inputs)
            : this()
        {
            foreach (var cl in inputs)
                Inputs.UnionWith(cl.Inputs);
        }

        public abstract int GetCode(TInput input);

        public abstract InputClass<TInput> GetNewClassFromCodes(IEnumerable<int> input_codes);

        public virtual IInputClassMatcher<TInput> GetMatcher(IEnumerable<InputClass<TInput>> input_classes)
        {
            return new SimpleSetMatcher<TInput>(input_classes);
        }

        public override bool Equals(object obj)
        {
            var cl = obj as InputClass<TInput>;
            if (cl != null)
                return InputCodes.SetEquals(cl.InputCodes);
            return false;
        }

        public override int GetHashCode()
        {
            return InputCodes.GetHashCode();
        }

        string str = null;

        public override string ToString()
        {
#if !DEBUG
            if (str == null)
#endif
            str = string.Format("{0}: {1}{2}{3}",
                 TransitionIndex,
                 Inputs.Count > 0 ? "{" : string.Empty,
                 System.Text.RegularExpressions.Regex.Escape(string.Join(",", Inputs.Select(i => i.ToString()))),
                 Inputs.Count > 0 ? "}" : string.Empty);

            return str;
        }

        static readonly ISet<int> empty_set = new HashSet<int>();

        public class DotClass : InputClass<TInput>
        {
            public override ISet<int> InputCodes { get { return empty_set; } }

            public DotClass()
                : base()
            {
            }

            public override int GetCode(TInput input)
            {
                throw new NotImplementedException();
            }

            public override InputClass<TInput> GetNewClassFromCodes(IEnumerable<int> input_codes)
            {
                throw new NotImplementedException();
            }

            public override string ToString()
            {
                return string.Format("{0}: .", TransitionIndex);
            }
        }

        internal static readonly DotClass DOT = new DotClass();

    } // class InputClass

    public interface IInputClassMatcher<TInput>
    {
        IEnumerable<InputClass<TInput>> ClassThatMatched(TInput input);
    }

    public class SimpleSetMatcher<TInput> : IInputClassMatcher<TInput>
    {
        protected IDictionary<TInput, InputClass<TInput>> inputs_to_class = new Dictionary<TInput, InputClass<TInput>>();
        protected InputClass<TInput>.DotClass dot_class = null;        

        public SimpleSetMatcher(IEnumerable<InputClass<TInput>> input_classes)
        {
            foreach (var cl in input_classes)
            {
                var dcl = cl as InputClass<TInput>.DotClass;

                if (dcl != null)
                {
                    if (dot_class == null)
                        dot_class = dcl;
                    else
                        throw new Exception("You cannot have more than one dot input class.");
                }

                foreach (var input in cl.Inputs)
                {
                    if (inputs_to_class.ContainsKey(input))
                        throw new Exception("Input classes that are processed by a SimpleSetMatcher need to be disjoint.");
                    inputs_to_class[input] = cl;
                }
            }
        }

        public virtual IEnumerable<InputClass<TInput>> ClassThatMatched(TInput input)
        {
            InputClass<TInput> result;
            if (inputs_to_class.TryGetValue(input, out result))
                yield return result;
            if (dot_class != null)
                yield return dot_class;
        }
    }
}
