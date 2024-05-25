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
        [SerializeField]
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
                worldPosition = transform.position + (Vector3.up * .2f)
            };
        }

        protected void ConnectAction()
        {
            var touchService = GameManager.Instance.TouchController.TouchServiceManager;

            var service = new NodeConnectService(this, Enums.ConnectionServiceMode.CONNECT, true);
            touchService.RegisterService(service);
            Toast.Show(null, "Pressione outro objeto para conectar", () => touchService.UnregisterService(service));
        }

        protected void DisconnectAction()
        {
            var touchService = GameManager.Instance.TouchController.TouchServiceManager;

            var service = new NodeConnectService(this, Enums.ConnectionServiceMode.DISCONNECT, true);
            touchService.RegisterService(service);

            Toast.Show(null, "Pressione outro objeto para desconectar", () => touchService.UnregisterService(service));

        }

        private UnityAction ParseActionSignal(string actionSignal)
        {
            if (ActionSignalMap.TryGetValue(actionSignal, out var action))
                return action;
            else
                return () => { };
        }

        public virtual void OnAgentTouch(AgentController agent) { }
    }
}
