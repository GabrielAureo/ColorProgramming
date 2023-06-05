namespace ColorProgramming.Core
{
    public class ConditionalNode : Node
    {
        public Element CheckedElement { get; set; }
        public Element TrueElement { get; set; }
        public Element FalseElement { get; set; }
        public bool IsNot { get; set; }

        public ConditionalNode()
            : base() { }

        public Element EvalNode(PlayerNode player)
        {
            return player.CurrentElement == CheckedElement && !IsNot ? TrueElement : FalseElement;
        }
    }
}
