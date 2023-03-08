namespace ColorProgramming
{
    public class ConditionalNode : Node
    {
        public Pattern CheckedPattern { get; private set; }
        public Pattern TruePattern { get; private set; }
        public Pattern FalsePattern { get; private set; }
        public bool IsNot { get; private set; }

        public ConditionalNode(Pattern conditionalColor, Pattern trueColor, Pattern falsecolor, bool isNot = false) : base()
        {
            CheckedPattern = conditionalColor;
            TruePattern = trueColor;
            FalsePattern = falsecolor;
            IsNot = isNot;

        }

        public Pattern EvalNode(Player player)
        {
            return player.CurrentPattern == CheckedPattern && !IsNot ? TruePattern : FalsePattern;
        }


    }
}