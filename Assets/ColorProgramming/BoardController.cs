using ColorProgramming.Core;
using ColorProgramming.Items;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace ColorProgramming
{
    public class BoardController : MonoBehaviour
    {
        private Board board;

        private BoardScope globalScope;
        private Dictionary<CapsuleNode, BoardScope> scopes;
        private BoardScope currentScope;

        private AgentController player;
        private TargetNodeController target;

        private LoopNode loopNodeScope;

        [Header("Prefabs")]
        public GameObject edgePrefab;

        public bool isGlobalScope;

        public void WalkGraph()
        {
            if (!board.IsTraversable)
                return;

            var controllers = globalScope.NodeControllers
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

        public void ToggleLoopBuildMode(LoopNode loopNode)
        {
            loopNodeScope = loopNodeScope != null ? null : loopNode;
        }

        public void SetBoard()
        {
            var initialNodes = FindObjectsByType<ProgrammingNodeController>(
                FindObjectsSortMode.None
            );

            player = FindAnyObjectByType<AgentController>();

            target = FindAnyObjectByType<TargetNodeController>();

            board = new Board(player.Node, target.Node);

            globalScope = new BoardScope();
            currentScope = globalScope;
            scopes = new();

            foreach (var node in initialNodes)
            {
                AddNode(node);
            }
            AddNode(player);
            AddNode(target);
            isGlobalScope = true;
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
                currentScope.NodeControllers.Add(nodeController);
            }

            return nodeController;
        }

        public BaseNodeController AddNode(BaseNodeController instantiatedNodeController)
        {
            board.AddNode(instantiatedNodeController.Node);
            currentScope.NodeControllers.Add(instantiatedNodeController);
            return instantiatedNodeController;
        }

        public void ConnectNodes(BaseNodeController from, BaseNodeController to)
        {
            var edge = CreateEdge(to, from);
            var edgeObj = Instantiate(edgePrefab);
            var edgeController = edgeObj.GetComponent<EdgeController>();
            edgeController.FromNodeController = from;
            edgeController.ToNodeController = to;
            edgeController.Edge = edge;

            currentScope.EdgeControllers.Add(edgeController);
        }

        public void DisconnectNodes(BaseNodeController node, BaseNodeController other)
        {
            var removed = board.RemoveConnection(node.Node, other.Node);

            var edgeObj = currentScope.EdgeControllers.FirstOrDefault((e) => e.Edge == removed);
            Destroy(edgeObj.gameObject);
        }

        private Edge CreateEdge(BaseNodeController from, BaseNodeController to)
        {
            return loopNodeScope != null
                ? board.ConnectLoop(from.Node, to.Node, loopNodeScope)
                : board.ConnectNodes(from.Node, to.Node);
        }

        public void SetScope()
        {
            currentScope.HideScope();
            currentScope = globalScope;
            currentScope.ShowScope();
            isGlobalScope = true;

            GameManager.Instance.InventoryController.ClearTemporaryItems();
        }

        public void SetScope(CapsuleNode capsuleScope)
        {
            if (!scopes.TryGetValue(capsuleScope, out var scope))
            {
                scope = new BoardScope();
                scopes.Add(capsuleScope, scope);
            }

            currentScope.HideScope();
            currentScope = scope;
            currentScope.ShowScope();
            isGlobalScope = false;

            var newItems = EvaluateScopeItems(capsuleScope);

            GameManager.Instance.InventoryController.AddTemporaryItems(newItems);
        }

        private List<Item> EvaluateScopeItems(CapsuleNode capsuleScope)
        {
            GameObject inputNodePrefab = GameManager.Instance.NodePrefabCollection.Data[
                "InputNode"
            ];
            GameObject outputNodePrefab = GameManager.Instance.NodePrefabCollection.Data[
                "OutputNode"
            ];

            List<Item> result = new();

            var hasAdjacencyList = board.ScopeBodies.TryGetValue(
                capsuleScope,
                out var scopeAdjacencyList
            );

            if (!hasAdjacencyList)
            {
                return new List<Item>()
                {
                    new ConcreteItem<InputNode>()
                    {
                        NodeGameObject = inputNodePrefab,
                        ItemQuantity = 1
                    },
                    new ConcreteItem<OutputNode>()
                    {
                        NodeGameObject = outputNodePrefab,
                        ItemQuantity = 1
                    }
                };
            }

            if (scopeAdjacencyList.SourceNode == null)
            {
                result.Add(
                    new ConcreteItem<InputNode>()
                    {
                        NodeGameObject = inputNodePrefab,
                        ItemQuantity = 1
                    }
                );
            }

            if (scopeAdjacencyList.TargetNode == null)
            {
                result.Add(
                    new ConcreteItem<OutputNode>()
                    {
                        NodeGameObject = outputNodePrefab,
                        ItemQuantity = 1
                    }
                );
            }

            return result;
        }
    }
}
