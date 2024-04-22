using System;
using ColorProgramming.Core;
using UnityEngineInternal;

namespace ColorProgramming.Items
{
    public class CapsuleNodeItemController : ConcreteItemController<CapsuleNode>
    {
        Guid guid = new();

        protected override void SetupConcreteController(ConcreteNodeController<CapsuleNode> nodeController)
        {
        }

        protected override void SetupConcreteNode(CapsuleNode concreteNode)
        {

            concreteNode.ScopeKey = guid;
        }
    }
}