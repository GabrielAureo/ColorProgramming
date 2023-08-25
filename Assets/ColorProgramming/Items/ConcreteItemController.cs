using ColorProgramming;
using ColorProgramming.Core;
using ColorProgramming.Items;
using System;
using UnityEngine;

namespace AssetsolorProgramming.Items
{
    [Serializable]
    public class ConcreteItemController<T> : ItemController
        where T : Node
    {
        public ConcreteItem<T> ConcreteItem;

        public override Item Item
        {
            get => ConcreteItem;
            set => ConcreteItem = (ConcreteItem<T>)value;
        }

        protected override void SetupItem(Movable movable)
        {
            if (movable.TryGetComponent<BaseNodeController>(out var nodeController))
            {
                nodeController.Node = ConcreteItem.ConcreteNode;
            }
        }
    }
}
