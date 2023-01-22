using System;
using System.Collections.Generic;

namespace ColorProgramming
{
    public class Board
    {
        public List<Node> Nodes { get; private set; }

        public Board()
        {
            this.Nodes = new List<Node>();
        }

        public void AddNode(Node node)
        {
            Nodes.Add(node);
        }

        public void ConnectNodes(Node from, Node to)
        {
            CheckIfNodesPresent(from, to);

            from.SetConnection(to);
        }

        public void RemoveConnection(Node from)
        {
            CheckIfNodesPresent(from);

            from.UnsetConnection();
        }

        private void CheckIfNodesPresent(params Node[] nodes)
        {
            foreach (var node in nodes)
            {
                if (!Nodes.Contains(node))
                {
                    throw new InvalidOperationException("Node not in Board");
                }
            }
        }


    }
}