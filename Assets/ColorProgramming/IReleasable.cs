using System.Collections;
using UnityEngine;

namespace ColorProgramming
{
    public interface IReleasable : IARInteractable
    {
        public void OnRelease();
    }
}
