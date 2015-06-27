// Verophyle.Regexp Copyright © Verophyle Informatics 2015

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Verophyle.Regexp
{
    public class DeterministicAutomaton<TInput> : FiniteStateAutomaton
    {
        IEnumerable<InputClass<TInput>> input_classes = null;
        IDictionary<int, IDictionary<int, int>> transition_table = new Dictionary<int, IDictionary<int, int>>();
        IInputClassMatcher<TInput> matcher = null;
        int start_index = (int)SpecialIndices.FAIL;
        int cur_index = (int)SpecialIndices.FAIL;
        ISet<int> accepting_indices = new HashSet<int>();

        string Table { get { return GetTable(); } }

        public int Count { get; private set; }
        public bool Succeeded { get { return accepting_indices.Contains(cur_index); } }
        public bool Failed { get { return cur_index == (int)SpecialIndices.FAIL; } }

        public DeterministicAutomaton()
        {
        }

        public DeterministicAutomaton(Regexp<InputClass<TInput>> regex)
            : this()
        {
            Compile(regex);
        }

        public void Reset()
        {
            Count = 0;
            cur_index = start_index;
        }

        /// <summary>
        /// Returns true if the DFA matches the entire input string from beginning to end.
        /// </summary>
        /// <param name="inputs">Inputs.</param>
        public bool Matches(IEnumerable<TInput> inputs)
        {
            Reset();
            foreach (var input in inputs)
                ProcessInput(input);
            return Succeeded;
        }

        /// <summary>
        /// Processes one input.  You can use <see cref="Succeeded"/> or <see cref="Failed"/> to check the state of the DFA after calling this.
        /// </summary>
        /// <param name="input">Input.</param>
        public void ProcessInput(TInput input)
        {
            if (transition_table.Count == 0)
                throw new Exception("You cannot increment a DFA that has no data.");

            if (Failed)
                return;

            int next_index = (int)SpecialIndices.FAIL;
            
            // check for specific input
            foreach (var input_class in matcher.ClassThatMatched(input))
            {
                IDictionary<int, int> out_table;
                if (transition_table.TryGetValue(cur_index, out out_table))
                {
                    int out_index;
                    if (out_table.TryGetValue(input_class.TransitionIndex, out out_index))
                    {
                        next_index = out_index;
                        break;
                    }
                }
            }

            cur_index = next_index;

            if (!Failed)
                Count++;
        }

        protected void Compile(Regexp<InputClass<TInput>> regex)
        {
            // clear old data
            ClearData();

            // collect disjoint sets of inputs
            var joint_to_disjoint = new Dictionary<InputClass<TInput>, ISet<InputClass<TInput>>>();
            InputClass<TInput> most_specialized_input_class;
            GetDisjointSets(joint_to_disjoint, regex, out most_specialized_input_class);

            // rewrite regex tree to add ORs for disjoint sets of inputs
            var inputs_to_positions = new Dictionary<int, ISet<int>>();
            var end_positions = new HashSet<int>();
            var fail_positions = new HashSet<int>();
            RewriteExpressionTree(joint_to_disjoint, inputs_to_positions, end_positions, fail_positions, ref regex);

            // get new matcher
            input_classes = joint_to_disjoint.Values.SelectMany(s => s).Distinct().ToArray();
            matcher = most_specialized_input_class.GetMatcher(input_classes);

            // now compile
            // a d-state is a set of positions from the regex
            // an input symbol is a disjoint set of TInput
            var follow_pos = regex.FollowPos;
            regex.FixNeg(follow_pos, -1);

            var d_states = new List<ISet<int>>();
            var unmarked = new Stack<ISet<int>>();
            var states_to_indices = new Dictionary<ISet<int>, int>();

            var start_state = regex.FirstPos;
            states_to_indices[start_state] = d_states.Count;
            d_states.Add(start_state);
            unmarked.Push(start_state);

            while (unmarked.Count > 0)
            {
                var S = unmarked.Pop();

                foreach (var a in input_classes)
                {
                    ISet<int> U = new HashSet<int>();

                    //
                    ISet<int> positions;
                    if (inputs_to_positions.TryGetValue(a.TransitionIndex, out positions))
                    {
                        foreach (var pos in positions)
                            if (S.Contains(pos))
                                U.UnionWith(follow_pos[pos]);
                    }

                    // 
                    if (U.Count > 0)
                    {
                        ISet<int> found = null;
                        foreach (var seen_state in d_states)
                        {
                            if (seen_state.SetEquals(U))
                            {
                                found = seen_state;
                                break;
                            }
                        }

                        if (found != null)
                        {
                            U = found;
                        }
                        else
                        {
                            states_to_indices[U] = d_states.Count;
                            d_states.Add(U);
                            unmarked.Push(U);
                        }

                        //
                        int in_index = states_to_indices[S];
                        int out_index = states_to_indices[U];

                        if (S.Overlaps(fail_positions) && !(a is InputClass<TInput>.DotClass))
                            out_index = (int)SpecialIndices.FAIL;

                        IDictionary<int, int> out_trans;
                        if (!transition_table.TryGetValue(in_index, out out_trans))
                        {
                            out_trans = new Dictionary<int, int>();
                            transition_table[in_index] = out_trans;
                        }
                        out_trans[a.TransitionIndex] = out_index;
                    }
                }
            }

            // start index
            start_index = 0;

            // get accepting states
            accepting_indices = new HashSet<int>(d_states.Where(state => state.Overlaps(end_positions)).Select(state => states_to_indices[state]));

            //
            Reset();
        }

        bool IsMarked(IDictionary<ISet<int>, bool> state_marks, ISet<int> d_state)
        {
            bool is_marked;
            state_marks.TryGetValue(d_state, out is_marked);
            return is_marked;
        }

        static Type[] CTOR_PARAM_TYPES = new Type[] { typeof(InputClass<TInput>), Type.GetType("System.Int32&") };

        void RewriteExpressionTree(IDictionary<InputClass<TInput>, ISet<InputClass<TInput>>> joint_to_disjoint, 
                                   IDictionary<int, ISet<int>> inputs_to_positions, 
                                   ISet<int> accepting_positions, ISet<int> fail_positions,
                                   ref Regexp<InputClass<TInput>> regex)
        {
            if (regex.Left != null)
            {
                var tmp = regex.Left;
                RewriteExpressionTree(joint_to_disjoint, inputs_to_positions, accepting_positions, fail_positions, ref tmp);
                regex.Left = tmp;
            }

            if (regex.Right != null)
            {
                var tmp = regex.Right;
                RewriteExpressionTree(joint_to_disjoint, inputs_to_positions, accepting_positions, fail_positions, ref tmp);
                regex.Right = tmp;
            }

            //RegularExpression.Fail<InputClass<TInput>> fail;
            //if ((fail = regex as RegularExpression.Fail<InputClass<TInput>>) != null)
            //    fail_positions.UnionWith(fail.FirstPos);

            Node.Leaf<InputClass<TInput>> leaf;
            Node.End<InputClass<TInput>> end;

            if ((end = regex as Node.End<InputClass<TInput>>) != null)
            {
                accepting_positions.Add(end.Pos);
            }
            else if ((leaf = regex as Node.Leaf<InputClass<TInput>>) != null && leaf.Input != null)
            {
                ISet<InputClass<TInput>> disjoint_sets;
                if (joint_to_disjoint.TryGetValue(leaf.Input, out disjoint_sets))
                {
                    if (disjoint_sets.Count == 1)
                    {
                        AddPositionForInput(inputs_to_positions, disjoint_sets.First(), leaf.Pos);
                    }
                    else
                    {
                        Regexp<InputClass<TInput>> top = null;
                        foreach (var disjoint in disjoint_sets)
                        {
                            var ctor = regex.GetType().GetConstructor(CTOR_PARAM_TYPES);
                            var node = (Regexp<InputClass<TInput>>)ctor.Invoke(new object[] { disjoint, leaf.Pos });

                            top = top != null ? new Node.Or<InputClass<TInput>>(top, node) : node;

                            AddPositionForInput(inputs_to_positions, disjoint, leaf.Pos);
                        }

                        if (top != null)
                            regex = top;
                    }
                }
                else
                {
                    throw new Exception("Internal error compiling regex: no disjoint set equivalent found for regex leaf.");
                }
            }
        }

        void AddPositionForInput(IDictionary<int, ISet<int>> inputs_to_positions, InputClass<TInput> input, int pos)
        {
            ISet<int> positions;
            if (!inputs_to_positions.TryGetValue(input.TransitionIndex, out positions))
            {
                positions = new HashSet<int>();
                inputs_to_positions[input.TransitionIndex] = positions;
            }
            positions.Add(pos);
        }

        public static void GetDisjointSets(
            IDictionary<InputClass<TInput>, ISet<InputClass<TInput>>> joint_to_disjoint, 
            Regexp<InputClass<TInput>> regex,
            out InputClass<TInput> most_specialized_input_class)
        {
            var possibly_joint_sets = new List<InputClass<TInput>>();
            CollectJointSets(possibly_joint_sets, regex);

            most_specialized_input_class = GetMostSpecializedInputClass(possibly_joint_sets);

            // get disjoint sets
            var input_codes_to_disjoint_sets = new Dictionary<int, InputClass<TInput>>();
            foreach (var joint_set in possibly_joint_sets)
            {
                // find subsets shared with other new sets
                var sets_we_share_with = new Dictionary<InputClass<TInput>, ISet<int>>();

                foreach (var input_code in joint_set.InputCodes)
                {
                    InputClass<TInput> existing_set;
                    if (input_codes_to_disjoint_sets.TryGetValue(input_code, out existing_set))
                    {
                        ISet<int> shared_codes;
                        if (!sets_we_share_with.TryGetValue(existing_set, out shared_codes))
                        {
                            shared_codes = new HashSet<int>();
                            sets_we_share_with[existing_set] = shared_codes;
                        }
                        shared_codes.Add(input_code);
                    }
                }

                // if we share any items with existing sets, split them off
                if (sets_we_share_with.Count > 0)
                {
                    // clear all references to our inputs and those of the sets we share inputs with
                    foreach (var input_code in joint_set.InputCodes)
                        input_codes_to_disjoint_sets.Remove(input_code);
                    foreach (var shared_set in sets_we_share_with.Keys)
                        foreach (var input_code in shared_set.InputCodes)
                            input_codes_to_disjoint_sets.Remove(input_code);

                    // now, for each set that shares with us, split the shared and unshared items
                    var unshared_code_items = new HashSet<int>(joint_set.InputCodes);
                    foreach (var shared_set in sets_we_share_with.Keys)
                    {
                        var unshared_codes = new HashSet<int>(shared_set.InputCodes);
                        unshared_codes.ExceptWith(sets_we_share_with[shared_set]);
                        unshared_code_items.ExceptWith(sets_we_share_with[shared_set]);

                        var first = most_specialized_input_class.GetNewClassFromCodes(unshared_codes);
                        var second = most_specialized_input_class.GetNewClassFromCodes(sets_we_share_with[shared_set]);

                        foreach (var input_code in first.InputCodes)
                            input_codes_to_disjoint_sets[input_code] = first;
                        foreach (var input_code in second.InputCodes)
                            input_codes_to_disjoint_sets[input_code] = second;
                    }
                    
                    // now make a new class for the items left over
                    if (unshared_code_items.Count > 0)
                    {
                        var third = most_specialized_input_class.GetNewClassFromCodes(unshared_code_items);
                        foreach (var input_code in third.InputCodes)
                            input_codes_to_disjoint_sets[input_code] = third;
                    }
                }
                // otherwise just point all our inputs to us
                else
                {
                    foreach (var input_code in joint_set.InputCodes)
                        input_codes_to_disjoint_sets[input_code] = joint_set;
                }
            }

            // build the mapping from joint to disjoint
            foreach (var joint_set in possibly_joint_sets)
            {
                if (joint_set is InputClass<TInput>.DotClass)
                {
                    joint_to_disjoint[joint_set] = new HashSet<InputClass<TInput>> { joint_set };
                }
                else
                {
                    var disjoint_sets = new HashSet<InputClass<TInput>>();
                    joint_to_disjoint[joint_set] = disjoint_sets;
                    foreach (var input_codes in joint_set.InputCodes)
                        disjoint_sets.Add(input_codes_to_disjoint_sets[input_codes]);
                }
            }
        }

        static void CollectJointSets(IList<InputClass<TInput>> possibly_joint_sets, Regexp<InputClass<TInput>> regex)
        {
            if (regex.Input is InputClass<TInput>.DotClass)
            {
                possibly_joint_sets.Add(regex.Input);
            }
            else if (regex.Input != null && regex.Input.InputCodes.Any())
            {
                bool found = false;
                foreach (var js in possibly_joint_sets)
                {
                    if (js.Equals(regex.Input))
                    {
                        found = true;
                        regex.Input = js;
                        break;
                    }
                }

                if (!found)
                    possibly_joint_sets.Add(regex.Input);
            }

            if (regex.Left != null)
                CollectJointSets(possibly_joint_sets, regex.Left);
            if (regex.Right != null)
                CollectJointSets(possibly_joint_sets, regex.Right);
        }

        static InputClass<TInput> GetMostSpecializedInputClass(IEnumerable<InputClass<TInput>> joint_sets)
        {
            int max_depth = -1;
            InputClass<TInput> result = null;
            foreach (var set in joint_sets)
            {
                var type = set.GetType();
                int depth = 0;
                while ((type = type.BaseType) != null)
                    ++depth;
                if (depth > max_depth)
                {
                    max_depth = depth;
                    result = set;
                }
            }
            return result;
        }

        void ClearData()
        {
            transition_table.Clear();
            matcher = null;
            accepting_indices.Clear();
            cur_index = (int)SpecialIndices.FAIL;
        }

        public override string ToString()
        {
            return GetTable();
        }

        string GetTable()
        {
            if (transition_table.Count == 0)
                return "<empty>";

            var sb = new StringBuilder();
            sb.Append("     ");
            foreach (var input_symbol in input_classes)
            {
                sb.AppendFormat("{0,15}", input_symbol);
            }
            sb.AppendLine();

            foreach (var index in transition_table.Keys.OrderBy(n => n))
            {
                sb.AppendFormat("{0,-5}", GetStr(index) + ":");

                foreach (var input_symbol in input_classes)
                {
                    IDictionary<int, int> out_table;
                    int next_index; string str_index;
                    if (transition_table.TryGetValue(index, out out_table) && out_table.TryGetValue(input_symbol.TransitionIndex, out next_index))
                        str_index = (next_index == (int)SpecialIndices.FAIL) ? "FAIL" : GetStr(next_index);
                    else
                        str_index = string.Empty;
                    sb.AppendFormat("{0,15}", str_index);
                }

                sb.AppendLine();
            }

            return sb.ToString();
        }

        string GetStr(int index)
        {
            string res = index.ToString();

            if (index == 0)
                res = "<" + res + ">";
            if (accepting_indices.Contains(index))
                res =  "*" + res;

            return res;
        }
    }
}
