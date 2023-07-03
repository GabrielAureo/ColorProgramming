using ColorProgramming.Core;
using UnityEngine;

namespace ColorProgramming
{
    public class EdgeController : MonoBehaviour
    {
        public Edge Edge;

        public BaseNodeController FromNodeController;
        public BaseNodeController ToNodeController;

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
            lineRenderer.SetPosition(0, FromNodeController.transform.position);
            lineRenderer.SetPosition(1, ToNodeController.transform.position);
        }
    }
}
