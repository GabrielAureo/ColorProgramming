using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace ColorProgramming
{
    public interface IARInteractable { }

    public interface ITappable : IARInteractable { }

    public interface IHoldable : IARInteractable { }

    public interface IReleasable : IARInteractable { }
}
