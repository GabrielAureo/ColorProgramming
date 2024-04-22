using System;

namespace ColorProgramming.Core
{
    [Serializable]
    public class CapsuleNode : Node
    {
        public Guid ScopeKey;
        public string InvalidMessage = "";

        public CapsuleNode()
            : base()
        {
        }
    }
}
