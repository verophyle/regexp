//
// IronMeta RegexpParser Parser; Generated 2015-07-25 14:37:42Z UTC
//

using System;
using System.Collections.Generic;
using System.Linq;

using IronMeta.Matcher;

#pragma warning disable 0219
#pragma warning disable 1591

namespace Verophyle.Regexp
{

    using _RegexpParser_Inputs = IEnumerable<char>;
    using _RegexpParser_Results = IEnumerable<Regexp<InputClass<char>>>;
    using _RegexpParser_Item = IronMeta.Matcher.MatchItem<char, Regexp<InputClass<char>>>;
    using _RegexpParser_Args = IEnumerable<IronMeta.Matcher.MatchItem<char, Regexp<InputClass<char>>>>;
    using _RegexpParser_Memo = IronMeta.Matcher.MatchState<char, Regexp<InputClass<char>>>;
    using _RegexpParser_Rule = System.Action<IronMeta.Matcher.MatchState<char, Regexp<InputClass<char>>>, int, IEnumerable<IronMeta.Matcher.MatchItem<char, Regexp<InputClass<char>>>>>;
    using _RegexpParser_Base = IronMeta.Matcher.Matcher<char, Regexp<InputClass<char>>>;

    partial class RegexpParser : IronMeta.Matcher.Matcher<char, Regexp<InputClass<char>>>
    {
        public RegexpParser()
            : base()
        {
            _setTerminals();
        }

        public RegexpParser(bool handle_left_recursion)
            : base(handle_left_recursion)
        {
            _setTerminals();
        }

        void _setTerminals()
        {
            this.Terminals = new HashSet<string>()
            {
                "Category",
                "Ch",
                "Class",
                "Dot",
                "EOF",
                "Escaped",
                "EscapedChar",
                "HexDigit",
                "MetaCategory",
                "Range",
                "Single",
                "SyntaxChar",
                "UniCategory",
                "UnicodeGeneralCategory",
            };
        }


        public void Regex(_RegexpParser_Memo _memo, int _index, _RegexpParser_Args _args)
        {

            _RegexpParser_Item e = null;

            // AND 1
            int _start_i1 = _index;

            // CALLORVAR OrElement
            _RegexpParser_Item _r3;

            _r3 = _MemoCall(_memo, "OrElement", _index, OrElement, null);

            if (_r3 != null) _index = _r3.NextIndex;

            // BIND e
            e = _memo.Results.Peek();

            // AND shortcut
            if (_memo.Results.Peek() == null) { _memo.Results.Push(null); goto label1; }

            // CALLORVAR EOF
            _RegexpParser_Item _r4;

            _r4 = _MemoCall(_memo, "EOF", _index, EOF, null);

            if (_r4 != null) _index = _r4.NextIndex;

        label1: // AND
            var _r1_2 = _memo.Results.Pop();
            var _r1_1 = _memo.Results.Pop();

            if (_r1_1 != null && _r1_2 != null)
            {
                _memo.Results.Push( new _RegexpParser_Item(_start_i1, _index, _memo.InputEnumerable, _r1_1.Results.Concat(_r1_2.Results).Where(_NON_NULL), true) );
            }
            else
            {
                _memo.Results.Push(null);
                _index = _start_i1;
            }

            // ACT
            var _r0 = _memo.Results.Peek();
            if (_r0 != null)
            {
                _memo.Results.Pop();
                _memo.Results.Push( new _RegexpParser_Item(_r0.StartIndex, _r0.NextIndex, _memo.InputEnumerable, _Thunk(_IM_Result => { return new Node.Cat<InputClass<char>>(e, new Node.End<InputClass<char>>(ref NEXT_POS)); }, _r0), true) );
            }

        }


        public void OrElement(_RegexpParser_Memo _memo, int _index, _RegexpParser_Args _args)
        {

            _RegexpParser_Item a = null;
            _RegexpParser_Item b = null;

            // OR 0
            int _start_i0 = _index;

            // AND 2
            int _start_i2 = _index;

            // AND 3
            int _start_i3 = _index;

            // CALLORVAR OrElement
            _RegexpParser_Item _r5;

            _r5 = _MemoCall(_memo, "OrElement", _index, OrElement, null);

            if (_r5 != null) _index = _r5.NextIndex;

            // BIND a
            a = _memo.Results.Peek();

            // AND shortcut
            if (_memo.Results.Peek() == null) { _memo.Results.Push(null); goto label3; }

            // LITERAL "|"
            _ParseLiteralString(_memo, ref _index, "|");

        label3: // AND
            var _r3_2 = _memo.Results.Pop();
            var _r3_1 = _memo.Results.Pop();

            if (_r3_1 != null && _r3_2 != null)
            {
                _memo.Results.Push( new _RegexpParser_Item(_start_i3, _index, _memo.InputEnumerable, _r3_1.Results.Concat(_r3_2.Results).Where(_NON_NULL), true) );
            }
            else
            {
                _memo.Results.Push(null);
                _index = _start_i3;
            }

            // AND shortcut
            if (_memo.Results.Peek() == null) { _memo.Results.Push(null); goto label2; }

            // CALLORVAR AndElement
            _RegexpParser_Item _r8;

            _r8 = _MemoCall(_memo, "AndElement", _index, AndElement, null);

            if (_r8 != null) _index = _r8.NextIndex;

            // BIND b
            b = _memo.Results.Peek();

        label2: // AND
            var _r2_2 = _memo.Results.Pop();
            var _r2_1 = _memo.Results.Pop();

            if (_r2_1 != null && _r2_2 != null)
            {
                _memo.Results.Push( new _RegexpParser_Item(_start_i2, _index, _memo.InputEnumerable, _r2_1.Results.Concat(_r2_2.Results).Where(_NON_NULL), true) );
            }
            else
            {
                _memo.Results.Push(null);
                _index = _start_i2;
            }

            // ACT
            var _r1 = _memo.Results.Peek();
            if (_r1 != null)
            {
                _memo.Results.Pop();
                _memo.Results.Push( new _RegexpParser_Item(_r1.StartIndex, _r1.NextIndex, _memo.InputEnumerable, _Thunk(_IM_Result => { return new Node.Or<InputClass<char>>(a, b); }, _r1), true) );
            }

            // OR shortcut
            if (_memo.Results.Peek() == null) { _memo.Results.Pop(); _index = _start_i0; } else goto label0;

            // CALLORVAR AndElement
            _RegexpParser_Item _r9;

            _r9 = _MemoCall(_memo, "AndElement", _index, AndElement, null);

            if (_r9 != null) _index = _r9.NextIndex;

        label0: // OR
            int _dummy_i0 = _index; // no-op for label

        }


        public void AndElement(_RegexpParser_Memo _memo, int _index, _RegexpParser_Args _args)
        {

            _RegexpParser_Item a = null;
            _RegexpParser_Item b = null;

            // OR 0
            int _start_i0 = _index;

            // AND 2
            int _start_i2 = _index;

            // CALLORVAR AndElement
            _RegexpParser_Item _r4;

            _r4 = _MemoCall(_memo, "AndElement", _index, AndElement, null);

            if (_r4 != null) _index = _r4.NextIndex;

            // BIND a
            a = _memo.Results.Peek();

            // AND shortcut
            if (_memo.Results.Peek() == null) { _memo.Results.Push(null); goto label2; }

            // CALLORVAR PostElement
            _RegexpParser_Item _r6;

            _r6 = _MemoCall(_memo, "PostElement", _index, PostElement, null);

            if (_r6 != null) _index = _r6.NextIndex;

            // BIND b
            b = _memo.Results.Peek();

        label2: // AND
            var _r2_2 = _memo.Results.Pop();
            var _r2_1 = _memo.Results.Pop();

            if (_r2_1 != null && _r2_2 != null)
            {
                _memo.Results.Push( new _RegexpParser_Item(_start_i2, _index, _memo.InputEnumerable, _r2_1.Results.Concat(_r2_2.Results).Where(_NON_NULL), true) );
            }
            else
            {
                _memo.Results.Push(null);
                _index = _start_i2;
            }

            // ACT
            var _r1 = _memo.Results.Peek();
            if (_r1 != null)
            {
                _memo.Results.Pop();
                _memo.Results.Push( new _RegexpParser_Item(_r1.StartIndex, _r1.NextIndex, _memo.InputEnumerable, _Thunk(_IM_Result => { return new Node.Cat<InputClass<char>>(a, b); }, _r1), true) );
            }

            // OR shortcut
            if (_memo.Results.Peek() == null) { _memo.Results.Pop(); _index = _start_i0; } else goto label0;

            // CALLORVAR PostElement
            _RegexpParser_Item _r7;

            _r7 = _MemoCall(_memo, "PostElement", _index, PostElement, null);

            if (_r7 != null) _index = _r7.NextIndex;

        label0: // OR
            int _dummy_i0 = _index; // no-op for label

        }


