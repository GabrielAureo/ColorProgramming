using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace ColorProgramming
{
    public class ContextMenuService : ARTouchService
    {
        public ContextMenuService(bool isExclusive)
            : base(isExclusive) { }

        public override void OnHold(ARTouchData touchData) { }

        public override void OnRelease(ARTouchData touchData) { }

        public override void OnTap(ARTouchData touchData)
        {
            var interactable = touchData.selectedInteractable;

            if (interactable is NodeController)
            {
                var nodeController = interactable as NodeController;
                var contextMenu = nodeController.GetContextMenu();
                GameManager.Instance.ContextMenuController.SetContextMenu(contextMenu);
            }
            else
            {
                GameManager.Instance.ContextMenuController.EnableMenu(false);
            }
        }
    }
}
