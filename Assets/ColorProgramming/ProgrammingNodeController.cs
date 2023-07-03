using ColorProgramming.Core;

namespace ColorProgramming
{
    public abstract class ProgrammingNodeController : BaseNodeController
    {
        public abstract void Evaluate(AgentController playerNodeController);
    }
}
