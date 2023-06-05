using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

namespace ColorProgramming
{
    public class BoardSocket : MonoBehaviour, IMovableReceiver
    {
        public UnityEvent<Movable[]> OnMovablesPlace;

        public void OnPlace(ARTouchData touchData, Movable movable)
        {
            movable.transform.localPosition = touchData.hit.point;
            movable.transform.rotation = Quaternion.identity;
            OnMovablesPlace.Invoke(new Movable[] { movable });
        }

        public bool ShouldPlace(Movable movable)
        {
            return true;
        }
    }
}
