using ColorProgramming.Core;
using ColorProgramming.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace ColorProgramming
{
    public class BoardController : MonoBehaviour
    {
        private Board board;

        private BoardScope globalScope;
        private Dictionary<Node, BoardScope> scopes;
        public IReadOnlyDictionary<Node, BoardScope> Scopes => scopes;
        private BoardScope CurrentScope
        {
            get
            {
                return currentScopeKey == null ? globalScope : scopes[currentScopeKey];
            }
        }
        private Node currentScopeKey;

        private AgentController player;
        private TargetNodeController target;

        [Header("Prefabs")]
        public GameObject edgePrefab;

        public bool isGlobalScope;
        public bool IsWalking { get; private set; }

        private InventoryController InventoryController => _inventoryController ?? FindObjectOfType<InventoryController>();
        private InventoryController _inventoryController;

        public void WalkGraph()
        {
            if (!board.IsTraversable)
                return;

            var rootPath = ScopeToPath(globalScope, board.Path);

            var subGraphs = rootPath.FindAll((controller) => controller is CapsuleNodeController capsuleController && board.ScopeBodies.ContainsKey(capsuleController.ConcreteNode.ScopeKey));
            var capsuleNodes = subGraphs.Cast<CapsuleNodeController>();

            var capsulePaths = board.ScopeBodies.ToDictionary((scope) => scope.Key,
                (scope) => board.BuildPath(scope.Value))
                .ToDictionary((scope) => capsuleNodes.First((capsule) => capsule.ConcreteNode.ScopeKey == scope.Key).ConcreteNode, (scope) => GetPathFromScopeKey(scope.Key, capsuleNodes, scope.Value));

            var path = new AgentPath()
            {
                RootPath = rootPath,
                SubPaths = capsulePaths
            };

            IsWalking = true;

            player.WalkGraph(path);
        }

        private List<BaseNodeController> GetPathFromScopeKey(Guid key, IEnumerable<CapsuleNodeController> capsuleNodes, List<Node> value)
        {
            var capsuleScope = capsuleNodes.First((capsule) => capsule.ConcreteNode.ScopeKey == key);
            return ScopeToPath(scopes[capsuleScope.ConcreteNode], value);

        }

        private List<BaseNodeController> ScopeToPath(BoardScope scope, List<Node> path)
        {
            return scope.NodeControllers
                .Select((controller, index) => new { Controller = controller, Index = index })
                .Join(
                    path.Select((node, index) => new { Node = node, Index = index }),
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
                .OrderBy(entry => entry.Index)
                .Select(entry => entry.Controller)
                .ToList();
        }

        public void ToggleLoopBuildMode(LoopNode loopNode)
        {

            if (currentScopeKey == loopNode)
            {
                isGlobalScope = true;
                currentScopeKey = null;
                return;
            }
            if (!scopes.TryGetValue(loopNode, out var _))
            {
                var scope = new BoardScope();
                scopes.Add(loopNode, scope);
            }
            currentScopeKey = loopNode;
            isGlobalScope = false;

        }

        public void CloseLoopBuildMode()
        {
            ToggleLoopBuildMode((LoopNode)currentScopeKey);
        }

        public void SetBoard(AgentController player, TargetNodeController target)
        {
            var initialNodes = FindObjectsByType<ProgrammingNodeController>(
                FindObjectsSortMode.None
            );

            this.player = player;
            this.target = target;

            board = new Board(player.Node, target.Node);

            globalScope = new BoardScope();
            currentScopeKey = null;
            scopes = new();

            foreach (var node in initialNodes)
            {
                AddNode(node);
            }
            AddNode(player);
            AddNode(target);
            isGlobalScope = true;
            IsWalking = false;
            player.AgentNode.SetElement(player.InitialElement);
        }

        public void ResetBoard()
        {
            board = null;
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
                AddNode(nodeController);
            }

            return nodeController;
        }

        public BaseNodeController AddNode(BaseNodeController instantiatedNodeController)
        {
            if (currentScopeKey is CapsuleNode capsuleNode)
            {
                board.AddNode(instantiatedNodeController.Node, capsuleNode);
            }
            else
            {
                board.AddNode(instantiatedNodeController.Node);
            }
            CurrentScope.NodeControllers.Add(instantiatedNodeController);
            return instantiatedNodeController;
        }

        public void ConnectNodes(BaseNodeController from, BaseNodeController to)
        {
            var edge = CreateEdge(to, from);
            var edgeObj = Instantiate(edgePrefab);
            var edgeController = edgeObj.GetComponent<EdgeController>();

            if (from.Node == edge.To)
            {
                (from, to) = (to, from);
            }
            edgeController.FromNodeController = from;
            edgeController.ToNodeController = to;
            edgeController.Edge = edge;

            CurrentScope.EdgeControllers.Add(edgeController);
        }

        public void DisconnectNodes(BaseNodeController node, BaseNodeController other)
        {
            Edge removed;

            if (currentScopeKey is CapsuleNode capsuleNode)
            {
                removed = board.RemoveConnection(node.Node, other.Node, capsuleNode);
            }
            else if (currentScopeKey is LoopNode loopNode)
            {
                removed = board.RemoveLoop(node.Node, other.Node, loopNode);
            }
            else
            {
                removed = board.RemoveConnection(node.Node, other.Node);
            }

            if (removed == null)
            {
                return;
            }

            var edgeObj = CurrentScope.EdgeControllers.FirstOrDefault((e) => e.Edge == removed);
            Destroy(edgeObj.gameObject);
        }

        private Edge CreateEdge(BaseNodeController from, BaseNodeController to)
        {
            Edge edge;
            if (currentScopeKey is LoopNode loopNode)
            {
                edge = board.ConnectLoop(from.Node, to.Node, loopNode);
            }
            else if (currentScopeKey is CapsuleNode capsuleNode)
            {
                edge = board.ConnectNodes(from.Node, to.Node, capsuleNode);
            }
            else
            {
                edge = board.ConnectNodes(from.Node, to.Node);
            }

            return edge;
        }

        public void SetScope()
        {
            CurrentScope.HideScope();
            currentScopeKey = null;
            CurrentScope.ShowScope();
            isGlobalScope = true;

            InventoryController.ClearTemporaryItems();
        }

        public void SetScope(Node scopeKey)
        {
            if (!scopes.TryGetValue(scopeKey, out var scope))
            {
                scope = new BoardScope();
                scopes.Add(scopeKey, scope);
            }

            CurrentScope.HideScope();
            currentScopeKey = scopeKey;
            CurrentScope.ShowScope();
            isGlobalScope = false;

            if (scopeKey is CapsuleNode capsuleNode)
            {

                var newItems = EvaluateScopeItems(capsuleNode);
                InventoryController.SetTemporaryItems(newItems);
            }

            Toast.Show(null, "Building Capsule", () => SetScope());

        }

        private ItemController[] EvaluateScopeItems(CapsuleNode capsuleScope)
        {
            var inputNodePrefab = GameManager.Instance.NodePrefabCollection.Data[
                "InputNode"
            ];
            var outputNodePrefab = GameManager.Instance.NodePrefabCollection.Data[
                "OutputNode"
            ];

            List<ItemController> result = new();

            var hasAdjacencyList = board.ScopeBodies.TryGetValue(
                capsuleScope.ScopeKey,
                out var scopeAdjacencyList
            );

            if (!hasAdjacencyList)
            {
                return new ItemController[]
                {
                    inputNodePrefab, outputNodePrefab
                };
            }

            if (scopeAdjacencyList.SourceNode == null)
            {
                result.Add(
                    inputNodePrefab
                );
            }

            if (scopeAdjacencyList.TargetNode == null)
            {
                result.Add(
                    outputNodePrefab
                );
            }

            return result.ToArray();
        }

        public Bounds CalculateCapsuleBounds(CapsuleNode capsuleNode)
        {
            var bounds = new Bounds();
            if (!scopes.TryGetValue(capsuleNode, out var scope))
            {
                return bounds;
            }

            foreach (var nodeController in scope.NodeControllers)
            {
                var obj = nodeController.gameObject;

                bounds.Encapsulate(obj.transform.position);

            }

            return bounds;
        }

    }
}
