using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;

namespace ColorProgramming.Core
{
    public class AdjacencyList : Dictionary<Node, List<Edge>>
    {
        public Node SourceNode { get; set; }
        public Node TargetNode { get; set; }
    }

    public class Board
    {
        public AdjacencyList AdjacencyList { get; private set; }
        public Dictionary<LoopNode, AdjacencyList> LoopBodies { get; private set; }

        public Dictionary<Guid, AdjacencyList> ScopeBodies { get; private set; }

        public bool IsTraversable
        {
            get { return Path != null && Path.Count > 0; }
        }

        public Board(Node sourceNode, Node targetNode)
        {
            AdjacencyList = new AdjacencyList()
            {
                { sourceNode, new List<Edge>() },
                { targetNode, new List<Edge>() },
            };
            AdjacencyList.SourceNode = sourceNode;
            AdjacencyList.TargetNode = targetNode;

            LoopBodies = new();
            ScopeBodies = new();
        }

        public List<Node> Path;
        public List<int> PathLoopIndexes;

        private void TraversableUpdate()
        {
            PathLoopIndexes = new List<int>();
            if (HasPath(out var path))
            {
                Path = path;
            }
            else
            {
                Path = null;
            }
        }

        public bool HasPath(out List<Node> path)
        {
            var rootPath = BuildPath(AdjacencyList);

            if (rootPath == null)
            {
                path = null;
                return false;
            }

            var flattenedPath = new List<Node>();

            foreach (var node in rootPath)
            {
                if (node is CapsuleNode capsuleNode)
                {
                    if (!ScopeBodies.TryGetValue(capsuleNode.ScopeKey, out var capsuleBody))
                    {
                        capsuleNode.InvalidMessage = "Must have a Portal In and Out inside it";

                        path = null;
                        return false;
                    }
                    if (capsuleBody.SourceNode == null || capsuleBody.TargetNode == null)
                    {
                        capsuleNode.InvalidMessage = "Must have a Portal In and Out inside it";
                        path = null;
                        return false;
                    }

                    capsuleNode.InvalidMessage = "";
                }

                if (node is LoopNode loopNode && LoopBodies.TryGetValue(loopNode, out var loopBody))
                {

                    var loopPath = BuildPath(loopBody);
                    // Add loop node to beginning and end of list
                    loopPath.Insert(0, loopNode);

                    loopPath = Enumerable
                    .Repeat(loopPath, loopNode.TotalLoops)
                    .SelectMany(x => x)
                    .ToList();

                    loopPath.Add(node);
                    flattenedPath.AddRange(loopPath);

                    var loopIndexes = Enumerable.Range(flattenedPath.Count - loopPath.Count + 1, flattenedPath.Count - 2);
                    PathLoopIndexes.AddRange(loopIndexes);

                }
                else
                {
                    flattenedPath.Add(node);
                }
            }

            flattenedPath.Reverse();
            path = flattenedPath;

            return path.Count > 0;
        }

        public List<Node> BuildPath(AdjacencyList adjacencyList)
        {
            Dictionary<Node, Node> previousNodes = new();
            HashSet<Node> visited = new();
            Queue<Node> queue = new();
            queue.Enqueue(adjacencyList.SourceNode);

            while (queue.Count > 0)
            {
                Node currentNode = queue.Dequeue();

                if (currentNode.Equals(adjacencyList.TargetNode))
                {
                    // Reconstruct the path from target to source
                    List<Node> path = new();
                    Node node = adjacencyList.TargetNode;
                    while (node != null)
                    {
                        path.Add(node);
                        node = previousNodes.ContainsKey(node) ? previousNodes[node] : null;
                    }

                    return path;
                }

                visited.Add(currentNode);

                if (adjacencyList.ContainsKey(currentNode))
                {
                    foreach (Edge edge in adjacencyList[currentNode])
                    {
                        if (!visited.Contains(edge.To))
                        {
                            queue.Enqueue(edge.To);
                            // Set the previous node for the neighbor
                            previousNodes[edge.To] = currentNode;
                        }
                    }
                }

                // Consider bidirectional edges
                foreach (var nodeEdges in adjacencyList)
                {
                    Node node = nodeEdges.Key;
                    List<Edge> edges = nodeEdges.Value;

                    foreach (Edge edge in edges)
                    {
                        if (edge.To.Equals(currentNode) && !visited.Contains(node))
                        {
                            queue.Enqueue(node);
                            // Set the previous node for the neighbor
                            previousNodes[node] = currentNode;
                        }
                    }
                }
            }

            return null; // No path found
        }

        public void AddNode(Node node, CapsuleNode scope = null)
        {
            AdjacencyList adjacencyList = AdjacencyList;
            if (scope != null)
            {
                adjacencyList = FindOrCreateScope(scope);

                if (node is InputNode)
                {
                    adjacencyList.SourceNode = node;
                }
                if (node is OutputNode)
                {
                    adjacencyList.TargetNode = node;
                }
            }
            if (!adjacencyList.ContainsKey(node))
                adjacencyList[node] = new List<Edge>();
        }

