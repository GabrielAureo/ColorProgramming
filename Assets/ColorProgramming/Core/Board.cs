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

        public bool IsTraversable { get; private set; }

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

        private void TraversableUpdate()
        {
            IsTraversable = HasPath();
        }

        // TODO : Fix this to consider edges as bidirectional

        public bool HasPath()
        {
            if (!AdjacencyList.ContainsKey(sourceNode) || !AdjacencyList.ContainsKey(targetNode))
                return false;

            HashSet<Node> visited = new HashSet<Node>();
            Queue<Node> queue = new Queue<Node>();
            queue.Enqueue(sourceNode);

            while (queue.Count > 0)
            {
                Node currentNode = queue.Dequeue();

                if (currentNode.Equals(targetNode))
                    return true;

                visited.Add(currentNode);

                if (AdjacencyList.ContainsKey(currentNode))
                {
                    foreach (Edge edge in AdjacencyList[currentNode])
                    {
                        if (!visited.Contains(edge.To))
                            queue.Enqueue(edge.To);
                    }
                }
            }

            return false;
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
            AdjacencyList[edge.From].Add(edge);

            TraversableUpdate();

            return edge;
        }

        public Edge RemoveConnection(Node from, Node to)
        {
            CheckIfNodesPresent(from, to);

            List<Edge> edges = AdjacencyList[from];
            Edge edgeToRemove = edges.Find(e => e.To.Equals(to));

            if (edgeToRemove != null)
            {
                edges.Remove(edgeToRemove);
                TraversableUpdate();
                return edgeToRemove;
            }
            else
            {
                throw new InvalidOperationException("Edge not found in Board");
            }
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

        private void CheckIfEdgesPresent(params Edge[] edges)
        {
            foreach (Edge edge in edges)
            {
                if (
                    !AdjacencyList.ContainsKey(edge.From)
                    || !AdjacencyList[edge.From].Contains(edge)
                )
                {
                    throw new InvalidOperationException("Edge not found in Board");
                }
            }
        }
    }
}
