using UnityEngine;

namespace ColorProgramming
{
    public class EdgeController : MonoBehaviour
    {
        [HideInInspector]
        public GameObject connectedObject1;

        [HideInInspector]
        public GameObject connectedObject2;
        private LineRenderer lineRenderer;

        void Start()
        {
            lineRenderer = GetComponent<LineRenderer>();
        }

        void Update()
        {
            UpdateLineRenderer();
        }

        private void UpdateLineRenderer()
        {
            lineRenderer.SetPosition(0, connectedObject1.transform.position);
            lineRenderer.SetPosition(1, connectedObject2.transform.position);
        }
    }
}
