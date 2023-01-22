using System;

namespace ColorProgramming
{
    public abstract class Node
    {
        public Guid Id { get; private set; }
        public Node Connected { get; private set; }

        protected Node()
        {
            Id = Guid.NewGuid();
        }

        public override bool Equals(object obj)
        {
            if (obj == null)
            {
                return false;
            }
            if (ReferenceEquals(this, obj))
            {
                return true;
            }

            if (GetType() != obj.GetType())
            {
                return false;
            }

            Node other = (Node)obj;
            return Id == other.Id;
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }

        public void SetConnection(Node other)
        {
            this.Connected = other;
        }
        public void UnsetConnection()
        {
            this.Connected = null;
        }

    }
}