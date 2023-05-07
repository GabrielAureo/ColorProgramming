using UnityEngine;

namespace ColorProgramming
{
    public class NodeConnectController : MonoBehaviour
    {
        [SerializeField]
        private GameObject edgePrefab;

        public void CreateConnection(NodeController node, NodeController other)
        {
            var edgeObj = Instantiate(edgePrefab);
            var edgeController = edgeObj.GetComponent<EdgeController>();
            edgeController.connectedObject1 = node.gameObject;
            edgeController.connectedObject2 = other.gameObject;
        }
    }
}
