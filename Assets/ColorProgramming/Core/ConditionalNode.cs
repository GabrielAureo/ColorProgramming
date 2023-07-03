using System;
using UnityEngine;

namespace ColorProgramming.Core
{
    [Serializable]
    public class ConditionalNode : Node
    {
        public Element CheckedElement;
        public Element TrueElement;
        public Element FalseElement;
        public bool IsNot;

        public ConditionalNode()
            : base() { }
    }
}
