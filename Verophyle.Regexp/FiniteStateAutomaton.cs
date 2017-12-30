// Verophyle.Regexp Copyright © Verophyle Informatics 2017

using System.Collections.Generic;

namespace Verophyle.Regexp
{
    public abstract class FiniteStateAutomaton<T>
    {
        public enum SpecialIndices
        {
            FAIL = -1,
            END = -2,
            DOT = -3,
        }

        public abstract bool Succeeded { get; }
        public abstract bool Failed { get; }

        public abstract bool Matches(IEnumerable<T> inputs);
        public abstract void ProcessInput(T input);
    }
}
