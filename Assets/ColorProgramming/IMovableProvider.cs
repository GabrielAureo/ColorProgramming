using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

namespace ColorProgramming
{
    public interface IMovableProvider : IHoldable
    {
        public Movable TakeMovable(ARTouchData touchData);
        public void ReturnToOrigin();
    }
}