        public void PostElement(_RegexpParser_Memo _memo, int _index, _RegexpParser_Args _args)
        {

            _RegexpParser_Item a = null;

            // OR 0
            int _start_i0 = _index;

            // OR 1
            int _start_i1 = _index;

            // OR 2
            int _start_i2 = _index;

            // AND 4
            int _start_i4 = _index;

            // CALLORVAR Element
            _RegexpParser_Item _r6;

            _r6 = _MemoCall(_memo, "Element", _index, Element, null);

            if (_r6 != null) _index = _r6.NextIndex;

            // BIND a
            a = _memo.Results.Peek();

            // AND shortcut
            if (_memo.Results.Peek() == null) { _memo.Results.Push(null); goto label4; }

            // LITERAL '+'
            _ParseLiteralChar(_memo, ref _index, '+');

        label4: // AND
            var _r4_2 = _memo.Results.Pop();
            var _r4_1 = _memo.Results.Pop();

            if (_r4_1 != null && _r4_2 != null)
            {
                _memo.Results.Push( new _RegexpParser_Item(_start_i4, _index, _memo.InputEnumerable, _r4_1.Results.Concat(_r4_2.Results).Where(_NON_NULL), true) );
            }
            else
            {
                _memo.Results.Push(null);
                _index = _start_i4;
            }

            // ACT
            var _r3 = _memo.Results.Peek();
            if (_r3 != null)
            {
                _memo.Results.Pop();
                _memo.Results.Push( new _RegexpParser_Item(_r3.StartIndex, _r3.NextIndex, _memo.InputEnumerable, _Thunk(_IM_Result => { return new Node.Cat<InputClass<char>>(a, new Node.Star<InputClass<char>>(a)); }, _r3), true) );
            }

            // OR shortcut
            if (_memo.Results.Peek() == null) { _memo.Results.Pop(); _index = _start_i2; } else goto label2;

            // AND 9
            int _start_i9 = _index;

            // CALLORVAR Element
            _RegexpParser_Item _r11;

            _r11 = _MemoCall(_memo, "Element", _index, Element, null);

            if (_r11 != null) _index = _r11.NextIndex;

            // BIND a
            a = _memo.Results.Peek();

            // AND shortcut
            if (_memo.Results.Peek() == null) { _memo.Results.Push(null); goto label9; }

            // LITERAL '*'
            _ParseLiteralChar(_memo, ref _index, '*');

        label9: // AND
            var _r9_2 = _memo.Results.Pop();
            var _r9_1 = _memo.Results.Pop();

            if (_r9_1 != null && _r9_2 != null)
            {
                _memo.Results.Push( new _RegexpParser_Item(_start_i9, _index, _memo.InputEnumerable, _r9_1.Results.Concat(_r9_2.Results).Where(_NON_NULL), true) );
            }
            else
            {
                _memo.Results.Push(null);
                _index = _start_i9;
            }

            // ACT
            var _r8 = _memo.Results.Peek();
            if (_r8 != null)
            {
                _memo.Results.Pop();
                _memo.Results.Push( new _RegexpParser_Item(_r8.StartIndex, _r8.NextIndex, _memo.InputEnumerable, _Thunk(_IM_Result => { return new Node.Star<InputClass<char>>(a); }, _r8), true) );
            }

        label2: // OR
            int _dummy_i2 = _index; // no-op for label

            // OR shortcut
            if (_memo.Results.Peek() == null) { _memo.Results.Pop(); _index = _start_i1; } else goto label1;

            // AND 14
            int _start_i14 = _index;

            // CALLORVAR Element
            _RegexpParser_Item _r16;

            _r16 = _MemoCall(_memo, "Element", _index, Element, null);

            if (_r16 != null) _index = _r16.NextIndex;

            // BIND a
            a = _memo.Results.Peek();

            // AND shortcut
            if (_memo.Results.Peek() == null) { _memo.Results.Push(null); goto label14; }

            // LITERAL '?'
            _ParseLiteralChar(_memo, ref _index, '?');

        label14: // AND
            var _r14_2 = _memo.Results.Pop();
            var _r14_1 = _memo.Results.Pop();

            if (_r14_1 != null && _r14_2 != null)
            {
                _memo.Results.Push( new _RegexpParser_Item(_start_i14, _index, _memo.InputEnumerable, _r14_1.Results.Concat(_r14_2.Results).Where(_NON_NULL), true) );
            }
            else
            {
                _memo.Results.Push(null);
                _index = _start_i14;
            }

            // ACT
            var _r13 = _memo.Results.Peek();
            if (_r13 != null)
            {
                _memo.Results.Pop();
                _memo.Results.Push( new _RegexpParser_Item(_r13.StartIndex, _r13.NextIndex, _memo.InputEnumerable, _Thunk(_IM_Result => { return new Node.Or<InputClass<char>>(a, new Node.Epsilon<InputClass<char>>(ref NEXT_POS)); }, _r13), true) );
            }

        label1: // OR
            int _dummy_i1 = _index; // no-op for label

            // OR shortcut
            if (_memo.Results.Peek() == null) { _memo.Results.Pop(); _index = _start_i0; } else goto label0;

            // CALLORVAR Element
            _RegexpParser_Item _r18;

            _r18 = _MemoCall(_memo, "Element", _index, Element, null);

            if (_r18 != null) _index = _r18.NextIndex;

        label0: // OR
            int _dummy_i0 = _index; // no-op for label

        }


        public void Element(_RegexpParser_Memo _memo, int _index, _RegexpParser_Args _args)
        {

            // OR 0
            int _start_i0 = _index;

            // OR 1
            int _start_i1 = _index;

            // OR 2
            int _start_i2 = _index;

            // CALLORVAR Group
            _RegexpParser_Item _r3;

            _r3 = _MemoCall(_memo, "Group", _index, Group, null);

            if (_r3 != null) _index = _r3.NextIndex;

            // OR shortcut
            if (_memo.Results.Peek() == null) { _memo.Results.Pop(); _index = _start_i2; } else goto label2;

            // CALLORVAR Class
            _RegexpParser_Item _r4;

            _r4 = _MemoCall(_memo, "Class", _index, Class, null);

            if (_r4 != null) _index = _r4.NextIndex;

        label2: // OR
            int _dummy_i2 = _index; // no-op for label

            // OR shortcut
            if (_memo.Results.Peek() == null) { _memo.Results.Pop(); _index = _start_i1; } else goto label1;

            // CALLORVAR Dot
            _RegexpParser_Item _r5;

            _r5 = _MemoCall(_memo, "Dot", _index, Dot, null);

            if (_r5 != null) _index = _r5.NextIndex;

        label1: // OR
            int _dummy_i1 = _index; // no-op for label

            // OR shortcut
            if (_memo.Results.Peek() == null) { _memo.Results.Pop(); _index = _start_i0; } else goto label0;

            // CALLORVAR Single
            _RegexpParser_Item _r6;

            _r6 = _MemoCall(_memo, "Single", _index, Single, null);

            if (_r6 != null) _index = _r6.NextIndex;

        label0: // OR
            int _dummy_i0 = _index; // no-op for label

        }


