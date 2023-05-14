using System.Collections;
using UnityEngine;

namespace ColorProgramming
{
    [RequireComponent(typeof(Movable))]
    public class SingleMovableProvider : MonoBehaviour, IMovableProvider
    {
        [SerializeField]
        private Movable movable;

        public Movable GetMovable(ARTouchData touchData)
        {
            return movable;
        }

        public void OnInvalidDrop()
        {
            throw new System.NotImplementedException();
        }

        public bool ShouldTake()
        {
            return true;
        }
    }
}
