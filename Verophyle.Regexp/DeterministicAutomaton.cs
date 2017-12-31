// Verophyle.Regexp Copyright © Verophyle Informatics 2018

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Verophyle.Regexp
{
    public class DeterministicAutomaton<T, TMatcher> : FiniteStateAutomaton<T>
        where TMatcher : IInputMatcher<T>
    {
        IEnumerable<IInputSet<T>> inputClasses = null;
        TMatcher matcher;

        IDictionary<int, IDictionary<int, int>> transitionTable = new Dictionary<int, IDictionary<int, int>>();
        int startIndex = (int)SpecialIndices.FAIL;
        int curIndex = (int)SpecialIndices.FAIL;
        ISet<int> acceptingIndices = new HashSet<int>();

        public DeterministicAutomaton()
        {
        }

        public DeterministicAutomaton(Node.Node<T> node)
            : this()
        {
            Compile(node);
        }

        public int Count { get; private set; }
        public override bool Succeeded { get { return acceptingIndices.Contains(curIndex); } }
        public override bool Failed { get { return curIndex == (int)SpecialIndices.FAIL; } }

        public void Reset()
        {
            Count = 0;
            curIndex = startIndex;
        }

        /// <summary>
        /// Returns true if the DFA matches the entire input string from beginning to end.
        /// </summary>
        /// <param name="inputs">Inputs.</param>
        public override bool Matches(IEnumerable<T> inputs)
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
        public override void ProcessInput(T input)
        {
            if (transitionTable.Count == 0)
                throw new Exception("You cannot increment a DFA that has no data.");

            if (Failed)
                return;

            int nextIndex = (int)SpecialIndices.FAIL;
            
            // check for specific input
            foreach (var inputClass in matcher.GetSetsThatMatch(input))
            {
                IDictionary<int, int> outTable;
                if (transitionTable.TryGetValue(curIndex, out outTable))
                {
                    int outIndex;
                    if (outTable.TryGetValue(inputClass.TransitionIndex, out outIndex))
                    {
                        nextIndex = outIndex;
                        break;
                    }
                }
            }

            curIndex = nextIndex;

            if (!Failed)
                Count++;
        }

        protected void Compile(Node.Node<T> node)
        {
            // clear old data
            ClearData();

            // collect disjoint sets of inputs
            var jointToDisjoint = new Dictionary<IInputSet<T>, ISet<IInputSet<T>>>();
            var tempMatcher = CreateMatcher(Enumerable.Empty<IInputSet<T>>());
            GetDisjointSets(node, jointToDisjoint, tempMatcher);

            // rewrite regex tree to add ORs for disjoint sets of inputs
            var inputsToPositions = new Dictionary<int, ISet<int>>();
            var endingPositions = new HashSet<int>();
            var failingPositions = new HashSet<int>();
            RewriteExpressionTree(jointToDisjoint, inputsToPositions, endingPositions, failingPositions, ref node);

            // get new matcher
            inputClasses = jointToDisjoint.Values.SelectMany(s => s).Distinct().ToArray();
            matcher = CreateMatcher(inputClasses);

            // now compile
            // a d-state is a set of positions from the regex
            // an input symbol is a disjoint set of TInput
            var followPos = node.FollowPos;
            node.FixNeg(followPos, -1);

            var detStates = new List<ISet<int>>();
            var unmarked = new Stack<ISet<int>>();
            var statesToIndices = new Dictionary<ISet<int>, int>();

            var startState = node.FirstPos;
            statesToIndices[startState] = detStates.Count;
            detStates.Add(startState);
            unmarked.Push(startState);

            while (unmarked.Count > 0)
            {
                var S = unmarked.Pop();

                foreach (var a in inputClasses)
                {
                    ISet<int> U = new HashSet<int>();

                    //
                    ISet<int> positions;
                    if (inputsToPositions.TryGetValue(a.TransitionIndex, out positions))
                    {
                        foreach (var pos in positions)
                            if (S.Contains(pos))
                                U.UnionWith(followPos[pos]);
                    }

                    // 
                    if (U.Count > 0)
                    {
                        ISet<int> found = null;
                        foreach (var seenState in detStates)
                        {
                            if (seenState.SetEquals(U))
                            {
                                found = seenState;
                                break;
                            }
                        }

                        if (found != null)
                        {
                            U = found;
                        }
                        else
                        {
                            statesToIndices[U] = detStates.Count;
                            detStates.Add(U);
                            unmarked.Push(U);
                        }

                        //
                        int inIndex = statesToIndices[S];
                        int outIndex = statesToIndices[U];

                        if (S.Overlaps(failingPositions) && !(a is InputSet.DotSet<T>))
                            outIndex = (int)SpecialIndices.FAIL;

                        IDictionary<int, int> outTransitions;
                        if (!transitionTable.TryGetValue(inIndex, out outTransitions))
                        {
                            outTransitions = new Dictionary<int, int>();
                            transitionTable[inIndex] = outTransitions;
                        }
                        outTransitions[a.TransitionIndex] = outIndex;
                    }
                }
            }

            // start index
            startIndex = 0;

            // get accepting states
            acceptingIndices = new HashSet<int>(detStates.Where(state => state.Overlaps(endingPositions)).Select(state => statesToIndices[state]));

            //
            Reset();
        }

        TMatcher CreateMatcher(IEnumerable<IInputSet<T>> inputClasses)
        {
#if NETSTANDARD1_4
            var ctor = typeof(TMatcher).GetTypeInfo().DeclaredConstructors.FirstOrDefault(c =>
            {
                var parms = c.GetParameters();
                return parms.Length == 1 && typeof(IEnumerable<IInputSet<T>>).GetTypeInfo().IsAssignableFrom(parms[0].ParameterType.GetTypeInfo());
            });
#else
            var ctor = typeof(TMatcher).GetConstructor(new Type[] { typeof(IEnumerable<IInputSet<T>>) });
#endif
            if (ctor == null)
            {
                throw new Exception(string.Format("Unable to find a constructor for {0} with a parameter of type IEnumerable<IInputSet<{1}>>",
                    typeof(TMatcher).Name, typeof(T).Name));
            }

            var matcher = (TMatcher)ctor.Invoke(new object[] { inputClasses });
            return matcher;
        }

        bool IsMarked(IDictionary<ISet<int>, bool> stateMarks, ISet<int> dState)
        {
            bool isMarked;
            stateMarks.TryGetValue(dState, out isMarked);
            return isMarked;
        }

        static readonly Type[] CTOR_PARAM_TYPES = new Type[] { typeof(IInputSet<T>), Type.GetType("System.Int32&") };

        void RewriteExpressionTree(IDictionary<IInputSet<T>, ISet<IInputSet<T>>> jointToDisjoint, 
                                   IDictionary<int, ISet<int>> inputsToPositions, 
                                   ISet<int> acceptingPositions, ISet<int> failingPositions,
                                   ref Node.Node<T> node)
        {
            Node.Binary<T> binNode;
            Node.Star<T> starNode;
            Node.Leaf<T> leafNode;
            Node.End<T> endNode;

            if ((binNode = node as Node.Binary<T>) != null)
            {
                if (binNode.Left != null)
                {
                    var tmp = binNode.Left;
                    RewriteExpressionTree(jointToDisjoint, inputsToPositions, acceptingPositions, failingPositions, ref tmp);
                    binNode.Left = tmp;
                }

                if (binNode.Right != null)
                {
                    var tmp = binNode.Right;
                    RewriteExpressionTree(jointToDisjoint, inputsToPositions, acceptingPositions, failingPositions, ref tmp);
                    binNode.Right = tmp;
                }
            }
            else if ((starNode = node as Node.Star<T>) != null)
            {
                if (starNode.Child != null)
                {
                    var tmp = starNode.Child;
                    RewriteExpressionTree(jointToDisjoint, inputsToPositions, acceptingPositions, failingPositions, ref tmp);
                    starNode.Child = tmp;
                }
            }
            else if ((endNode = node as Node.End<T>) != null)
            {
                acceptingPositions.Add(endNode.Pos);
            }
            else if ((leafNode = node as Node.Leaf<T>) != null && leafNode.Input != null)
            {
                ISet<IInputSet<T>> disjointSets;
                if (jointToDisjoint.TryGetValue(leafNode.Input, out disjointSets))
                {
                    if (disjointSets.Count == 1)
                    {
                        AddPositionForInput(inputsToPositions, disjointSets.First(), leafNode.Pos);
                    }
                    else
                    {
                        Node.Node<T> topNode = null;
                        var ctor =
#if NETSTANDARD1_4
                            node.GetType().GetTypeInfo().DeclaredConstructors.FirstOrDefault(c =>
                            {
                                var parms = c.GetParameters();
                                if (parms.Length != CTOR_PARAM_TYPES.Length)
                                    return false;
                                for (int i = 0; i < parms.Length; i++)
                                {
                                    if (!CTOR_PARAM_TYPES[i].GetTypeInfo().IsAssignableFrom(parms[i].ParameterType.GetTypeInfo()))
                                        return false;
                                }
                                return true;
                            });
#else
                            node.GetType().GetConstructor(CTOR_PARAM_TYPES);
#endif
                        if (ctor == null)
                        {
                            throw new Exception(string.Format("Unable to find constructor for {0} with parameters of type ({1})",
                                node.GetType().Name,
                                string.Join(", ", CTOR_PARAM_TYPES.Select(t => t.Name))));
                        }

                        foreach (var disjoint in disjointSets)
                        {
                            var newNode = (Node.Node<T>)ctor.Invoke(new object[] { disjoint, leafNode.Pos });
                            topNode = topNode != null ? new Node.Disj<T>(topNode, newNode) : newNode;

                            AddPositionForInput(inputsToPositions, disjoint, leafNode.Pos);
                        }

                        if (topNode != null)
                            node = topNode;
                    }
                }
                else
                {
                    throw new Exception("Internal error compiling regex: no disjoint set equivalent found for regex leaf.");
                }
            }
        }

        void AddPositionForInput(IDictionary<int, ISet<int>> inputsToPositions, IInputSet<T> input, int pos)
        {
            ISet<int> positions;
            if (!inputsToPositions.TryGetValue(input.TransitionIndex, out positions))
            {
                positions = new HashSet<int>();
                inputsToPositions[input.TransitionIndex] = positions;
            }
            positions.Add(pos);
        }

        public static void GetDisjointSets(
            Node.Node<T> node,
            IDictionary<IInputSet<T>, ISet<IInputSet<T>>> jointToDisjoint,
            TMatcher matcher)
        {
            var possiblyJointSets = new List<IInputSet<T>>();
            CollectJointSets(possiblyJointSets, node);

            // get disjoint sets
            var inputCodeToDisjointSet = new Dictionary<int, IInputSet<T>>();
            foreach (var jointSet in possiblyJointSets)
            {
                // find subsets shared with other new sets
                var setsWeShareWith = new Dictionary<IInputSet<T>, ISet<int>>();

                foreach (var inputCode in jointSet.InputCodes)
                {
                    IInputSet<T> existingSet;
                    if (inputCodeToDisjointSet.TryGetValue(inputCode, out existingSet))
                    {
                        ISet<int> sharedCodes;
                        if (!setsWeShareWith.TryGetValue(existingSet, out sharedCodes))
                        {
                            sharedCodes = new HashSet<int>();
                            setsWeShareWith[existingSet] = sharedCodes;
                        }
                        sharedCodes.Add(inputCode);
                    }
                }

                // if we share any items with existing sets, split them off
                if (setsWeShareWith.Count > 0)
                {
                    // clear all references to our inputs and those of the sets we share inputs with
                    foreach (var inputCode in jointSet.InputCodes)
                        inputCodeToDisjointSet.Remove(inputCode);
                    foreach (var sharedSet in setsWeShareWith.Keys)
                        foreach (var inputCode in sharedSet.InputCodes)
                            inputCodeToDisjointSet.Remove(inputCode);

                    // now, for each set that shares with us, split the shared and unshared items
                    var unsharedInputCodeItems = new HashSet<int>(jointSet.InputCodes);
                    foreach (var sharedSet in setsWeShareWith.Keys)
                    {
                        var unsharedCodes = new HashSet<int>(sharedSet.InputCodes);
                        unsharedCodes.ExceptWith(setsWeShareWith[sharedSet]);
                        unsharedInputCodeItems.ExceptWith(setsWeShareWith[sharedSet]);

                        var first = matcher.GetNewSetFromCodes(unsharedCodes);
                        var second = matcher.GetNewSetFromCodes(setsWeShareWith[sharedSet]);

                        foreach (var inputCode in first.InputCodes)
                            inputCodeToDisjointSet[inputCode] = first;
                        foreach (var inputCode in second.InputCodes)
                            inputCodeToDisjointSet[inputCode] = second;
                    }
                    
                    // now make a new class for the items left over
                    if (unsharedInputCodeItems.Count > 0)
                    {
                        var third = matcher.GetNewSetFromCodes(unsharedInputCodeItems);
                        foreach (var inputCode in third.InputCodes)
                            inputCodeToDisjointSet[inputCode] = third;
                    }
                }
                // otherwise just point all our inputs to us
                else
                {
                    foreach (var inputCode in jointSet.InputCodes)
                        inputCodeToDisjointSet[inputCode] = jointSet;
                }
            }

            // build the mapping from joint to disjoint
            foreach (var jointSet in possiblyJointSets)
            {
                if (jointSet is InputSet.DotSet<T>)
                {
                    jointToDisjoint[jointSet] = new HashSet<IInputSet<T>> { jointSet };
                }
                else
                {
                    var disjointSets = new HashSet<IInputSet<T>>();
                    jointToDisjoint[jointSet] = disjointSets;
                    foreach (var inputCode in jointSet.InputCodes)
                        disjointSets.Add(inputCodeToDisjointSet[inputCode]);
                }
            }
        }

        static void CollectJointSets(IList<IInputSet<T>> possiblyJointSets, Node.Node<T> node)
        {
            var leaves = node.InLCROrder().OfType<Node.Leaf<T>>().Where(leaf => leaf.Input != null);
            foreach (var leafNode in leaves)
            {
                if (leafNode.Input is InputSet.DotSet<T>)
                {
                    possiblyJointSets.Add(leafNode.Input);
                }
                else if (leafNode.Input.InputCodes.Any())
                {
                    bool found = false;
                    foreach (var js in possiblyJointSets)
                    {
                        if (js.Equals(leafNode.Input))
                        {
                            found = true;
                            leafNode.Input = js;
                            break;
                        }
                    }

                    if (!found)
                        possiblyJointSets.Add(leafNode.Input);
                }
            }
        }

        void ClearData()
        {
            transitionTable.Clear();
            acceptingIndices.Clear();
            curIndex = (int)SpecialIndices.FAIL;
        }

        public override string ToString()
        {
            return GetTable();
        }

#if DEBUG
        public string DebugTable
        {
            get { return GetTable(); }
        }
#endif

        string GetTable()
        {
            if (transitionTable.Count == 0)
                return "<empty>";

            var sb = new StringBuilder();
            sb.Append("     ");
            foreach (var inputSymbol in inputClasses)
            {
                sb.AppendFormat("{0,15}", inputSymbol);
            }
            sb.AppendLine();

            foreach (var index in transitionTable.Keys.OrderBy(n => n))
            {
                sb.AppendFormat("{0,-5}", GetStr(index) + ":");

                foreach (var inputSymbol in inputClasses)
                {
                    IDictionary<int, int> outTable;
                    string strIndex;
                    int nextIndex = (int)SpecialIndices.FAIL;
                    if (transitionTable.TryGetValue(index, out outTable) 
                        && outTable.TryGetValue(inputSymbol.TransitionIndex, out nextIndex))
                        strIndex = (nextIndex == (int)SpecialIndices.FAIL) ? "FAIL" : GetStr(nextIndex);
                    else
                        strIndex = string.Empty;
                    sb.AppendFormat("{0,15}", strIndex);
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
            if (acceptingIndices.Contains(index))
                res =  "*" + res;

            return res;
        }
    }
}
