using System.Collections;
using UnityEngine;

namespace Assets.ColorProgramming
{
    public interface IHoldable : IARInteractable
    {
        public void OnHold();

    }
}