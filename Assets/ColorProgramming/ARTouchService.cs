using System.Collections;
using UnityEngine;

namespace ColorProgramming
{
    public abstract class ARTouchService
    {
        public bool IsExclusive { get; private set; } = false;
        public bool IsEnabled { get; set; } = true;
        public abstract void OnTap(ARTouchData touchData);
        public abstract void OnHold(ARTouchData touchData);
        public abstract void OnRelease(ARTouchData touchData);

        public ARTouchService(bool isExclusive)
        {
            IsExclusive = isExclusive;
        }
    }
}
