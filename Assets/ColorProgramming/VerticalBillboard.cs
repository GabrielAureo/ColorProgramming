using UnityEngine;

namespace ColorProgramming
{
    [ExecuteAlways]
    public class VerticalBillboard : MonoBehaviour
    {


        private void Update()
        {
            if (!Camera.main) return;
            transform.LookAt(Camera.main.transform);
            transform.rotation = Quaternion.Euler(0, transform.rotation.eulerAngles.y, 0);
        }
    }
}

