using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;

namespace ColorProgramming.Core
{
    public class Board
    {
        public Dictionary<Node, List<Edge>> AdjacencyList { get; private set; }

        private Node sourceNode;

        private Node targetNode;

        public bool IsTraversable
        {
            get { return Path != null && Path.Count > 0; }
        }

        public Board(Node sourceNode, Node targetNode)
        {
            this.sourceNode = sourceNode;
            this.targetNode = targetNode;

            AdjacencyList = new Dictionary<Node, List<Edge>>()
            {
                { sourceNode, new List<Edge>() },
                { targetNode, new List<Edge>() },
            };
        }

        public List<Node> Path;

        private void TraversableUpdate()
        {
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
            path = null;

            if (!AdjacencyList.ContainsKey(sourceNode) || !AdjacencyList.ContainsKey(targetNode))
                return false;

            Dictionary<Node, Node> previousNodes = new();
            HashSet<Node> visited = new();
            Queue<Node> queue = new();
            queue.Enqueue(sourceNode);

            while (queue.Count > 0)
            {
                Node currentNode = queue.Dequeue();

                if (currentNode.Equals(targetNode))
                {
                    // Reconstruct the path from target to source
                    path = new List<Node>();
                    Node node = targetNode;
                    while (node != null)
                    {
                        path.Add(node);
                        node = previousNodes.ContainsKey(node) ? previousNodes[node] : null;
                    }
                    path.Reverse();
                    return true;
                }

                visited.Add(currentNode);

                if (AdjacencyList.ContainsKey(currentNode))
                {
                    foreach (Edge edge in AdjacencyList[currentNode])
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
                foreach (var nodeEdges in AdjacencyList)
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

            return false; // No path found
        }

        public void AddNode(Node node)
        {
            if (!AdjacencyList.ContainsKey(node))
                AdjacencyList[node] = new List<Edge>();
        }

        public Edge ConnectNodes(Node from, Node to)
        {
            CheckIfNodesPresent(from, to);

            var edge = new Edge(from, to);
            CheckIfEdgeAlreadyExists(edge);
            AdjacencyList[edge.From].Add(edge);

            TraversableUpdate();

            return edge;
        }

        public Edge RemoveConnection(Node from, Node to)
        {
            CheckIfNodesPresent(from, to);

            Edge edgeToRemove = AdjacencyList[from].Find(e => e.To.Equals(to));

            if (edgeToRemove != null)
            {
                AdjacencyList[from].Remove(edgeToRemove);
                TraversableUpdate();
                return edgeToRemove;
            }

            edgeToRemove ??= AdjacencyList[to].Find(e => e.To.Equals(from));

            if (edgeToRemove != null)
            {
                AdjacencyList[to].Remove(edgeToRemove);
                TraversableUpdate();
                return edgeToRemove;
            }

            throw new InvalidOperationException("Edge not found in Board");
        }

        public void RemoveNode(Node node)
        {
            CheckIfNodesPresent(node);
            List<Edge> edgesToRemove = AdjacencyList[node];
            foreach (Edge edge in edgesToRemove)
            {
                AdjacencyList[edge.From].Remove(edge);
            }

            // Remove the node from the adjacency list
            AdjacencyList.Remove(node);
            TraversableUpdate();
        }

        private void CheckIfNodesPresent(params Node[] nodes)
        {
            foreach (Node node in nodes)
            {
                if (!AdjacencyList.ContainsKey(node))
                {
                    throw new InvalidOperationException("Node not in Board");
                }
            }
        }

        private void CheckIfEdgeAlreadyExists(Edge edge)
        {
            if (
                AdjacencyList[edge.From].Exists((e) => e.To == edge.To)
                || AdjacencyList[edge.To].Exists((e) => e.To == edge.From)
            )
            {
                throw new InvalidOperationException("Edge already exists in Board");
            }
        }
    }
}
