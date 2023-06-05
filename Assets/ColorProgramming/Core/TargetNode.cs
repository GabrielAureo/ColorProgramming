namespace ColorProgramming.Core
{
    public class TargetNode : Node
    {
        public Element? CurrentElement { get; private set; }

        public TargetNode(Element element)
            : base()
        {
            CurrentElement = element;
        }
    }
}
