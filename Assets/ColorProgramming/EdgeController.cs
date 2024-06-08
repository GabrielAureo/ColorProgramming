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

        [SerializeField] private Material normalMaterial;
        [SerializeField] private Material loopMaterial;

        public int segmentCount = 50;

        void Start()
        {
            lineRenderer = GetComponent<LineRenderer>();
            var material = Edge.IsLoop ? loopMaterial : normalMaterial;
            lineRenderer.material = material;
        }

        void Update()
        {

            if (GameManager.Instance.BoardController.IsWalking)
            {
                return;
            }
            if (Edge.IsLoop)
            {
                RenderBezierLine();

            }
            else
            {
                RenderStraightLine();

            }

        }

        private void RenderStraightLine()
        {
            lineRenderer.SetPosition(0, FromNodeController.transform.position);
            lineRenderer.SetPosition(1, ToNodeController.transform.position);
        }

        private void RenderBezierLine()
        {
            Vector3 P0 = FromNodeController.transform.position;
            Vector3 P3 = ToNodeController.transform.position;

            // Calculate direction and orthogonal direction
            Vector3 direction = P3 - P0;
            Vector3 orthogonalDirection = Vector3.Cross(direction, Vector3.up).normalized * direction.magnitude / 2;

            // Control points
            Vector3 P1 = P0 + orthogonalDirection;
            Vector3 P2 = P3 + orthogonalDirection;

            // Set the number of points in the LineRenderer
            lineRenderer.positionCount = segmentCount;

            // Calculate points on the Bezier curve
            for (int i = 0; i < segmentCount; i++)
            {
                float t = i / (float)(segmentCount - 1);
                Vector3 pointOnCurve = CalculateBezierPoint(t, P0, P1, P2, P3);
                lineRenderer.SetPosition(i, pointOnCurve);
            }
        }

        Vector3 CalculateBezierPoint(float t, Vector3 P0, Vector3 P1, Vector3 P2, Vector3 P3)
        {
            float u = 1 - t;
            float tt = t * t;
            float uu = u * u;
            float uuu = uu * u;
            float ttt = tt * t;

            Vector3 point = uuu * P0; // (1-t)^3 * P0
            point += 3 * uu * t * P1; // 3 * (1-t)^2 * t * P1
            point += 3 * u * tt * P2; // 3 * (1-t) * t^2 * P2
            point += ttt * P3; // t^3 * P3

            return point;
        }

    }
}