        public void Group(_RegexpParser_Memo _memo, int _index, _RegexpParser_Args _args)
        {

            // AND 0
            int _start_i0 = _index;

            // AND 1
            int _start_i1 = _index;

            // LITERAL '('
            _ParseLiteralChar(_memo, ref _index, '(');

            // AND shortcut
            if (_memo.Results.Peek() == null) { _memo.Results.Push(null); goto label1; }

            // CALLORVAR OrElement
            _RegexpParser_Item _r3;

            _r3 = _MemoCall(_memo, "OrElement", _index, OrElement, null);

            if (_r3 != null) _index = _r3.NextIndex;

        label1: // AND
            var _r1_2 = _memo.Results.Pop();
            var _r1_1 = _memo.Results.Pop();

            if (_r1_1 != null && _r1_2 != null)
            {
                _memo.Results.Push( new _RegexpParser_Item(_start_i1, _index, _memo.InputEnumerable, _r1_1.Results.Concat(_r1_2.Results).Where(_NON_NULL), true) );
            }
            else
            {
                _memo.Results.Push(null);
                _index = _start_i1;
            }

            // AND shortcut
            if (_memo.Results.Peek() == null) { _memo.Results.Push(null); goto label0; }

            // LITERAL ')'
            _ParseLiteralChar(_memo, ref _index, ')');

        label0: // AND
            var _r0_2 = _memo.Results.Pop();
            var _r0_1 = _memo.Results.Pop();

            if (_r0_1 != null && _r0_2 != null)
            {
                _memo.Results.Push( new _RegexpParser_Item(_start_i0, _index, _memo.InputEnumerable, _r0_1.Results.Concat(_r0_2.Results).Where(_NON_NULL), true) );
            }
            else
            {
                _memo.Results.Push(null);
                _index = _start_i0;
            }

        }


        public void Class(_RegexpParser_Memo _memo, int _index, _RegexpParser_Args _args)
        {

            _RegexpParser_Item n = null;
            _RegexpParser_Item c = null;

            // AND 1
            int _start_i1 = _index;

            // AND 2
            int _start_i2 = _index;

            // AND 3
            int _start_i3 = _index;

            // LITERAL '['
            _ParseLiteralChar(_memo, ref _index, '[');

            // AND shortcut
            if (_memo.Results.Peek() == null) { _memo.Results.Push(null); goto label3; }

            // LITERAL '^'
            _ParseLiteralChar(_memo, ref _index, '^');

            // QUES
            if (_memo.Results.Peek() == null) { _memo.Results.Pop(); _memo.Results.Push(new _RegexpParser_Item(_index, _memo.InputEnumerable)); }

            // BIND n
            n = _memo.Results.Peek();

        label3: // AND
            var _r3_2 = _memo.Results.Pop();
            var _r3_1 = _memo.Results.Pop();

            if (_r3_1 != null && _r3_2 != null)
            {
                _memo.Results.Push( new _RegexpParser_Item(_start_i3, _index, _memo.InputEnumerable, _r3_1.Results.Concat(_r3_2.Results).Where(_NON_NULL), true) );
            }
            else
            {
                _memo.Results.Push(null);
                _index = _start_i3;
            }

            // AND shortcut
            if (_memo.Results.Peek() == null) { _memo.Results.Push(null); goto label2; }

            // PLUS 9
            int _start_i9 = _index;
            var _res9 = Enumerable.Empty<Regexp<InputClass<char>>>();
        label9:

            // AND 10
            int _start_i10 = _index;

            // NOT 11
            int _start_i11 = _index;

            // LITERAL ']'
            _ParseLiteralChar(_memo, ref _index, ']');

            // NOT 11
            var _r11 = _memo.Results.Pop();
            _memo.Results.Push( _r11 == null ? new _RegexpParser_Item(_start_i11, _memo.InputEnumerable) : null);
            _index = _start_i11;

            // AND shortcut
            if (_memo.Results.Peek() == null) { _memo.Results.Push(null); goto label10; }

            // OR 13
            int _start_i13 = _index;

            // CALLORVAR Range
            _RegexpParser_Item _r14;

            _r14 = _MemoCall(_memo, "Range", _index, Range, null);

            if (_r14 != null) _index = _r14.NextIndex;

            // OR shortcut
            if (_memo.Results.Peek() == null) { _memo.Results.Pop(); _index = _start_i13; } else goto label13;

            // CALLORVAR Single
            _RegexpParser_Item _r15;

            _r15 = _MemoCall(_memo, "Single", _index, Single, null);

            if (_r15 != null) _index = _r15.NextIndex;

        label13: // OR
            int _dummy_i13 = _index; // no-op for label

        label10: // AND
            var _r10_2 = _memo.Results.Pop();
            var _r10_1 = _memo.Results.Pop();

            if (_r10_1 != null && _r10_2 != null)
            {
                _memo.Results.Push( new _RegexpParser_Item(_start_i10, _index, _memo.InputEnumerable, _r10_1.Results.Concat(_r10_2.Results).Where(_NON_NULL), true) );
            }
            else
            {
                _memo.Results.Push(null);
                _index = _start_i10;
            }

            // PLUS 9
            var _r9 = _memo.Results.Pop();
            if (_r9 != null)
            {
                _res9 = _res9.Concat(_r9.Results);
                goto label9;
            }
            else
            {
                if (_index > _start_i9)
                    _memo.Results.Push(new _RegexpParser_Item(_start_i9, _index, _memo.InputEnumerable, _res9.Where(_NON_NULL), true));
                else
                    _memo.Results.Push(null);
            }

            // BIND c
            c = _memo.Results.Peek();

        label2: // AND
            var _r2_2 = _memo.Results.Pop();
            var _r2_1 = _memo.Results.Pop();

            if (_r2_1 != null && _r2_2 != null)
            {
                _memo.Results.Push( new _RegexpParser_Item(_start_i2, _index, _memo.InputEnumerable, _r2_1.Results.Concat(_r2_2.Results).Where(_NON_NULL), true) );
            }
            else
            {
                _memo.Results.Push(null);
                _index = _start_i2;
            }

            // AND shortcut
            if (_memo.Results.Peek() == null) { _memo.Results.Push(null); goto label1; }

            // LITERAL ']'
            _ParseLiteralChar(_memo, ref _index, ']');

        label1: // AND
            var _r1_2 = _memo.Results.Pop();
            var _r1_1 = _memo.Results.Pop();

            if (_r1_1 != null && _r1_2 != null)
            {
                _memo.Results.Push( new _RegexpParser_Item(_start_i1, _index, _memo.InputEnumerable, _r1_1.Results.Concat(_r1_2.Results).Where(_NON_NULL), true) );
            }
            else
            {
                _memo.Results.Push(null);
                _index = _start_i1;
            }

            // ACT
            var _r0 = _memo.Results.Peek();
            if (_r0 != null)
            {
                _memo.Results.Pop();
                _memo.Results.Push( new _RegexpParser_Item(_r0.StartIndex, _r0.NextIndex, _memo.InputEnumerable, _Thunk(_IM_Result => { var input_class = new CategoryClass(c.Results.Select(l => l.Input));
            if (n.Inputs.Any())
                return new Node.Fail<InputClass<char>>(input_class, ref NEXT_POS);
            else
                return new Node.Leaf<InputClass<char>>(input_class, ref NEXT_POS); }, _r0), true) );
            }

        }


        public void Dot(_RegexpParser_Memo _memo, int _index, _RegexpParser_Args _args)
        {

            // LITERAL '.'
            _ParseLiteralChar(_memo, ref _index, '.');

            // ACT
            var _r0 = _memo.Results.Peek();
            if (_r0 != null)
            {
                _memo.Results.Pop();
                _memo.Results.Push( new _RegexpParser_Item(_r0.StartIndex, _r0.NextIndex, _memo.InputEnumerable, _Thunk(_IM_Result => { return new Node.Dot<InputClass<char>>(ref NEXT_POS); }, _r0), true) );
            }

        }


