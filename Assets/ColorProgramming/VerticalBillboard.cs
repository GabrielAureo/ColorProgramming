using UnityEngine;

namespace ColorProgramming
{
    [ExecuteAlways]
    public class VerticalBillboard : MonoBehaviour
    {
        private Camera _main;

        private void Start()
        {
            _main = Camera.main;
        }

        private void Update()
        {
            Transform transform1;
            (transform1 = transform).LookAt(2 * transform.position - _main.transform.position, Vector3.up);
            var eulerAngles = transform1.eulerAngles;
            eulerAngles.x = 0;
            transform1.localEulerAngles = eulerAngles;
        }
    }
}

