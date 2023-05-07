using ColorProgramming.Core;
using System.Diagnostics;

namespace ColorProgramming
{
    public class NodeConnectService : ARTouchService
    {
        private NodeController targetNodeController;

        public NodeConnectService(NodeController targetNodeController, bool isExclusive)
            : base(isExclusive)
        {
            this.targetNodeController = targetNodeController;
        }

        public override void OnHold(ARTouchData touchData) { }

        public override void OnRelease(ARTouchData touchData) { }

        public override void OnTap(ARTouchData touchData)
        {
            GameManager.Instance.TouchController.TouchServiceManager.UnregisterService(this);
            if (touchData.selectedInteractable is not NodeController controller)
                return;

            GameManager.Instance.NodeConnectController.CreateConnection(
                targetNodeController,
                controller
            );
        }
    }
}
