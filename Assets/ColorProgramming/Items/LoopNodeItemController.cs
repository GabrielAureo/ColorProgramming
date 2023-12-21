using ColorProgramming.Core;

namespace ColorProgramming.Items
{
    public class LoopNodeItemController : ConcreteItemController<LoopNode>
    {
        protected override void SetupConcreteController(ConcreteNodeController<LoopNode> nodeController)
        {
            LoopNodeController loopNodeController = nodeController as LoopNodeController;
            loopNodeController.SpawnBatteries();
        }

        protected override void SetupConcreteNode(LoopNode concreteNode)
        {
            concreteNode.TotalLoops = ConcreteItem.ConcreteNode.TotalLoops;

        }
    }
}