        public void Range(_RegexpParser_Memo _memo, int _index, _RegexpParser_Args _args)
        {

            _RegexpParser_Item a = null;
            _RegexpParser_Item b = null;

            // AND 1
            int _start_i1 = _index;

            // AND 2
            int _start_i2 = _index;

            // CALLORVAR Single
            _RegexpParser_Item _r4;

            _r4 = _MemoCall(_memo, "Single", _index, Single, null);

            if (_r4 != null) _index = _r4.NextIndex;

            // BIND a
            a = _memo.Results.Peek();

            // AND shortcut
            if (_memo.Results.Peek() == null) { _memo.Results.Push(null); goto label2; }

            // LITERAL '-'
            _ParseLiteralChar(_memo, ref _index, '-');

        label2: // AND
            var _r2_2 = _memo.Results.Pop();
            var _r2_1 = _memo.Results.Pop();

            if (_r2_1 != null && _r2_2 != null)
            {
                _memo.Results.Push( new _RegexpParser_Item(_start_i2, _index, _memo.InputEnumerable, _r2_1.Results.Concat(_r2_2.Results).Where(_NON_NULL), true) );
            }
            else
            {
                _memo.Results.Push(null);
                _index = _start_i2;
            }

            // AND shortcut
            if (_memo.Results.Peek() == null) { _memo.Results.Push(null); goto label1; }

            // CALLORVAR Single
            _RegexpParser_Item _r7;

            _r7 = _MemoCall(_memo, "Single", _index, Single, null);

            if (_r7 != null) _index = _r7.NextIndex;

            // BIND b
            b = _memo.Results.Peek();

        label1: // AND
            var _r1_2 = _memo.Results.Pop();
            var _r1_1 = _memo.Results.Pop();

            if (_r1_1 != null && _r1_2 != null)
            {
                _memo.Results.Push( new _RegexpParser_Item(_start_i1, _index, _memo.InputEnumerable, _r1_1.Results.Concat(_r1_2.Results).Where(_NON_NULL), true) );
            }
            else
            {
                _memo.Results.Push(null);
                _index = _start_i1;
            }

            // ACT
            var _r0 = _memo.Results.Peek();
            if (_r0 != null)
            {
                _memo.Results.Pop();
                _memo.Results.Push( new _RegexpParser_Item(_r0.StartIndex, _r0.NextIndex, _memo.InputEnumerable, _Thunk(_IM_Result => { var start = a.Results.Single().Input.Inputs.Single();
            var end = b.Results.Single().Input.Inputs.Single();
            var inputs = new List<char>();
            for (char ch = start; ch <= end; ch++)
                inputs.Add(ch);
            var cc = new CharClass(inputs);
            return new Node.Leaf<InputClass<char>>(cc, ref NEXT_POS); }, _r0), true) );
            }

        }


        public void Single(_RegexpParser_Memo _memo, int _index, _RegexpParser_Args _args)
        {

            // OR 0
            int _start_i0 = _index;

            // OR 1
            int _start_i1 = _index;

            // CALLORVAR Category
            _RegexpParser_Item _r2;

            _r2 = _MemoCall(_memo, "Category", _index, Category, null);

            if (_r2 != null) _index = _r2.NextIndex;

            // OR shortcut
            if (_memo.Results.Peek() == null) { _memo.Results.Pop(); _index = _start_i1; } else goto label1;

            // CALLORVAR Escaped
            _RegexpParser_Item _r3;

            _r3 = _MemoCall(_memo, "Escaped", _index, Escaped, null);

            if (_r3 != null) _index = _r3.NextIndex;

        label1: // OR
            int _dummy_i1 = _index; // no-op for label

            // OR shortcut
            if (_memo.Results.Peek() == null) { _memo.Results.Pop(); _index = _start_i0; } else goto label0;

            // CALLORVAR Ch
            _RegexpParser_Item _r4;

            _r4 = _MemoCall(_memo, "Ch", _index, Ch, null);

            if (_r4 != null) _index = _r4.NextIndex;

        label0: // OR
            int _dummy_i0 = _index; // no-op for label

        }


        public void Category(_RegexpParser_Memo _memo, int _index, _RegexpParser_Args _args)
        {

            // OR 0
            int _start_i0 = _index;

            // CALLORVAR MetaCategory
            _RegexpParser_Item _r1;

            _r1 = _MemoCall(_memo, "MetaCategory", _index, MetaCategory, null);

            if (_r1 != null) _index = _r1.NextIndex;

            // OR shortcut
            if (_memo.Results.Peek() == null) { _memo.Results.Pop(); _index = _start_i0; } else goto label0;

            // CALLORVAR UniCategory
            _RegexpParser_Item _r2;

            _r2 = _MemoCall(_memo, "UniCategory", _index, UniCategory, null);

            if (_r2 != null) _index = _r2.NextIndex;

        label0: // OR
            int _dummy_i0 = _index; // no-op for label

        }


        public void MetaCategory(_RegexpParser_Memo _memo, int _index, _RegexpParser_Args _args)
        {

            _RegexpParser_Item cat = null;

            // AND 1
            int _start_i1 = _index;

            // LITERAL '\\'
            _ParseLiteralChar(_memo, ref _index, '\\');

            // AND shortcut
            if (_memo.Results.Peek() == null) { _memo.Results.Push(null); goto label1; }

            // OR 4
            int _start_i4 = _index;

            // OR 5
            int _start_i5 = _index;

            // LITERAL 's'
            _ParseLiteralChar(_memo, ref _index, 's');

            // OR shortcut
            if (_memo.Results.Peek() == null) { _memo.Results.Pop(); _index = _start_i5; } else goto label5;

            // LITERAL 'w'
            _ParseLiteralChar(_memo, ref _index, 'w');

        label5: // OR
            int _dummy_i5 = _index; // no-op for label

            // OR shortcut
            if (_memo.Results.Peek() == null) { _memo.Results.Pop(); _index = _start_i4; } else goto label4;

            // LITERAL 'd'
            _ParseLiteralChar(_memo, ref _index, 'd');

        label4: // OR
            int _dummy_i4 = _index; // no-op for label

            // BIND cat
            cat = _memo.Results.Peek();

        label1: // AND
            var _r1_2 = _memo.Results.Pop();
            var _r1_1 = _memo.Results.Pop();

            if (_r1_1 != null && _r1_2 != null)
            {
                _memo.Results.Push( new _RegexpParser_Item(_start_i1, _index, _memo.InputEnumerable, _r1_1.Results.Concat(_r1_2.Results).Where(_NON_NULL), true) );
            }
            else
            {
                _memo.Results.Push(null);
                _index = _start_i1;
            }

            // ACT
            var _r0 = _memo.Results.Peek();
            if (_r0 != null)
            {
                _memo.Results.Pop();
                _memo.Results.Push( new _RegexpParser_Item(_r0.StartIndex, _r0.NextIndex, _memo.InputEnumerable, _Thunk(_IM_Result => { return new Node.Leaf<InputClass<char>>(new CategoryClass(cat.Inputs), ref NEXT_POS); }, _r0), true) );
            }

        }


        public void UniCategory(_RegexpParser_Memo _memo, int _index, _RegexpParser_Args _args)
        {

            _RegexpParser_Item cat = null;

            // AND 1
            int _start_i1 = _index;

            // AND 2
            int _start_i2 = _index;

            // LITERAL "\\p{"
            _ParseLiteralString(_memo, ref _index, "\\p{");

            // AND shortcut
            if (_memo.Results.Peek() == null) { _memo.Results.Push(null); goto label2; }

            // CALLORVAR UnicodeGeneralCategory
            _RegexpParser_Item _r5;

            _r5 = _MemoCall(_memo, "UnicodeGeneralCategory", _index, UnicodeGeneralCategory, null);

            if (_r5 != null) _index = _r5.NextIndex;

            // BIND cat
            cat = _memo.Results.Peek();

        label2: // AND
            var _r2_2 = _memo.Results.Pop();
            var _r2_1 = _memo.Results.Pop();

            if (_r2_1 != null && _r2_2 != null)
            {
                _memo.Results.Push( new _RegexpParser_Item(_start_i2, _index, _memo.InputEnumerable, _r2_1.Results.Concat(_r2_2.Results).Where(_NON_NULL), true) );
            }
            else
            {
                _memo.Results.Push(null);
                _index = _start_i2;
            }

            // AND shortcut
            if (_memo.Results.Peek() == null) { _memo.Results.Push(null); goto label1; }

            // LITERAL '}'
            _ParseLiteralChar(_memo, ref _index, '}');

        label1: // AND
            var _r1_2 = _memo.Results.Pop();
            var _r1_1 = _memo.Results.Pop();

            if (_r1_1 != null && _r1_2 != null)
            {
                _memo.Results.Push( new _RegexpParser_Item(_start_i1, _index, _memo.InputEnumerable, _r1_1.Results.Concat(_r1_2.Results).Where(_NON_NULL), true) );
            }
            else
            {
                _memo.Results.Push(null);
                _index = _start_i1;
            }

            // ACT
            var _r0 = _memo.Results.Peek();
            if (_r0 != null)
            {
                _memo.Results.Pop();
                _memo.Results.Push( new _RegexpParser_Item(_r0.StartIndex, _r0.NextIndex, _memo.InputEnumerable, _Thunk(_IM_Result => { return new Node.Leaf<InputClass<char>>(new CategoryClass(cat.Inputs), ref NEXT_POS); }, _r0), true) );
            }

        }


