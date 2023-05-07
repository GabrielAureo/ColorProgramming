using ColorProgramming.Core;
using System.Collections;
using UnityEngine;

namespace ColorProgramming
{
    public class NodeConnectService : ARTouchService
    {
        public NodeConnectService(bool isExclusive)
            : base(isExclusive) { }

        public override void OnHold(ARTouchData touchData) { }

        public override void OnRelease(ARTouchData touchData) { }

        public override void OnTap(ARTouchData touchData)
        {
            throw new System.NotImplementedException();
        }
    }
}
