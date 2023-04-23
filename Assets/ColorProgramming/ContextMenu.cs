using System;
using UnityEngine;
using UnityEngine.Events;

namespace ColorProgramming
{
    [Serializable]
    public class ContextMenu : ScriptableObject
    {
        public ContextMenuAction[] actions;
    }

    [Serializable]
    public class ContextMenuAction
    {
        public string ActionTitle;
        public Sprite ActionIcon;
        public UnityAction Action;
    }
}
