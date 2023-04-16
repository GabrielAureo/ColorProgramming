using Assets.ColorProgramming;
using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

public interface IMovableProvider : IHoldable
{
    public bool ShouldTake();
    public Movable GetMovable(ARTouchData touchData);
    public void OnInvalidDrop();

}