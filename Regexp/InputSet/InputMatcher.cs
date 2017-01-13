// Verophyle.Regexp Copyright © Verophyle Informatics 2017

using System;
using System.Collections.Generic;

namespace Verophyle.Regexp
{
    public interface IInputMatcher<T>
    {
        IEnumerable<IInputSet<T>> GetSetsThatMatch(T input);
        IInputSet<T> GetNewSetFromCodes(IEnumerable<int> codes);
    }

    namespace InputSet
    {
        public abstract class InputMatcher<T> : IInputMatcher<T>
        {
            protected readonly IDictionary<T, IInputSet<T>> setsByInput = new Dictionary<T, IInputSet<T>>();
            protected DotSet<T> dotClass = null;

            public InputMatcher(IEnumerable<IInputSet<T>> inputSets)
            {
                foreach (var inputSet in inputSets)
                {
                    var ds = inputSet as DotSet<T>;

                    if (ds != null)
                    {
                        if (dotClass == null)
                            dotClass = ds;
                        else
                            throw new Exception("You cannot have more than one dot input class in an input set matcher.");
                    }

                    foreach (var input in inputSet.Inputs)
                    {
                        if (setsByInput.ContainsKey(input))
                            throw new Exception("Input classes that are processed by an input set matcher need to be disjoint.");
                        setsByInput[input] = inputSet;
                    }
                }
            }

            public abstract IInputSet<T> GetNewSetFromCodes(IEnumerable<int> codes);                

            public virtual IEnumerable<IInputSet<T>> GetSetsThatMatch(T input)
            {
                IInputSet<T> result;
                if (setsByInput.TryGetValue(input, out result))
                    yield return result;
                if (dotClass != null)
                    yield return dotClass;
            }
        }
    }
}
