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

        const int numPoints = 10;

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
            // Sample points along the spline
            Vector3[] splinePoints = new Vector3[numPoints];
            for (int i = 0; i < numPoints; i++)
            {
                float t = i / (float)(numPoints - 1);

                Vector3 v = FromNodeController.transform.position - ToNodeController.transform.position;
                Vector3 controlPoint = FindPerpendicularVector(v);


                splinePoints[i] = CalculateBezierPoint(t, FromNodeController.transform.position, controlPoint, ToNodeController.transform.position);
            }

            Debug.Log(splinePoints.Length);

            // Set spline points to the LineRenderer
            lineRenderer.positionCount = numPoints;
            lineRenderer.SetPositions(splinePoints);
        }

        private Vector3 CalculateBezierPoint(float t, Vector3 p0, Vector3 p1, Vector3 p2)
        {
            float u = 1 - t;
            float tt = t * t;
            float uu = u * u;

            Vector3 p = uu * p0;
            p += 2 * u * t * p1;
            p += tt * p2;

            return p;
        }

        private Vector3 FindPerpendicularVector(Vector3 originalVector)
        {
            Vector3 directionVector = FromNodeController.transform.position - ToNodeController.transform.position;
            // Find an arbitrary vector not parallel to the original vector
            Vector3 arbitraryVector = Vector3.up;

            // Cross product to find the perpendicular vector
            Vector3 perpendicularVector = Vector3.Cross(directionVector.normalized, arbitraryVector).normalized * 3f;

            return perpendicularVector;
        }

    }
}
