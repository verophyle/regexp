// Verophyle.Regexp Copyright © Verophyle Informatics 2018

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Verophyle.Regexp
{
    public static class StringRegexpParser
    {
        public static Node.Node<char> ParseRegexp(IEnumerable<char> str, int start, out int next, IList<string> errors)
        {
            Node.Node<char> node = null;
            int cur = start;

            int nodePos = 0;
            var disj = ParseDisjunction(false, str, cur, out cur, ref nodePos, errors);
            if (disj != null)
            {
                var end = new Node.End<char>(ref nodePos);
                node = new Node.Seq<char>(disj, end);
            }

            next = node != null ? cur : start;
            return node;
        }

        static Node.Node<char> ParseDisjunction(bool parens, IEnumerable<char> str, int start, out int next, ref int nodePos, IList<string> errors)
        {
            Node.Node<char> node = null;
            int cur = start;
            if (parens)
            {
                char open = str.ElementAtOrDefault(cur);
                if (open != '(')
                {
                    errors.Add(string.Format("expected '(' at index {0}", cur));
                    goto done;
                }
                cur++;
            }

            Node.Node<char> disj = null;
            while (true)
            {
                var seq = ParseSequence(true, str, cur, out cur, ref nodePos, errors);
                if (seq != null)
                {
                    disj = disj == null ? seq : new Node.Disj<char>(disj, seq);
                }

                char bar = str.ElementAtOrDefault(cur);
                if (bar == '|')
                {
                    cur++;
                }
                else if (bar == ')')
                {
                    if (!parens)
                    {
                        errors.Add(string.Format("unexpected character ')' at index {0}", cur));
                        goto done;
                    }
                    cur++;
                    break;
                }
                else if (bar == default(char))
                {
                    if (parens)
                    {
                        errors.Add(string.Format("expected ')' at index {0}", cur));
                        goto done;
                    }
                    break;
                }
                else
                {
                    errors.Add(string.Format("unexpected character '{0}' at index {1}", bar, cur));
                    goto done;
                }
            }
            node = disj;

        done:
            next = node != null ? cur : start;
            return node;
        }

        static Node.Node<char> ParseSequence(bool allowNonChars, IEnumerable<char> str, int start, out int next, ref int nodePos, IList<string> errors)
        {
            Node.Node<char> node = null;
            int cur = start;

            Node.Node<char> seq = null;
            while (true)
            {
                Node.Node<char> elem = null;

                char ch = str.ElementAtOrDefault(cur);
                if (ch == '(')
                {
                    if (!allowNonChars)
                    {
                        errors.Add(string.Format("'(' is not allowed at index {0}", cur));
                        goto done;
                    }

                    elem = ParseDisjunction(true, str, cur, out cur, ref nodePos, errors);
                }
                else if (ch == '[')
                {
                    if (!allowNonChars)
                    {
                        errors.Add(string.Format("'[' is not allowed at index {0}", cur));
                        goto done;
                    }

                    elem = ParseCategory(str, cur, out cur, ref nodePos, errors);
                }
                else if (ch == '\\')
                {
                    elem = new Node.Leaf<char>(ParseEscaped(str, cur, out cur, errors), ref nodePos);
                }
                else if (ch == '.')
                {
                    elem = new Node.Dot<char>(new InputSet.DotSet<char>(), ref nodePos);
                    cur++;
                }
                else if (ch == ')' || ch == ']' || ch == '|' || ch == default(char))
                {
                    break;
                }
                else
                {
                    elem = new Node.Leaf<char>(new InputSet.CharSet(ch), ref nodePos);
                    cur++;
                }

                if (elem == null)
                    goto done;

                char suf = str.ElementAtOrDefault(cur);
                if (suf == '+')
                {
                    var star = new Node.Star<char>(elem);
                    elem = new Node.Seq<char>(elem, star);
                    cur++;
                }
                else if (suf == '*')
                {
                    elem = new Node.Star<char>(elem);
                    cur++;
                }
                else if (suf == '?')
                {
                    elem = new Node.Disj<char>(elem, new Node.Epsilon<char>(ref nodePos));
                    cur++;
                }

                seq = seq == null ? elem : new Node.Seq<char>(seq, elem);
            }
            node = seq;

        done:
            next = node != null ? cur : start;
            return node;
        }

        static Node.Node<char> ParseCategory(IEnumerable<char> str, int start, out int next, ref int nodePos, IList<string> errors)
        {
            Node.Node<char> node = null;
            int cur = start;

            char bra = str.ElementAtOrDefault(cur);
            if (bra == '[')
            {
                cur++;
            }
            else
            {
                errors.Add(string.Format("expected '[' at index {0}", cur));
                goto done;
            }

            bool negate = false;
            char hat = str.ElementAtOrDefault(cur);
            if (hat == '^')
            {
                negate = true;
                cur++;
            }

            var inputSets = new List<IInputSet<char>>();
            while (true)
            {
                var range = ParseCategoryRange(str, cur, out cur, errors);
                if (range != null)
                {
                    inputSets.Add(range);
                }
                else
                {
                    char ch = str.ElementAtOrDefault(cur);
                    if (ch == ']')
                    {
                        cur++;
                        break;
                    }
                    else if (ch == '\\')
                    {
                        inputSets.Add(ParseEscaped(str, cur, out cur, errors));
                    }
                    else if (ch == 0)
                    {
                        errors.Add(string.Format("unterminated [] set starting at {0}", start));
                        goto done;
                    }
                    else
                    {
                        cur++;
                        inputSets.Add(new InputSet.CharSet(ch));
                    }
                }
            }

            if (inputSets.Any())
            {
                var combinedInputSets = new InputSet.UnicodeCategorySet(inputSets);

                if (negate)
                    node = new Node.Fail<char>(combinedInputSets, ref nodePos);
                else
                    node = new Node.Leaf<char>(combinedInputSets, ref nodePos);
            }

        done:
            next = node != null ? cur : start;
            return node;
        }

        static IInputSet<char> ParseCategoryRange(IEnumerable<char> str, int start, out int next, IList<string> errors)
        {
            IInputSet<char> inputSet = null;
            int cur = start;
            char first = str.ElementAtOrDefault(cur);
            if (first == default(char) || first == '\\' || first == ']')
            {
                goto done;
            }

            cur++;
            char dash = str.ElementAtOrDefault(cur);
            if (dash != '-')
            {
                goto done;
            }

            cur++;
            char last = str.ElementAtOrDefault(cur);
            if (last == default(char) || last == '\\' || last == ']')
            {
                goto done;
            }

            cur++;
            inputSet = new InputSet.CharSet(Enumerable.Range(first, (last + 1) - first)
                .Select(n => (char)n));

        done:
            next = inputSet != null ? cur : start;
            return inputSet;
        }

        static IInputSet<char> ParseEscaped(IEnumerable<char> str, int start, out int next, IList<string> errors)
        {
            IInputSet<char> inputSet = null;
            int cur = start;

            var slash = str.ElementAtOrDefault(cur);
            if (slash != '\\')
            {
                errors.Add(string.Format("expected '\\' at index {0}", cur));
                goto done;
            }
            cur++;

            var ch = str.ElementAtOrDefault(cur);
            switch (ch)
            {
                case 's':
                case 'w':
                case 'd':
                    cur++;
                    inputSet = new InputSet.UnicodeCategorySet(new[] { ch });
                    break;
                case 'p':
                    cur++;
                    inputSet = ParseUnicodeCategory(str, cur, out cur, errors);
                    break;
                case 'u':
                case 'U':
                case 'x':
                case 'X':
                    cur++;
                    inputSet = ParseHexadecimal(str, cur, out cur, errors);
                    break;
                case 'r':
                    cur++;
                    inputSet = new InputSet.CharSet('\r');
                    break;
                case 'n':
                    cur++;
                    inputSet = new InputSet.CharSet('\n');
                    break;
                case 't':
                    cur++;
                    inputSet = new InputSet.CharSet('\t');
                    break;
                case default(char):
                    errors.Add(string.Format("expected escaped character at index {0}", cur));
                    goto done;
                default:
                    cur++;
                    inputSet = new InputSet.CharSet(ch);
                    break;
            }

        done:
            next = inputSet != null ? cur : start;
            return inputSet;
        }

        static IInputSet<char> ParseUnicodeCategory(IEnumerable<char> str, int start, out int next, IList<string> errors)
        {
            IInputSet<char> inputSet = null;
            int cur = start;

            var open = str.ElementAtOrDefault(cur);
            if (open == '{')
            {
                cur++;
            }
            else
            {
                errors.Add(string.Format("expected '{{' at index {0}", cur));
                goto done;
            }

            var sb = new StringBuilder();
            while (true)
            {
                char ch = str.ElementAtOrDefault(cur);
                if (ch == '}')
                {
                    cur++;
                    break;
                }
                else if (ch == default(char))
                {
                    if (cur > start + 1)
                        errors.Add(string.Format("expected '}}' at index {0}", cur));
                    else
                        errors.Add(string.Format("expected unicode category at index {0}", start));
                    goto done;
                }
                else
                {
                    sb.Append(ch);
                    cur++;
                }
            }
            string name = sb.ToString();
            try
            {
                inputSet = new InputSet.UnicodeCategorySet(name);
            }
            catch (Exception e)
            {
                errors.Add(string.Format("invalid unicode category name '{0}' at {1}: {2}", name, start, e.Message));
                goto done;
            }

        done:
            next = inputSet != null ? cur : start;
            return inputSet;
        }

        static IInputSet<char> ParseHexadecimal(IEnumerable<char> str, int start, out int next, IList<string> errors)
        {
            IInputSet<char> inputSet = null;
            int cur = start;

            var open = str.ElementAtOrDefault(cur);
            if (open != '{')
            {
                errors.Add(string.Format("expected '{{' at index {0}", cur));
                goto done;
            }
            cur++;

            var sb = new StringBuilder();
            while (true)
            {
                char ch = str.ElementAtOrDefault(cur);
                if (ch == '}')
                {
                    cur++;
                    break;
                }
                else if (ch == default(char))
                {
                    if (cur > start + 1)
                        errors.Add(string.Format("expected '}}' at index {0}", cur));
                    else
                        errors.Add(string.Format("expected hexadecimal at index {0}", start));
                    goto done;
                }
                else if ((ch >= '0' && ch <= '9') || (ch >= 'a' && ch <= 'f') || (ch >= 'A' && ch <= 'F'))
                {
                    sb.Append(ch);
                    cur++;
                }
            }
            string hex = sb.ToString();
            try
            {
                int code = Convert.ToInt32(hex, 16);
                if (code > char.MaxValue)
                {
                    errors.Add(string.Format("hexadecimal character value exceeds 0xffff at index {0}", start));
                    goto done;
                }
                inputSet = new InputSet.CharSet((char)code);
            }
            catch (Exception e)
            {
                errors.Add(string.Format("invalid hexadecimal number '{0}' at {1}: {2}", hex, start, e.Message));
                goto done;
            }

        done:
            next = inputSet != null ? cur : start;
            return inputSet;
        }
    }
}
