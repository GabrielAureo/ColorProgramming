using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

namespace ColorProgramming
{
    public interface IMovableReceiver : IReleasable
    {
        //public bool TryPlaceObject(ARTouchData touchData, Movable movable)
        //{
        //    var canPlace = ShouldPlace(movable);

        //    if(canPlace)
        //        OnPlace(touchData, movable);
        //    return canPlace;
        //}

        public bool ShouldPlace(Movable movable);

        public void OnPlace(ARTouchData touchData, Movable movable);
    }
}
