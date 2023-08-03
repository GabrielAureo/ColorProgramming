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

        [SerializeField]
        private Material normalMaterial;

        [SerializeField]
        private Material loopMaterial;

        void Start()
        {
            lineRenderer = GetComponent<LineRenderer>();
            var material = Edge.IsLoop ? loopMaterial : normalMaterial;
            lineRenderer.material = material;
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
