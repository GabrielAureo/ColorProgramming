using System;
using ColorProgramming.Core;
using UnityEngine;

namespace ColorProgramming.View
{
    public class BoardView : MonoBehaviour
    {
        public Board Board { get; private set; }

        [SerializeField]
        private Graphics boardPanel;
        private void EvalBoardUpdate(BoardNotification notification)
        {
            switch(notification.Action)
            {
                case Board.BoardChangedAction.ADD:
                    break;
                case Board.BoardChangedAction.REMOVE:
                    break;
                case Board.BoardChangedAction.DISCONNECT: 
                    break;
                case Board.BoardChangedAction.CONNECT:
                    break;
                default:
                    break;
            }
        }
           
        

    }
}