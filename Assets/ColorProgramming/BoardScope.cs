using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ColorProgramming
{
    public class BoardScope
    {
        public List<EdgeController> EdgeControllers { get; private set; }
        public List<BaseNodeController> NodeControllers { get; private set; }

        public BoardScope()
        {
            EdgeControllers = new();
            NodeControllers = new();
        }

        public BoardScope(
            List<EdgeController> edgeControllers,
            List<BaseNodeController> nodeControllers
        )
        {
            EdgeControllers = edgeControllers;
            NodeControllers = nodeControllers;
        }

        public void HideScope()
        {
            ToggleScope(false);
        }

        public void ShowScope()
        {
            ToggleScope(true);
        }

        private void ToggleScope(bool active)
        {
            foreach (var edgeController in EdgeControllers)
            {
                edgeController.gameObject.SetActive(active);
            }

            foreach (var nodeController in NodeControllers)
            {
                nodeController.gameObject.SetActive(active);
            }
        }
    }
}
