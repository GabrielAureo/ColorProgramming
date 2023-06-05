using ColorProgramming;
using ColorProgramming.Core;
using System.Collections;
using UnityEngine;

namespace ColorProgramming
{
    public class PlayerNode : Node
    {
        public Element? CurrentElement { get; private set; }

        public PlayerNode(Element currentElement)
            : base()
        {
            CurrentElement = currentElement;
        }

        public void SetElement(Element element)
        {
            CurrentElement = element;
        }
    }
}
