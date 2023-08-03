using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ColorProgramming.Core
{
    [Serializable]
    public class LoopNode : Node
    {
        [Min(0)]
        public int TotalLoops;
        public int LoopCount { get; private set; }
        public List<Node> Children { get; private set; }

        public void DoLoop()
        {
            if (LoopCount < TotalLoops)
                LoopCount++;
        }

        public LoopNode()
            : base()
        {
            LoopCount = 0;
            Children = new List<Node>();
        }
    }
}
