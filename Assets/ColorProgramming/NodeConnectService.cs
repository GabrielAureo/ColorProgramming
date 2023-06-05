using ColorProgramming.Core;
using ColorProgramming.Enums;
using System.Diagnostics;

namespace ColorProgramming
{
    public class NodeConnectService : ARTouchService
    {
        private readonly NodeController targetNodeController;
        private readonly ConnectionServiceMode mode;

        public NodeConnectService(
            NodeController targetNodeController,
            ConnectionServiceMode mode,
            bool isExclusive
        )
            : base(isExclusive)
        {
            this.targetNodeController = targetNodeController;
            this.mode = mode;
        }

        public override void OnHold(ARTouchData touchData) { }

        public override void OnRelease(ARTouchData touchData) { }

        public override void OnTap(ARTouchData touchData)
        {
            GameManager.Instance.TouchController.TouchServiceManager.UnregisterService(this);
            if (touchData.selectedInteractable is not NodeController controller)
                return;

            if (mode == ConnectionServiceMode.CONNECT)
            {
                GameManager.Instance.BoardController.ConnectNodes(targetNodeController, controller);
            }
            else
            {
                GameManager.Instance.BoardController.DisconnectNodes(
                    targetNodeController,
                    controller
                );
            }
        }
    }
}
