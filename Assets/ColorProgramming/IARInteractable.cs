using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public interface IARInteractable{
    void OnTap();
    void OnHold();
    void OnRelease();
}