        public void Escaped(_RegexpParser_Memo _memo, int _index, _RegexpParser_Args _args)
        {

            _RegexpParser_Item hex = null;
            _RegexpParser_Item c = null;

            // OR 0
            int _start_i0 = _index;

            // OR 1
            int _start_i1 = _index;

            // AND 3
            int _start_i3 = _index;

            // LITERAL '\\'
            _ParseLiteralChar(_memo, ref _index, '\\');

            // AND shortcut
            if (_memo.Results.Peek() == null) { _memo.Results.Push(null); goto label3; }

            // CALLORVAR EscapedChar
            _RegexpParser_Item _r5;

            _r5 = _MemoCall(_memo, "EscapedChar", _index, EscapedChar, null);

            if (_r5 != null) _index = _r5.NextIndex;

        label3: // AND
            var _r3_2 = _memo.Results.Pop();
            var _r3_1 = _memo.Results.Pop();

            if (_r3_1 != null && _r3_2 != null)
            {
                _memo.Results.Push( new _RegexpParser_Item(_start_i3, _index, _memo.InputEnumerable, _r3_1.Results.Concat(_r3_2.Results).Where(_NON_NULL), true) );
            }
            else
            {
                _memo.Results.Push(null);
                _index = _start_i3;
            }

            // ACT
            var _r2 = _memo.Results.Peek();
            if (_r2 != null)
            {
                _memo.Results.Pop();
                _memo.Results.Push( new _RegexpParser_Item(_r2.StartIndex, _r2.NextIndex, _memo.InputEnumerable, _Thunk(_IM_Result => { return new Node.Leaf<InputClass<char>>(new CharClass(Unescape(_IM_Result.Inputs)), ref NEXT_POS); }, _r2), true) );
            }

            // OR shortcut
            if (_memo.Results.Peek() == null) { _memo.Results.Pop(); _index = _start_i1; } else goto label1;

            // OR 7
            int _start_i7 = _index;

            // OR 8
            int _start_i8 = _index;

            // AND 9
            int _start_i9 = _index;

            // LITERAL "\\u"
            _ParseLiteralString(_memo, ref _index, "\\u");

            // AND shortcut
            if (_memo.Results.Peek() == null) { _memo.Results.Push(null); goto label9; }

            // AND 12
            int _start_i12 = _index;

            // AND 13
            int _start_i13 = _index;

            // AND 14
            int _start_i14 = _index;

            // CALLORVAR HexDigit
            _RegexpParser_Item _r15;

            _r15 = _MemoCall(_memo, "HexDigit", _index, HexDigit, null);

            if (_r15 != null) _index = _r15.NextIndex;

            // AND shortcut
            if (_memo.Results.Peek() == null) { _memo.Results.Push(null); goto label14; }

            // CALLORVAR HexDigit
            _RegexpParser_Item _r16;

            _r16 = _MemoCall(_memo, "HexDigit", _index, HexDigit, null);

            if (_r16 != null) _index = _r16.NextIndex;

        label14: // AND
            var _r14_2 = _memo.Results.Pop();
            var _r14_1 = _memo.Results.Pop();

            if (_r14_1 != null && _r14_2 != null)
            {
                _memo.Results.Push( new _RegexpParser_Item(_start_i14, _index, _memo.InputEnumerable, _r14_1.Results.Concat(_r14_2.Results).Where(_NON_NULL), true) );
            }
            else
            {
                _memo.Results.Push(null);
                _index = _start_i14;
            }

            // AND shortcut
            if (_memo.Results.Peek() == null) { _memo.Results.Push(null); goto label13; }

            // CALLORVAR HexDigit
            _RegexpParser_Item _r17;

            _r17 = _MemoCall(_memo, "HexDigit", _index, HexDigit, null);

            if (_r17 != null) _index = _r17.NextIndex;

        label13: // AND
            var _r13_2 = _memo.Results.Pop();
            var _r13_1 = _memo.Results.Pop();

            if (_r13_1 != null && _r13_2 != null)
            {
                _memo.Results.Push( new _RegexpParser_Item(_start_i13, _index, _memo.InputEnumerable, _r13_1.Results.Concat(_r13_2.Results).Where(_NON_NULL), true) );
            }
            else
            {
                _memo.Results.Push(null);
                _index = _start_i13;
            }

            // AND shortcut
            if (_memo.Results.Peek() == null) { _memo.Results.Push(null); goto label12; }

            // CALLORVAR HexDigit
            _RegexpParser_Item _r18;

            _r18 = _MemoCall(_memo, "HexDigit", _index, HexDigit, null);

            if (_r18 != null) _index = _r18.NextIndex;

        label12: // AND
            var _r12_2 = _memo.Results.Pop();
            var _r12_1 = _memo.Results.Pop();

            if (_r12_1 != null && _r12_2 != null)
            {
                _memo.Results.Push( new _RegexpParser_Item(_start_i12, _index, _memo.InputEnumerable, _r12_1.Results.Concat(_r12_2.Results).Where(_NON_NULL), true) );
            }
            else
            {
                _memo.Results.Push(null);
                _index = _start_i12;
            }

            // BIND hex
            hex = _memo.Results.Peek();

        label9: // AND
            var _r9_2 = _memo.Results.Pop();
            var _r9_1 = _memo.Results.Pop();

            if (_r9_1 != null && _r9_2 != null)
            {
                _memo.Results.Push( new _RegexpParser_Item(_start_i9, _index, _memo.InputEnumerable, _r9_1.Results.Concat(_r9_2.Results).Where(_NON_NULL), true) );
            }
            else
            {
                _memo.Results.Push(null);
                _index = _start_i9;
            }

            // OR shortcut
            if (_memo.Results.Peek() == null) { _memo.Results.Pop(); _index = _start_i8; } else goto label8;

            // AND 19
            int _start_i19 = _index;

            // LITERAL "\\x"
            _ParseLiteralString(_memo, ref _index, "\\x");

            // AND shortcut
            if (_memo.Results.Peek() == null) { _memo.Results.Push(null); goto label19; }

            // AND 22
            int _start_i22 = _index;

            // AND 23
            int _start_i23 = _index;

            // AND 24
            int _start_i24 = _index;

            // CALLORVAR HexDigit
            _RegexpParser_Item _r25;

            _r25 = _MemoCall(_memo, "HexDigit", _index, HexDigit, null);

            if (_r25 != null) _index = _r25.NextIndex;

            // AND shortcut
            if (_memo.Results.Peek() == null) { _memo.Results.Push(null); goto label24; }

            // CALLORVAR HexDigit
            _RegexpParser_Item _r27;

            _r27 = _MemoCall(_memo, "HexDigit", _index, HexDigit, null);

            if (_r27 != null) _index = _r27.NextIndex;

            // QUES
            if (_memo.Results.Peek() == null) { _memo.Results.Pop(); _memo.Results.Push(new _RegexpParser_Item(_index, _memo.InputEnumerable)); }

        label24: // AND
            var _r24_2 = _memo.Results.Pop();
            var _r24_1 = _memo.Results.Pop();

            if (_r24_1 != null && _r24_2 != null)
            {
                _memo.Results.Push( new _RegexpParser_Item(_start_i24, _index, _memo.InputEnumerable, _r24_1.Results.Concat(_r24_2.Results).Where(_NON_NULL), true) );
            }
            else
            {
                _memo.Results.Push(null);
                _index = _start_i24;
            }

            // AND shortcut
            if (_memo.Results.Peek() == null) { _memo.Results.Push(null); goto label23; }

            // CALLORVAR HexDigit
            _RegexpParser_Item _r29;

            _r29 = _MemoCall(_memo, "HexDigit", _index, HexDigit, null);

            if (_r29 != null) _index = _r29.NextIndex;

            // QUES
            if (_memo.Results.Peek() == null) { _memo.Results.Pop(); _memo.Results.Push(new _RegexpParser_Item(_index, _memo.InputEnumerable)); }

        label23: // AND
            var _r23_2 = _memo.Results.Pop();
            var _r23_1 = _memo.Results.Pop();

            if (_r23_1 != null && _r23_2 != null)
            {
                _memo.Results.Push( new _RegexpParser_Item(_start_i23, _index, _memo.InputEnumerable, _r23_1.Results.Concat(_r23_2.Results).Where(_NON_NULL), true) );
            }
            else
            {
                _memo.Results.Push(null);
                _index = _start_i23;
            }

            // AND shortcut
            if (_memo.Results.Peek() == null) { _memo.Results.Push(null); goto label22; }

            // CALLORVAR HexDigit
            _RegexpParser_Item _r31;

            _r31 = _MemoCall(_memo, "HexDigit", _index, HexDigit, null);

            if (_r31 != null) _index = _r31.NextIndex;

            // QUES
            if (_memo.Results.Peek() == null) { _memo.Results.Pop(); _memo.Results.Push(new _RegexpParser_Item(_index, _memo.InputEnumerable)); }

        label22: // AND
            var _r22_2 = _memo.Results.Pop();
            var _r22_1 = _memo.Results.Pop();

            if (_r22_1 != null && _r22_2 != null)
            {
                _memo.Results.Push( new _RegexpParser_Item(_start_i22, _index, _memo.InputEnumerable, _r22_1.Results.Concat(_r22_2.Results).Where(_NON_NULL), true) );
            }
            else
            {
                _memo.Results.Push(null);
                _index = _start_i22;
            }

            // BIND hex
            hex = _memo.Results.Peek();

        label19: // AND
            var _r19_2 = _memo.Results.Pop();
            var _r19_1 = _memo.Results.Pop();

            if (_r19_1 != null && _r19_2 != null)
            {
                _memo.Results.Push( new _RegexpParser_Item(_start_i19, _index, _memo.InputEnumerable, _r19_1.Results.Concat(_r19_2.Results).Where(_NON_NULL), true) );
            }
            else
            {
                _memo.Results.Push(null);
                _index = _start_i19;
            }

        label8: // OR
            int _dummy_i8 = _index; // no-op for label

            // OR shortcut
            if (_memo.Results.Peek() == null) { _memo.Results.Pop(); _index = _start_i7; } else goto label7;

            // AND 32
            int _start_i32 = _index;

            // LITERAL "\\U"
            _ParseLiteralString(_memo, ref _index, "\\U");

            // AND shortcut
            if (_memo.Results.Peek() == null) { _memo.Results.Push(null); goto label32; }

            // AND 35
            int _start_i35 = _index;

            // AND 36
            int _start_i36 = _index;

            // AND 37
            int _start_i37 = _index;

            // AND 38
            int _start_i38 = _index;

            // AND 39
            int _start_i39 = _index;

            // AND 40
            int _start_i40 = _index;

            // AND 41
            int _start_i41 = _index;

            // CALLORVAR HexDigit
            _RegexpParser_Item _r42;

            _r42 = _MemoCall(_memo, "HexDigit", _index, HexDigit, null);

            if (_r42 != null) _index = _r42.NextIndex;

            // AND shortcut
            if (_memo.Results.Peek() == null) { _memo.Results.Push(null); goto label41; }

            // CALLORVAR HexDigit
            _RegexpParser_Item _r43;

            _r43 = _MemoCall(_memo, "HexDigit", _index, HexDigit, null);

            if (_r43 != null) _index = _r43.NextIndex;

        label41: // AND
            var _r41_2 = _memo.Results.Pop();
            var _r41_1 = _memo.Results.Pop();

            if (_r41_1 != null && _r41_2 != null)
            {
                _memo.Results.Push( new _RegexpParser_Item(_start_i41, _index, _memo.InputEnumerable, _r41_1.Results.Concat(_r41_2.Results).Where(_NON_NULL), true) );
            }
            else
            {
                _memo.Results.Push(null);
                _index = _start_i41;
            }

            // AND shortcut
            if (_memo.Results.Peek() == null) { _memo.Results.Push(null); goto label40; }

            // CALLORVAR HexDigit
            _RegexpParser_Item _r44;

            _r44 = _MemoCall(_memo, "HexDigit", _index, HexDigit, null);

            if (_r44 != null) _index = _r44.NextIndex;

        label40: // AND
            var _r40_2 = _memo.Results.Pop();
            var _r40_1 = _memo.Results.Pop();

            if (_r40_1 != null && _r40_2 != null)
            {
                _memo.Results.Push( new _RegexpParser_Item(_start_i40, _index, _memo.InputEnumerable, _r40_1.Results.Concat(_r40_2.Results).Where(_NON_NULL), true) );
            }
            else
            {
                _memo.Results.Push(null);
                _index = _start_i40;
            }

            // AND shortcut
            if (_memo.Results.Peek() == null) { _memo.Results.Push(null); goto label39; }

            // CALLORVAR HexDigit
            _RegexpParser_Item _r45;

            _r45 = _MemoCall(_memo, "HexDigit", _index, HexDigit, null);

            if (_r45 != null) _index = _r45.NextIndex;

        label39: // AND
            var _r39_2 = _memo.Results.Pop();
            var _r39_1 = _memo.Results.Pop();

            if (_r39_1 != null && _r39_2 != null)
            {
                _memo.Results.Push( new _RegexpParser_Item(_start_i39, _index, _memo.InputEnumerable, _r39_1.Results.Concat(_r39_2.Results).Where(_NON_NULL), true) );
            }
            else
            {
                _memo.Results.Push(null);
                _index = _start_i39;
            }

            // AND shortcut
            if (_memo.Results.Peek() == null) { _memo.Results.Push(null); goto label38; }

            // CALLORVAR HexDigit
            _RegexpParser_Item _r46;

            _r46 = _MemoCall(_memo, "HexDigit", _index, HexDigit, null);

            if (_r46 != null) _index = _r46.NextIndex;

        label38: // AND
            var _r38_2 = _memo.Results.Pop();
            var _r38_1 = _memo.Results.Pop();

            if (_r38_1 != null && _r38_2 != null)
            {
                _memo.Results.Push( new _RegexpParser_Item(_start_i38, _index, _memo.InputEnumerable, _r38_1.Results.Concat(_r38_2.Results).Where(_NON_NULL), true) );
            }
            else
            {
                _memo.Results.Push(null);
                _index = _start_i38;
            }

            // AND shortcut
            if (_memo.Results.Peek() == null) { _memo.Results.Push(null); goto label37; }

            // CALLORVAR HexDigit
            _RegexpParser_Item _r47;

            _r47 = _MemoCall(_memo, "HexDigit", _index, HexDigit, null);

            if (_r47 != null) _index = _r47.NextIndex;

        label37: // AND
            var _r37_2 = _memo.Results.Pop();
            var _r37_1 = _memo.Results.Pop();

            if (_r37_1 != null && _r37_2 != null)
            {
                _memo.Results.Push( new _RegexpParser_Item(_start_i37, _index, _memo.InputEnumerable, _r37_1.Results.Concat(_r37_2.Results).Where(_NON_NULL), true) );
            }
            else
            {
                _memo.Results.Push(null);
                _index = _start_i37;
            }

            // AND shortcut
            if (_memo.Results.Peek() == null) { _memo.Results.Push(null); goto label36; }

            // CALLORVAR HexDigit
            _RegexpParser_Item _r48;

            _r48 = _MemoCall(_memo, "HexDigit", _index, HexDigit, null);

            if (_r48 != null) _index = _r48.NextIndex;

        label36: // AND
            var _r36_2 = _memo.Results.Pop();
            var _r36_1 = _memo.Results.Pop();

            if (_r36_1 != null && _r36_2 != null)
            {
                _memo.Results.Push( new _RegexpParser_Item(_start_i36, _index, _memo.InputEnumerable, _r36_1.Results.Concat(_r36_2.Results).Where(_NON_NULL), true) );
            }
            else
            {
                _memo.Results.Push(null);
                _index = _start_i36;
            }

            // AND shortcut
            if (_memo.Results.Peek() == null) { _memo.Results.Push(null); goto label35; }

            // CALLORVAR HexDigit
            _RegexpParser_Item _r49;

            _r49 = _MemoCall(_memo, "HexDigit", _index, HexDigit, null);

            if (_r49 != null) _index = _r49.NextIndex;

        label35: // AND
            var _r35_2 = _memo.Results.Pop();
            var _r35_1 = _memo.Results.Pop();

            if (_r35_1 != null && _r35_2 != null)
            {
                _memo.Results.Push( new _RegexpParser_Item(_start_i35, _index, _memo.InputEnumerable, _r35_1.Results.Concat(_r35_2.Results).Where(_NON_NULL), true) );
            }
            else
            {
                _memo.Results.Push(null);
                _index = _start_i35;
            }

            // BIND hex
            hex = _memo.Results.Peek();

        label32: // AND
            var _r32_2 = _memo.Results.Pop();
            var _r32_1 = _memo.Results.Pop();

            if (_r32_1 != null && _r32_2 != null)
            {
                _memo.Results.Push( new _RegexpParser_Item(_start_i32, _index, _memo.InputEnumerable, _r32_1.Results.Concat(_r32_2.Results).Where(_NON_NULL), true) );
            }
            else
            {
                _memo.Results.Push(null);
                _index = _start_i32;
            }

        label7: // OR
            int _dummy_i7 = _index; // no-op for label

            // ACT
            var _r6 = _memo.Results.Peek();
            if (_r6 != null)
            {
                _memo.Results.Pop();
                _memo.Results.Push( new _RegexpParser_Item(_r6.StartIndex, _r6.NextIndex, _memo.InputEnumerable, _Thunk(_IM_Result => { return new Node.Leaf<InputClass<char>>(new CharClass(UnHex(hex.Inputs)), ref NEXT_POS); }, _r6), true) );
            }

        label1: // OR
            int _dummy_i1 = _index; // no-op for label

            // OR shortcut
            if (_memo.Results.Peek() == null) { _memo.Results.Pop(); _index = _start_i0; } else goto label0;

            // AND 51
            int _start_i51 = _index;

            // LITERAL '\\'
            _ParseLiteralChar(_memo, ref _index, '\\');

            // AND shortcut
            if (_memo.Results.Peek() == null) { _memo.Results.Push(null); goto label51; }

            // ANY
            _ParseAny(_memo, ref _index);

            // BIND c
            c = _memo.Results.Peek();

        label51: // AND
            var _r51_2 = _memo.Results.Pop();
            var _r51_1 = _memo.Results.Pop();

            if (_r51_1 != null && _r51_2 != null)
            {
                _memo.Results.Push( new _RegexpParser_Item(_start_i51, _index, _memo.InputEnumerable, _r51_1.Results.Concat(_r51_2.Results).Where(_NON_NULL), true) );
            }
            else
            {
                _memo.Results.Push(null);
                _index = _start_i51;
            }

            // ACT
            var _r50 = _memo.Results.Peek();
            if (_r50 != null)
            {
                _memo.Results.Pop();
                _memo.Results.Push( new _RegexpParser_Item(_r50.StartIndex, _r50.NextIndex, _memo.InputEnumerable, _Thunk(_IM_Result => { return new Node.Leaf<InputClass<char>>(new CharClass(c.Inputs.First()), ref NEXT_POS); }, _r50), true) );
            }

        label0: // OR
            int _dummy_i0 = _index; // no-op for label

        }


