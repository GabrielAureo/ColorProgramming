using System.Collections;
using UnityEngine;

namespace Assets.ColorProgramming
{
    public interface IReleasable : IARInteractable
    {
        public void OnRelease();

    }
}