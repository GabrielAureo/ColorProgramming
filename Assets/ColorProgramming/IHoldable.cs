using System.Collections;
using UnityEngine;

namespace ColorProgramming
{
    public interface IHoldable : IARInteractable
    {
        public void OnHold();
    }
}