        public void Ch(_RegexpParser_Memo _memo, int _index, _RegexpParser_Args _args)
        {

            _RegexpParser_Item c = null;

            // AND 1
            int _start_i1 = _index;

            // NOT 2
            int _start_i2 = _index;

            // CALLORVAR SyntaxChar
            _RegexpParser_Item _r3;

            _r3 = _MemoCall(_memo, "SyntaxChar", _index, SyntaxChar, null);

            if (_r3 != null) _index = _r3.NextIndex;

            // NOT 2
            var _r2 = _memo.Results.Pop();
            _memo.Results.Push( _r2 == null ? new _RegexpParser_Item(_start_i2, _memo.InputEnumerable) : null);
            _index = _start_i2;

            // AND shortcut
            if (_memo.Results.Peek() == null) { _memo.Results.Push(null); goto label1; }

            // ANY
            _ParseAny(_memo, ref _index);

            // BIND c
            c = _memo.Results.Peek();

        label1: // AND
            var _r1_2 = _memo.Results.Pop();
            var _r1_1 = _memo.Results.Pop();

            if (_r1_1 != null && _r1_2 != null)
            {
                _memo.Results.Push( new _RegexpParser_Item(_start_i1, _index, _memo.InputEnumerable, _r1_1.Results.Concat(_r1_2.Results).Where(_NON_NULL), true) );
            }
            else
            {
                _memo.Results.Push(null);
                _index = _start_i1;
            }

            // ACT
            var _r0 = _memo.Results.Peek();
            if (_r0 != null)
            {
                _memo.Results.Pop();
                _memo.Results.Push( new _RegexpParser_Item(_r0.StartIndex, _r0.NextIndex, _memo.InputEnumerable, _Thunk(_IM_Result => { return new Node.Leaf<InputClass<char>>(new CharClass(c.Inputs.First()), ref NEXT_POS); }, _r0), true) );
            }

        }


