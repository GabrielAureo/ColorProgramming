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
        public Node Node { get; private set; }

        [SerializeField]
        protected ContextMenuData menuData;

        public void SetupController(Node node) => Node = node;

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

        protected abstract UnityAction ParseActionSignal(string actionSignal);
    }
}
