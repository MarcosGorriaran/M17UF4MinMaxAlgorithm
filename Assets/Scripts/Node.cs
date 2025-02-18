using System;
using System.Collections.Generic;

public class Node
{
    private List<Node> _nodeNextStep = new List<Node>();
    private float? _nodeValue = null;
    private MatrixStatus[,] _predictionTable;

    public Node(MatrixStatus[,] ticTacToeStatus, MatrixStatus actualTurnOrder)
    {
        GameResult result = Calculs.EvaluateWin(ticTacToeStatus);
        _predictionTable = ticTacToeStatus;
        if (result!=GameResult.NotFinished)
        {
            _nodeValue = Convert.ToSingle((int)result);
        }
        else
        {
            for (int i = 0; i < _predictionTable.GetLength(0); i++)
            {
                for(int j = 0; j < _predictionTable.GetLength(1); j++)
                {

                }
            }
        }
    }

    public virtual Node MostEffectiveMove()
    {
        if (_nodeValue != null)
        {
            return this;
        }
    }
}