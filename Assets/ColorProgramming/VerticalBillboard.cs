using UnityEngine;

namespace ColorProgramming
{
    [ExecuteAlways]
    public class VerticalBillboard : MonoBehaviour
    {


        private void Update()
        {
            Transform transform1;
            (transform1 = transform).LookAt(2 * transform.position - Camera.main.transform.position, Vector3.up);
            var eulerAngles = transform1.eulerAngles;
            eulerAngles.x = 0;
            transform1.localEulerAngles = eulerAngles;
        }
    }
}

