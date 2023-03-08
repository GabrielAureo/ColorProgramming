using ColorProgramming;
using static ColorProgramming.Board;

public class BoardNotification
{
    public Board Board { get; }
    public BoardChangedAction Action { get; }
    public Node From { get; }
    public Node To { get; }

    public BoardNotification(Board board, BoardChangedAction action, Node node)
    {
        Board = board;
        Action = action;
        From = node;
        To = null;
    }

    public BoardNotification(Board board, BoardChangedAction action, Node from, Node to)
    {
        Board = board;
        Action = action;
        From = from;
        To = to;
    }

    public override string ToString()
    {
        string fromNode = (From == null) ? "null" : From.ToString();
        string toNode = (To == null) ? "null" : To.ToString();
        string board = (Board == null) ? "null" : Board.ToString();
        return $"Board: {board}, Action: {Action}, From: {fromNode}, To: {toNode}";
    }
}