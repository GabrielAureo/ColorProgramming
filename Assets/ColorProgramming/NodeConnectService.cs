using ColorProgramming.Core;
using ColorProgramming.Enums;
using DG.Tweening;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

namespace ColorProgramming
{
    public class NodeConnectService : ARTouchService
    {
        private readonly BaseNodeController targetNodeController;
        private readonly ConnectionServiceMode mode;

        public NodeConnectService(
            BaseNodeController targetNodeController,
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
            if (touchData.selectedInteractable is not BaseNodeController controller)
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

            Toast.Close();
        }
    }
}
