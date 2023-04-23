namespace ColorProgramming.Core
{
    public class ConditionalNode : Node
    {
        public Element CheckedElement { get; private set; }
        public Element TrueElement { get; private set; }
        public Element FalseElement { get; private set; }
        public bool IsNot { get; private set; }

        public ConditionalNode(
            Element conditionalElement,
            Element trueElement,
            Element falseElement,
            bool isNot = false
        )
            : base()
        {
            CheckedElement = conditionalElement;
            TrueElement = trueElement;
            FalseElement = falseElement;
            IsNot = isNot;
        }

        public Element EvalNode(Player player)
        {
            return player.CurrentElement == CheckedElement && !IsNot ? TrueElement : FalseElement;
        }
    }
}
