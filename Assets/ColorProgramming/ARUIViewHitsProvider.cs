using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Assets.ColorProgramming
{
    public class ARUIViewHitsProvider : IARHitsProvider
    {
        public RaycastHit[] GetHits(ref ARTouchData touchData)
        {
            PointerEventData pointerData = new PointerEventData(EventSystem.current)
            {
                pointerId = -1,
            };

            pointerData.position = Input.mousePosition;
            List<RaycastResult> results = new List<RaycastResult>();
            EventSystem.current.RaycastAll(pointerData, results);
            return new RaycastHit[0];
        }
    }
}