        public void EscapedChar(_RegexpParser_Memo _memo, int _index, _RegexpParser_Args _args)
        {

            // INPUT CLASS
            _ParseInputClass(_memo, ref _index, '\'', '"', '\\', ']', '+', '*', '0', 'a', 'b', 'f', 'n', 'r', 't', 'v');

        }


        public void SyntaxChar(_RegexpParser_Memo _memo, int _index, _RegexpParser_Args _args)
        {

            // INPUT CLASS
            _ParseInputClass(_memo, ref _index, '|', '(', ')', '[', ']', '\\', '+', '*');

        }


        public void HexDigit(_RegexpParser_Memo _memo, int _index, _RegexpParser_Args _args)
        {

            // INPUT CLASS
            _ParseInputClass(_memo, ref _index, '\u0030', '\u0031', '\u0032', '\u0033', '\u0034', '\u0035', '\u0036', '\u0037', '\u0038', '\u0039', '\u0061', '\u0062', '\u0063', '\u0064', '\u0065', '\u0066', '\u0041', '\u0042', '\u0043', '\u0044', '\u0045', '\u0046');

        }


        public void UnicodeGeneralCategory(_RegexpParser_Memo _memo, int _index, _RegexpParser_Args _args)
        {

            // OR 0
            int _start_i0 = _index;

            // OR 1
            int _start_i1 = _index;

            // OR 2
            int _start_i2 = _index;

            // OR 3
            int _start_i3 = _index;

            // OR 4
            int _start_i4 = _index;

            // OR 5
            int _start_i5 = _index;

            // OR 6
            int _start_i6 = _index;

            // OR 7
            int _start_i7 = _index;

            // OR 8
            int _start_i8 = _index;

            // OR 9
            int _start_i9 = _index;

            // OR 10
            int _start_i10 = _index;

            // LITERAL "Lu"
            _ParseLiteralString(_memo, ref _index, "Lu");

            // OR shortcut
            if (_memo.Results.Peek() == null) { _memo.Results.Pop(); _index = _start_i10; } else goto label10;

            // LITERAL "Ll"
            _ParseLiteralString(_memo, ref _index, "Ll");

        label10: // OR
            int _dummy_i10 = _index; // no-op for label

            // OR shortcut
            if (_memo.Results.Peek() == null) { _memo.Results.Pop(); _index = _start_i9; } else goto label9;

            // LITERAL "Lt"
            _ParseLiteralString(_memo, ref _index, "Lt");

        label9: // OR
            int _dummy_i9 = _index; // no-op for label

            // OR shortcut
            if (_memo.Results.Peek() == null) { _memo.Results.Pop(); _index = _start_i8; } else goto label8;

            // LITERAL "Lm"
            _ParseLiteralString(_memo, ref _index, "Lm");

        label8: // OR
            int _dummy_i8 = _index; // no-op for label

            // OR shortcut
            if (_memo.Results.Peek() == null) { _memo.Results.Pop(); _index = _start_i7; } else goto label7;

            // LITERAL "Lo"
            _ParseLiteralString(_memo, ref _index, "Lo");

        label7: // OR
            int _dummy_i7 = _index; // no-op for label

            // OR shortcut
            if (_memo.Results.Peek() == null) { _memo.Results.Pop(); _index = _start_i6; } else goto label6;

            // OR 16
            int _start_i16 = _index;

            // OR 17
            int _start_i17 = _index;

            // LITERAL "Mn"
            _ParseLiteralString(_memo, ref _index, "Mn");

            // OR shortcut
            if (_memo.Results.Peek() == null) { _memo.Results.Pop(); _index = _start_i17; } else goto label17;

            // LITERAL "Mc"
            _ParseLiteralString(_memo, ref _index, "Mc");

        label17: // OR
            int _dummy_i17 = _index; // no-op for label

            // OR shortcut
            if (_memo.Results.Peek() == null) { _memo.Results.Pop(); _index = _start_i16; } else goto label16;

            // LITERAL "Me"
            _ParseLiteralString(_memo, ref _index, "Me");

        label16: // OR
            int _dummy_i16 = _index; // no-op for label

        label6: // OR
            int _dummy_i6 = _index; // no-op for label

            // OR shortcut
            if (_memo.Results.Peek() == null) { _memo.Results.Pop(); _index = _start_i5; } else goto label5;

            // OR 21
            int _start_i21 = _index;

            // OR 22
            int _start_i22 = _index;

            // LITERAL "Nd"
            _ParseLiteralString(_memo, ref _index, "Nd");

            // OR shortcut
            if (_memo.Results.Peek() == null) { _memo.Results.Pop(); _index = _start_i22; } else goto label22;

            // LITERAL "Nl"
            _ParseLiteralString(_memo, ref _index, "Nl");

        label22: // OR
            int _dummy_i22 = _index; // no-op for label

            // OR shortcut
            if (_memo.Results.Peek() == null) { _memo.Results.Pop(); _index = _start_i21; } else goto label21;

            // LITERAL "No"
            _ParseLiteralString(_memo, ref _index, "No");

        label21: // OR
            int _dummy_i21 = _index; // no-op for label

        label5: // OR
            int _dummy_i5 = _index; // no-op for label

            // OR shortcut
            if (_memo.Results.Peek() == null) { _memo.Results.Pop(); _index = _start_i4; } else goto label4;

            // OR 26
            int _start_i26 = _index;

            // OR 27
            int _start_i27 = _index;

            // LITERAL "Zs"
            _ParseLiteralString(_memo, ref _index, "Zs");

            // OR shortcut
            if (_memo.Results.Peek() == null) { _memo.Results.Pop(); _index = _start_i27; } else goto label27;

            // LITERAL "Zl"
            _ParseLiteralString(_memo, ref _index, "Zl");

        label27: // OR
            int _dummy_i27 = _index; // no-op for label

            // OR shortcut
            if (_memo.Results.Peek() == null) { _memo.Results.Pop(); _index = _start_i26; } else goto label26;

            // LITERAL "Zp"
            _ParseLiteralString(_memo, ref _index, "Zp");

        label26: // OR
            int _dummy_i26 = _index; // no-op for label

        label4: // OR
            int _dummy_i4 = _index; // no-op for label

            // OR shortcut
            if (_memo.Results.Peek() == null) { _memo.Results.Pop(); _index = _start_i3; } else goto label3;

            // OR 31
            int _start_i31 = _index;

            // OR 32
            int _start_i32 = _index;

            // OR 33
            int _start_i33 = _index;

            // LITERAL "Cc"
            _ParseLiteralString(_memo, ref _index, "Cc");

            // OR shortcut
            if (_memo.Results.Peek() == null) { _memo.Results.Pop(); _index = _start_i33; } else goto label33;

            // LITERAL "Cf"
            _ParseLiteralString(_memo, ref _index, "Cf");

        label33: // OR
            int _dummy_i33 = _index; // no-op for label

            // OR shortcut
            if (_memo.Results.Peek() == null) { _memo.Results.Pop(); _index = _start_i32; } else goto label32;

            // LITERAL "Cs"
            _ParseLiteralString(_memo, ref _index, "Cs");

        label32: // OR
            int _dummy_i32 = _index; // no-op for label

            // OR shortcut
            if (_memo.Results.Peek() == null) { _memo.Results.Pop(); _index = _start_i31; } else goto label31;

            // LITERAL "Co"
            _ParseLiteralString(_memo, ref _index, "Co");

        label31: // OR
            int _dummy_i31 = _index; // no-op for label

        label3: // OR
            int _dummy_i3 = _index; // no-op for label

            // OR shortcut
            if (_memo.Results.Peek() == null) { _memo.Results.Pop(); _index = _start_i2; } else goto label2;

            // OR 38
            int _start_i38 = _index;

            // OR 39
            int _start_i39 = _index;

            // OR 40
            int _start_i40 = _index;

            // OR 41
            int _start_i41 = _index;

            // OR 42
            int _start_i42 = _index;

            // OR 43
            int _start_i43 = _index;

            // LITERAL "Pc"
            _ParseLiteralString(_memo, ref _index, "Pc");

            // OR shortcut
            if (_memo.Results.Peek() == null) { _memo.Results.Pop(); _index = _start_i43; } else goto label43;

            // LITERAL "Pd"
            _ParseLiteralString(_memo, ref _index, "Pd");

        label43: // OR
            int _dummy_i43 = _index; // no-op for label

            // OR shortcut
            if (_memo.Results.Peek() == null) { _memo.Results.Pop(); _index = _start_i42; } else goto label42;

            // LITERAL "Ps"
            _ParseLiteralString(_memo, ref _index, "Ps");

        label42: // OR
            int _dummy_i42 = _index; // no-op for label

            // OR shortcut
            if (_memo.Results.Peek() == null) { _memo.Results.Pop(); _index = _start_i41; } else goto label41;

            // LITERAL "Pe"
            _ParseLiteralString(_memo, ref _index, "Pe");

        label41: // OR
            int _dummy_i41 = _index; // no-op for label

            // OR shortcut
            if (_memo.Results.Peek() == null) { _memo.Results.Pop(); _index = _start_i40; } else goto label40;

            // LITERAL "Pi"
            _ParseLiteralString(_memo, ref _index, "Pi");

        label40: // OR
            int _dummy_i40 = _index; // no-op for label

            // OR shortcut
            if (_memo.Results.Peek() == null) { _memo.Results.Pop(); _index = _start_i39; } else goto label39;

            // LITERAL "Pf"
            _ParseLiteralString(_memo, ref _index, "Pf");

        label39: // OR
            int _dummy_i39 = _index; // no-op for label

            // OR shortcut
            if (_memo.Results.Peek() == null) { _memo.Results.Pop(); _index = _start_i38; } else goto label38;

            // LITERAL "Po"
            _ParseLiteralString(_memo, ref _index, "Po");

        label38: // OR
            int _dummy_i38 = _index; // no-op for label

        label2: // OR
            int _dummy_i2 = _index; // no-op for label

            // OR shortcut
            if (_memo.Results.Peek() == null) { _memo.Results.Pop(); _index = _start_i1; } else goto label1;

            // OR 51
            int _start_i51 = _index;

            // OR 52
            int _start_i52 = _index;

            // OR 53
            int _start_i53 = _index;

            // LITERAL "Sm"
            _ParseLiteralString(_memo, ref _index, "Sm");

            // OR shortcut
            if (_memo.Results.Peek() == null) { _memo.Results.Pop(); _index = _start_i53; } else goto label53;

            // LITERAL "Sc"
            _ParseLiteralString(_memo, ref _index, "Sc");

        label53: // OR
            int _dummy_i53 = _index; // no-op for label

            // OR shortcut
            if (_memo.Results.Peek() == null) { _memo.Results.Pop(); _index = _start_i52; } else goto label52;

            // LITERAL "Sk"
            _ParseLiteralString(_memo, ref _index, "Sk");

        label52: // OR
            int _dummy_i52 = _index; // no-op for label

            // OR shortcut
            if (_memo.Results.Peek() == null) { _memo.Results.Pop(); _index = _start_i51; } else goto label51;

            // LITERAL "So"
            _ParseLiteralString(_memo, ref _index, "So");

        label51: // OR
            int _dummy_i51 = _index; // no-op for label

        label1: // OR
            int _dummy_i1 = _index; // no-op for label

            // OR shortcut
            if (_memo.Results.Peek() == null) { _memo.Results.Pop(); _index = _start_i0; } else goto label0;

            // LITERAL "Cn"
            _ParseLiteralString(_memo, ref _index, "Cn");

        label0: // OR
            int _dummy_i0 = _index; // no-op for label

        }


        public void EOF(_RegexpParser_Memo _memo, int _index, _RegexpParser_Args _args)
        {

            // NOT 0
            int _start_i0 = _index;

            // ANY
            _ParseAny(_memo, ref _index);

            // NOT 0
            var _r0 = _memo.Results.Pop();
            _memo.Results.Push( _r0 == null ? new _RegexpParser_Item(_start_i0, _memo.InputEnumerable) : null);
            _index = _start_i0;

        }


    } // class RegexpParser

} // namespace Verophyle.Regexp

