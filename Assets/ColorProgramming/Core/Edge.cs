using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ColorProgramming.Core
{
    public class Edge
    {
        public Node From { get; private set; }
        public Node To { get; private set; }
        public Guid Id { get; private set; }
        public bool IsLoop { get; set; }

        public Edge(Node from, Node to)
        {
            From = from;
            To = to;
            Id = Guid.NewGuid();
        }

        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
                return false;

            Edge other = (Edge)obj;
            return Id.Equals(other.Id) && From.Equals(other.From) && To.Equals(other.To);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(From, To, Id);
        }
    }
}
