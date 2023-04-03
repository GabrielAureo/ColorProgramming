using System;

public interface ITouchController{
    event Action<ARTouchData> OnTouch;
    event Action<ARTouchData> OnHold;
    event Action<ARTouchData> OnRelease;
}