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

        private readonly List<EdgeController> edgeControllers = new();
        private readonly List<BaseNodeController> nodeControllers = new();
        private AgentController player;
        private TargetNodeController target;

        [Header("Prefabs")]
        public GameObject edgePrefab;

        public void WalkGraph()
        {
            if (!board.IsTraversable)
                return;

            var controllers = nodeControllers
                .Select((controller, index) => new { Controller = controller, Index = index })
                .Join(
                    board.Path.Select((node, index) => new { Node = node, Index = index }),
                    entry => entry.Controller.Node.Id,
                    node => node.Node.Id,
                    (entry, node) =>
                        new
                        {
                            entry.Controller,
                            node.Node,
                            node.Index
                        }
                )
                .OrderBy(entry => entry.Index) // Sort based on the index in the board.Path
                .Select(entry => entry.Controller)
                .ToList();

            player.WalkGraph(controllers);
        }

        public void SetBoard()
        {
            var initialNodes = FindObjectsByType<ProgrammingNodeController>(
                FindObjectsSortMode.None
            );

            player = FindAnyObjectByType<AgentController>();

            target = FindAnyObjectByType<TargetNodeController>();

            board = new Board(player.Node, target.Node);

            foreach (var node in initialNodes)
            {
                AddNode(node);
            }
            AddNode(player);
            AddNode(target);
        }

        public Board GetCurrentBoard()
        {
            return board;
        }

        public BaseNodeController AddNode(GameObject nodePrefab)
        {
            var nodeObj = Instantiate(nodePrefab);
            if (nodeObj.TryGetComponent<BaseNodeController>(out var nodeController))
            {
                board.AddNode(nodeController.Node);
                nodeControllers.Add(nodeController);
            }

            return nodeController;
        }

        public BaseNodeController AddNode(BaseNodeController instantiatedNodeController)
        {
            board.AddNode(instantiatedNodeController.Node);
            nodeControllers.Add(instantiatedNodeController);
            return instantiatedNodeController;
        }

        public void ConnectNodes(BaseNodeController from, BaseNodeController to)
        {
            var edge = board.ConnectNodes(from.Node, to.Node);
            var edgeObj = Instantiate(edgePrefab);
            var edgeController = edgeObj.GetComponent<EdgeController>();
            edgeController.FromNodeController = from;
            edgeController.ToNodeController = to;
            edgeController.Edge = edge;

            edgeControllers.Add(edgeController);
        }

        public void DisconnectNodes(BaseNodeController node, BaseNodeController other)
        {
            var removed = board.RemoveConnection(node.Node, other.Node);

            var edgeObj = edgeControllers.FirstOrDefault((e) => e.Edge == removed);
            Destroy(edgeObj.gameObject);
        }
    }
}
