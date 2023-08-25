using ColorProgramming.Core;
using System;
using UnityEngine;

namespace ColorProgramming.Items
{
    [Serializable]
    public class Item
    {
        public GameObject NodeGameObject;
        public int ItemQuantity;
        public Node Node;
    }

    [Serializable]
    public class ConcreteItem<T> : Item
        where T : Node
    {
        public T ConcreteNode;
    }
}
