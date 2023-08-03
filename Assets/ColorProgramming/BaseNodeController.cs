using ColorProgramming.Core;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;
using UnityEngine.Events;

namespace ColorProgramming
{
    public abstract class BaseNodeController : MonoBehaviour, ITappable
    {
        public abstract Node Node { get; set; }
        protected virtual Dictionary<string, UnityAction> ActionSignalMap =>
            new() { { "connect", ConnectAction }, { "disconnect", DisconnectAction }, };

        [SerializeField]
        protected ContextMenuData menuData;

        public ContextMenu GetContextMenu()
        {
            var data = menuData.actions
                .Select(
                    (action) =>
                    {
                        var runtimeAction = new RuntimeContextMenuAction()
                        {
                            ActionTitle = action.ActionTitle,
                            ActionIcon = action.ActionIcon,
                            Action = ParseActionSignal(action.ActionSignal)
                        };

                        return runtimeAction;
                    }
                )
                .ToArray();
            return new ContextMenu()
            {
                data = data,
                worldPosition = transform.position + (Vector3.up * 3f)
            };
        }

        protected void ConnectAction()
        {
            GameManager.Instance.TouchController.TouchServiceManager.RegisterService(
                new NodeConnectService(this, Enums.ConnectionServiceMode.CONNECT, true)
            );
        }

        protected void DisconnectAction()
        {
            GameManager.Instance.TouchController.TouchServiceManager.RegisterService(
                new NodeConnectService(this, Enums.ConnectionServiceMode.DISCONNECT, true)
            );
        }

        private UnityAction ParseActionSignal(string actionSignal)
        {
            if (ActionSignalMap.TryGetValue(actionSignal, out var action))
                return action;
            else
                return () => { };
        }
    }
}
