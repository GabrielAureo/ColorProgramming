using ColorProgramming.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ColorProgramming.Items
{
    public class OutputNodeItemController : ConcreteItemController<OutputNode>
    {
        protected override void SetupConcreteController(ConcreteNodeController<OutputNode> nodeController)
        {
            throw new NotImplementedException();
        }

        protected override void SetupConcreteNode(OutputNode concreteNode)
        {
        }
    }
}
