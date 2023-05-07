using ColorProgramming.Core;
using System.Collections;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;
using UnityEngine.Events;

namespace ColorProgramming
{
    public abstract class NodeController<T> : MonoBehaviour
        where T : Node
    {
        public T Node { get; private set; }

        [SerializeField]
        protected ContextMenuData menuData;

        protected void BuildMenu(Vector3 menuWorldPosition)
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
            var contextMenu = new ContextMenu() { data = data, worldPosition = menuWorldPosition, };
            GameManager.Instance.ContextMenuController.SetContextMenu(contextMenu);
        }

        protected abstract UnityAction ParseActionSignal(string actionSignal);
    }
}
