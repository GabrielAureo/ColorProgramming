using System;

namespace ColorProgramming.Core
{
    [Serializable]
    public class AgentNode : Node
    {
        public Element CurrentElement;

        public delegate void ChangeElementEvent();

        public ChangeElementEvent OnChangeElement;

        public AgentNode(Element currentElement)
            : base()
        {
            CurrentElement = currentElement;
        }

        public void SetElement(Element element)
        {
            var lastElement = CurrentElement;
            CurrentElement = element;
            OnChangeElement?.Invoke();
        }
    }
}
