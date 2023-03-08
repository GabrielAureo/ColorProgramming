using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using UnityEditor.Experimental.GraphView;

namespace ColorProgramming
{
    public class Board
    {
        public List<Node> Nodes { get; private set; }

        public Board()
        {
            this.Nodes = new List<Node>();
        }

        public enum BoardChangedAction
        {
            ADD, CONNECT, DISCONNECT, REMOVE
        }

        public delegate void BoardChangeEventHandler(BoardNotification boardNotification);

        public event BoardChangeEventHandler NodesChanged;

        protected void OnCollectionChanged(BoardNotification eventArgs)
        {
            BoardChangeEventHandler handler = NodesChanged;
            if (handler != null)
            {
                handler(eventArgs);
            }

        }

        public void AddNode(Node node)
        {
            Nodes.Add(node);
            OnCollectionChanged( new BoardNotification(this, BoardChangedAction.ADD, node) );
        }

        public void ConnectNodes(Node from, Node to)
        {
            CheckIfNodesPresent(from, to);
            from.SetConnection(to);

            OnCollectionChanged(new BoardNotification(this, BoardChangedAction.CONNECT, from, to));
        }

        public void RemoveConnection(Node from)
        {
            CheckIfNodesPresent(from);
            from.UnsetConnection();
            OnCollectionChanged(new BoardNotification(this, BoardChangedAction.DISCONNECT, from));
        }

        public void RemoveNode(Node node)
        {
            CheckIfNodesPresent(node);
            Nodes.Remove(node);
            OnCollectionChanged(new BoardNotification(this, BoardChangedAction.REMOVE, node));
        }

        private void CheckIfNodesPresent(params Node[] nodes)
        {
            foreach (var node in nodes)
            {
                if (!Nodes.Contains(node))
                {
                    throw new InvalidOperationException("Node not in Board");
                }
            }
        }


    }
}