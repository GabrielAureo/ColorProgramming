using ColorProgramming.Core;
using UnityEngine;

namespace ColorProgramming
{
    public abstract class ConcreteNodeController<T> : ProgrammingNodeController
        where T : Node
    {
        public T ConcreteNode;

        public override Node Node
        {
            get => ConcreteNode;
            set => ConcreteNode = (T)value;
        }
    }
}
