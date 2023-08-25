using ColorProgramming.Core;
using System;
using UnityEngine;
using UnityEngine.Events;

namespace ColorProgramming
{
    [CreateAssetMenu(fileName = "New Context Menu", menuName = "ContextMenu", order = 1)]
    public class ContextMenuData : ScriptableObject
    {
        public ContextMenuAction[] actions;
    }

    [Serializable]
    public class ContextMenuAction
    {
        public string ActionTitle;
        public Sprite ActionIcon;
        public string ActionSignal;
    }

    public class RuntimeContextMenuAction
    {
        public string ActionTitle;
        public Sprite ActionIcon;
        public UnityAction Action;
    }

    public class ContextMenu
    {
        public RuntimeContextMenuAction[] data { get; set; }
        public Vector3 worldPosition { get; set; }
    }


}
