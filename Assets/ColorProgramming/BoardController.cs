using ColorProgramming.Core;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace ColorProgramming
{
    public class BoardController : MonoBehaviour
    {
        private BoardSocket boardSocket;
        private Board board;

        private readonly List<EdgeController> edges = new();
        private PlayerNodeController player;
        private TargetNodeController target;

        [SerializeField]
        private NodeController[] initialNodes;

        [Header("Prefabs")]
        public GameObject edgePrefab;

        public void SetBoard()
        {
            player = FindAnyObjectByType<PlayerNodeController>();
            player.Node = new PlayerNode(Element.LIGHTNING);

            target = FindAnyObjectByType<TargetNodeController>();
            target.Node = new TargetNode(Element.LIGHTNING);

            board = new Board(player.Node, target.Node);

            foreach (var node in initialNodes)
            {
                node.Node = new ConditionalNode()
                {
                    CheckedElement = Element.ROCK,
                    TrueElement = Element.FIRE,
                    FalseElement = Element.AIR
                };
                board.AddNode(node.Node);
            }
        }

        public Board GetCurrentBoard()
        {
            return board;
        }

        public NodeController AddNode(GameObject nodePrefab)
        {
            var nodeObj = Instantiate(nodePrefab);
            if (nodeObj.TryGetComponent<NodeController>(out var nodeController))
            {
                board.AddNode(nodeController.Node);
            }

            return nodeController;
        }

        public void ConnectNodes(NodeController from, NodeController to)
        {
            var edge = board.ConnectNodes(from.Node, to.Node);
            var edgeObj = Instantiate(edgePrefab);
            var edgeController = edgeObj.GetComponent<EdgeController>();
            edgeController.FromNodeController = from;
            edgeController.ToNodeController = to;
            edgeController.Edge = edge;

            edges.Add(edgeController);
        }

        public void DisconnectNodes(NodeController node, NodeController other)
        {
            var removed = board.RemoveConnection(node.Node, other.Node);

            var edgeObj = edges.FirstOrDefault((e) => e.Edge == removed);
            Destroy(edgeObj.gameObject);
        }
    }
}
