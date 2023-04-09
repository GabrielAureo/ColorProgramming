using UnityEngine;

namespace Assets.ColorProgramming
{
    public interface IARHitsProvider
    {
        RaycastHit[] GetHits(ref ARTouchData touchData);
    }
}