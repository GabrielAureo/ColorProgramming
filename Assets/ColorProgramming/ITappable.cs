using System.Collections;
using UnityEngine;

namespace ColorProgramming
{
    public interface ITappable : IARInteractable
    {
        public void OnTap();
    }
}
