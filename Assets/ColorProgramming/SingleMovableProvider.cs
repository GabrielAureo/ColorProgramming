using System.Collections;
using UnityEngine;

namespace ColorProgramming
{
    [RequireComponent(typeof(Movable))]
    public class SingleMovableProvider : MonoBehaviour, IMovableProvider, ITappable
    {
        [SerializeField]
        private Movable movable;

        [SerializeField]
        private SingleMovableMenu movableMenu;

        public Movable GetMovable(ARTouchData touchData)
        {
            return movable;
        }

        public void OnHold() { }

        public void OnInvalidDrop()
        {
            throw new System.NotImplementedException();
        }

        public void OnTap()
        {
            throw new System.NotImplementedException();
        }

        public bool ShouldTake()
        {
            return true;
        }
    }
}
