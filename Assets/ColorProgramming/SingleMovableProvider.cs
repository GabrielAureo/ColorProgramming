using System.Collections;
using UnityEngine;

namespace ColorProgramming
{
    [RequireComponent(typeof(Movable))]
    public class SingleMovableProvider : MonoBehaviour, IMovableProvider
    {
        [SerializeField]
        private Movable movable;

        public Movable TakeMovable(ARTouchData touchData)
        {
            movable.OnTake();
            return movable;
        }

        public void ReturnToOrigin()
        {
            movable.SetPose(movable.LastPose);
        }
    }
}
