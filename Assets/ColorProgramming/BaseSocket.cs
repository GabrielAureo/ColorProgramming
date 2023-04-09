using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

public abstract class BaseSocket : MonoBehaviour, IARInteractable
{
    public Movable TryTake(ARTouchData touchData)
    {
        var taken = TakeOperation();        
        return taken ? GetMovable(touchData) : null;
    }

    protected abstract bool TakeOperation();
    public abstract Movable GetMovable(ARTouchData touchData);
    public bool TryPlaceObject(ARTouchData touchData, Movable movable)
    {
        var canPlace = ShouldPlace(movable); 

        if(canPlace)       
            OnPlace(touchData, movable);
        return canPlace;
    }

    protected abstract bool ShouldPlace( Movable movable);

    protected abstract void OnPlace(ARTouchData touchData, Movable movable);

    public void OnTap()
    {
       
    }

    public void OnHold()
    {
        
    }

    public void OnRelease()
    {
        
    }
}