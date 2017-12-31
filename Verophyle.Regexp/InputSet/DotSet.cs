// Verophyle.Regexp Copyright © Verophyle Informatics 2018

using System;
using System.Collections.Generic;

namespace Verophyle.Regexp.InputSet
{
    public class DotSet<T> : InputSet<T>
    {
        public override string ToString()
        {
            return string.Format("{0}: .", TransitionIndex);
        }

        public override bool Equals(object obj)
        {
            var other = obj as DotSet<T>;
            return other != null;
        }

        public override int GetHashCode()
        {
            return 0;
        }
    }
}
