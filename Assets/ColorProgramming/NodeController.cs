using ColorProgramming.Core;
using System.Collections;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;
using UnityEngine.Events;

namespace ColorProgramming
{
    public abstract class NodeController : MonoBehaviour, ITappable
    {
        public Node Node;

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

                        Debug.Log(runtimeAction);
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

        protected UnityAction ParseActionSignal(string actionSignal)
        {
            return actionSignal switch
            {
                "connect"
                    => () =>
                        GameManager.Instance.TouchController.TouchServiceManager.RegisterService(
                            new NodeConnectService(this, Enums.ConnectionServiceMode.CONNECT, true)
                        ),
                "disconnect"
                    => () =>
                        GameManager.Instance.TouchController.TouchServiceManager.RegisterService(
                            new NodeConnectService(
                                this,
                                Enums.ConnectionServiceMode.DISCONNECT,
                                true
                            )
                        ),
                _ => () => { },
            };
        }
    }
}
