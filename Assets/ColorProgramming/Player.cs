using ColorProgramming;
using ColorProgramming.Core;
using System.Collections;
using UnityEngine;

namespace ColorProgramming
{
    public class Player
    {
        public Element CurrentElement { get; private set; }

        public Player()
        {
            CurrentElement = null;
        }

        public Player(Element currentElement)
        {
            CurrentElement = currentElement;
        }

        public void SetElement(Element element)
        {
            CurrentElement = element;
        }
    }
}
