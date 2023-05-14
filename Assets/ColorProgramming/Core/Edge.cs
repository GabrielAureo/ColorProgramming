using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ColorProgramming.Core
{
    public class Edge
    {
        public Node Node { get; private set; }
        public Node Other { get; private set; }

        public Edge(Node node, Node other)
        {
            this.Node = node;
            this.Other = other;
        }

        public Guid Id { get; private set; }
    }
}
