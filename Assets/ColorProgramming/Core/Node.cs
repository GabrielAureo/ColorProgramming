using System;
using System.Collections.Generic;
using UnityEngine;

namespace ColorProgramming.Core
{
    [Serializable]
    public abstract class Node
    {
        public Guid Id { get; private set; }

        protected Node()
        {
            Id = Guid.NewGuid();
        }

        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
                return false;

            Node other = (Node)obj;
            return Id.Equals(other.Id);
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }
    }
}
