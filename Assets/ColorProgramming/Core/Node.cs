using System;
using System.Collections.Generic;

namespace ColorProgramming.Core
{
    public abstract class Node
    {
        public Guid Id { get; private set; }

        protected Node()
        {
            Id = Guid.NewGuid();
        }

        //public void SetConnection(Node other, bool createReverse = true)
        //{
        //    Edges.Add(new Edge(this, other));
        //    if (createReverse)
        //        other.SetConnection(this, false);
        //}

        //public void RemoveConnections()
        //{
        //    Edges = new List<Edge> { };
        //}
    }
}
