using ColorProgramming.Core;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace ColorProgramming
{
    public class CapsuleNodeController : ConcreteNodeController<CapsuleNode>
    {
        private bool isOpen;

        protected override Dictionary<string, UnityAction> ActionSignalMap =>
            new()
            {
                { "connect", ConnectAction },
                { "disconnect", DisconnectAction },
                { "open-capsule", Open }
            };

        private void Open()
        {
            isOpen = true;
            GameManager.Instance.BoardController.SetScope(ConcreteNode);
        }
    }
}
