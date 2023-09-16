using AssetsolorProgramming.Items;
using ColorProgramming.Core;

namespace ColorProgramming.Items
{
    public class LoopNodeItemController : ConcreteItemController<LoopNode>
    {
        protected override void SetupConcreteNode(LoopNode concreteNode)
        {
            concreteNode.TotalLoops = ConcreteItem.ConcreteNode.TotalLoops;

        }
    }
}
