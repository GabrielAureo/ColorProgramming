using System.Collections;
using UnityEngine;

namespace Assets.ColorProgramming
{
    public class ARGameViewHitsProvider : IARHitsProvider
    {
        public RaycastHit[] GetHits(ref ARTouchData touchData)
        {
            var ray = CameraRay();
            touchData.ray = ray;
            return Physics.RaycastAll(ray, Mathf.Infinity, 1 << LayerMask.NameToLayer("Default"));
        }

        private Ray CameraRay()
        {
            Vector2 inputPosition = Input.mousePosition;
#if UNITY_ANDROID && !UNITY_EDITOR
            inputPosition = Input.touches[0].position;
#endif

            var wrldPos = Camera.main.ScreenToWorldPoint(new Vector3(inputPosition.x, inputPosition.y, 1.35f));
            //Debug.Log(wrldPos);
            //transform.position = new Vector3(wrldPos.x, wrldPos.y, wrldPos.z);
            Ray ray = Camera.main.ScreenPointToRay(inputPosition);
            return ray;

        }


    }
}