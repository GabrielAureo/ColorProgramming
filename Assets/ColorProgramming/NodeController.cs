using ColorProgramming.Core;
using System.Collections;
using UnityEngine;

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
            var contextMenu = new ContextMenu()
            {
                data = menuData,
                worldPosition = menuWorldPosition,
            };
            GameManager.Instance.ContextMenuController.SetContextMenu(contextMenu);
        }
    }
}
