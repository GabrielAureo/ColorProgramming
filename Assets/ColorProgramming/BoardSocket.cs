using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.ColorProgramming
{
    public class BoardSocket : MonoBehaviour, IMovableReceiver
    {
        [SerializeField]
        private Movable[] initialMovables;

        private List<Movable> movables;

        private void Awake()
        {
            movables = new List<Movable>(initialMovables);
        }

        public void OnPlace(ARTouchData touchData, Movable movable)
        {
            movable.transform.localPosition = touchData.hit.point;
            if (!movables.Contains(movable))
            {
                movables.Add(movable);
            }
        }

        public bool ShouldPlace(Movable movable)
        {
            return true;
        }

        public void OnRelease()
        {
            throw new System.NotImplementedException();
        }
    }
}