namespace ColorProgramming.Core
{
    public class Item
    {
        public Node Node { get; private set; }

        public Item(Node node)
        {
            this.Node = node;
        }

    }
}