        public Edge ConnectNodes(Node from, Node to, CapsuleNode scope = null)
        {
            if (scope != null)
            {
                var capsuleScope = FindOrCreateScope(scope);
                return Connect(from, to, capsuleScope);
            }
            return Connect(from, to, AdjacencyList);
        }

        public Edge ConnectLoop(Node from, Node to, LoopNode scope)
        {
            if (!LoopBodies.ContainsKey(scope))
            {
                LoopBodies[scope] = new AdjacencyList();
            }

            if (!LoopBodies[scope].ContainsKey(from))
            {
                LoopBodies[scope][from] = new List<Edge>();
            }
            if (!LoopBodies[scope].ContainsKey(to))
            {
                LoopBodies[scope][to] = new List<Edge>();
            }

            var edge = Connect(from, to, LoopBodies[scope]);
            edge.IsLoop = true;

            Node inOutLoopNode = null;

            // if edge is connected to loop node, we grab the other

            if (edge.To == scope)
            {
                inOutLoopNode = edge.From;
            }
            else if (edge.From == scope)
            {
                inOutLoopNode = edge.To;
            }

            if (inOutLoopNode != null)
            {
                if (LoopBodies[scope].SourceNode == null)
                {
                    LoopBodies[scope].SourceNode = inOutLoopNode;
                    if (inOutLoopNode == edge.To)
                        (edge.To, edge.From) = (edge.From, edge.To);
                }
                else if (LoopBodies[scope].TargetNode == null)
                {
                    LoopBodies[scope].TargetNode = inOutLoopNode;
                    if (inOutLoopNode == edge.From)
                        (edge.To, edge.From) = (edge.From, edge.To);
                }
                LoopBodies[scope][edge.From].Remove(edge);
            }

            return edge;
        }

        private Edge Connect(Node from, Node to, AdjacencyList adjacencyList)
        {
            CheckIfNodesPresent(adjacencyList, from, to);

            var edge = new Edge(from, to);
            CheckIfEdgeAlreadyExists(edge, adjacencyList);

            if (!adjacencyList.ContainsKey(edge.From))
                adjacencyList[edge.From] = new List<Edge>();

            adjacencyList[edge.From].Add(edge);
            TraversableUpdate();

            return edge;
        }

        public Edge RemoveConnection(Node from, Node to, CapsuleNode scope = null)
        {
            if (scope != null)
            {
                return RemoveEdge(from, to, ScopeBodies[scope.ScopeKey]);
            }
            return RemoveEdge(from, to, AdjacencyList);
        }

        public Edge RemoveLoop(Node from, Node to, LoopNode scope)
        {

            return RemoveEdge(from, to, LoopBodies[scope]);
        }

        private Edge RemoveEdge(Node from, Node to, AdjacencyList adjacencyList)
        {
            CheckIfNodesPresent(adjacencyList, from, to);

            Edge edgeToRemove = adjacencyList[from].Find(e => e.To.Equals(to));

            if (edgeToRemove != null)
            {
                adjacencyList[from].Remove(edgeToRemove);
                TraversableUpdate();
                return edgeToRemove;
            }

            edgeToRemove ??= adjacencyList[to].Find(e => e.To.Equals(from));

            if (edgeToRemove != null)
            {
                adjacencyList[to].Remove(edgeToRemove);
                TraversableUpdate();
                return edgeToRemove;
            }

            throw new InvalidOperationException("Edge not found in Board");
        }

        public void RemoveNode(Node node, AdjacencyList adjacencyList)
        {
            CheckIfNodesPresent(adjacencyList, node);
            List<Edge> edgesToRemove = AdjacencyList[node];
            foreach (Edge edge in edgesToRemove)
            {
                AdjacencyList[edge.From].Remove(edge);
            }

            // Remove the node from the adjacency list
            AdjacencyList.Remove(node);
            TraversableUpdate();
        }

        private AdjacencyList FindOrCreateScope(CapsuleNode scope)
        {
            if (!ScopeBodies.TryGetValue(scope.ScopeKey, out var capsuleScope))
            {
                ScopeBodies[scope.ScopeKey] = new AdjacencyList();
                capsuleScope = ScopeBodies[scope.ScopeKey];
            }
            return capsuleScope;
        }

        private void CheckIfNodesPresent(AdjacencyList adjacencyList, params Node[] nodes)
        {
            foreach (Node node in nodes)
            {
                if (!adjacencyList.ContainsKey(node))
                {
                    throw new InvalidOperationException("Node not in Board");
                }
            }
        }

        private void CheckIfEdgeAlreadyExists(Edge edge, Dictionary<Node, List<Edge>> adjacencyList)
        {
            if (adjacencyList.TryGetValue(edge.From, out var fromEdges))
            {
                if (fromEdges.Exists((e) => e.To == edge.To))
                {
                    throw new InvalidOperationException("Edge already exists in the Board");
                }
            }

            if (adjacencyList.TryGetValue(edge.To, out var toEdges))
            {
                if (toEdges.Exists((e) => e.To == edge.From))
                {
                    throw new InvalidOperationException("Edge already exists in the Board");
                }
            }
        }
    }
}
