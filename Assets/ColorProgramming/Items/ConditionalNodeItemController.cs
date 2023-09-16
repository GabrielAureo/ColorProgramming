using AssetsolorProgramming.Items;
using ColorProgramming.Core;

namespace ColorProgramming.Items
{
    public class ConditionalNodeItemController : ConcreteItemController<ConditionalNode>
    {
        protected override void SetupConcreteNode(ConditionalNode concreteNode)
        {
            concreteNode.CheckedElement = ConcreteItem.ConcreteNode.CheckedElement;
            concreteNode.FalseElement = ConcreteItem.ConcreteNode.FalseElement;
            concreteNode.TrueElement = ConcreteItem.ConcreteNode.TrueElement;
            concreteNode.IsNot = ConcreteItem.ConcreteNode.IsNot;
        }
    }
}
