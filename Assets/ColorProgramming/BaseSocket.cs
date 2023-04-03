using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

public abstract class BaseSocket : MonoBehaviour
{
    public Movable TryTake()
    {
        var taken = TakeOperation();        
        return taken ? GetMovable() : null;
    }

    protected abstract bool TakeOperation();
    public abstract Movable GetMovable();
    public bool TryPlaceObject(Movable movable)
    {
        var canPlace = ShouldPlace(movable); 

        if(canPlace)       
            OnPlace(movable);
        return canPlace;
    }

    protected abstract bool ShouldPlace(Movable movable);

    protected abstract void OnPlace(Movable movable);
    
}