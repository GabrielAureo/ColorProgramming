using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.ColorProgramming
{
    public class BoardSocket : BaseSocket
    {
        [SerializeField]
        private Movable[] initialMovables;

        private List<Movable> movables;

        private void Awake()
        {
            movables = new List<Movable>(initialMovables);
        }
        public override Movable GetMovable(ARTouchData touchData)
        {
            RaycastHit[] hits;
            hits = Physics.RaycastAll(touchData.ray, Mathf.Infinity, 1 << LayerMask.NameToLayer("Default"));
            foreach (var hit in hits)
            {
                var movable = hit.transform.GetComponent<Movable>();
                if (movable != null)
                {
                    movables.Remove(movable);
                    return movable;
                }
            }
            return null;
        }

        protected override void OnPlace(ARTouchData touchData, Movable movable)
        {
            movable.transform.localPosition = touchData.hit.point;
            if (!movables.Contains(movable))
            {
                movables.Add(movable);
            }
        }

        protected override bool ShouldPlace( Movable movable)
        {
            return true;
        }

        protected override bool TakeOperation()
        {
            return true;
        }
    }
}