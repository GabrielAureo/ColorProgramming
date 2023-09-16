using ColorProgramming;
using ColorProgramming.Core;
using ColorProgramming.Items;
using System;
using UnityEngine;

namespace AssetsolorProgramming.Items
{
    [Serializable]
    public abstract class ConcreteItemController<T> : ItemController
        where T : Node
    {
        public ConcreteItem<T> ConcreteItem;

        public override Item Item
        {
            get => ConcreteItem;
            set => ConcreteItem = (ConcreteItem<T>)value;
        }

        protected override void SetupNode(BaseNodeController nodeController)
        {
            var concreteNode = (T)nodeController.Node;
            SetupConcreteNode(concreteNode);
        }

        protected abstract void SetupConcreteNode(T concreteNode);
    }
}
