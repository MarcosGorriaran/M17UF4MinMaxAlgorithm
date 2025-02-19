using System;
using System.Collections.Generic;

public class Node
{
    #region Attributes
    private List<Node> _nodeNextStep = new List<Node>();
    private Node _choosenNode = null;
    private Node _parentNode;
    private MatrixStatus[,] _predictionTable;
    private MatrixStatus _turnOrder;
    private float? _nodeValue = null;
    private float _alpha = float.MinValue;
    private float _beta = float.MaxValue;
    #endregion

    #region Geters and setters
    public Node ChoosenNode {  
        get 
        {
            GenerateRoute();
            return _choosenNode;
        }
    }
    public MatrixStatus[,] PredictedTable
    {
        get { return _predictionTable; }
    }
    #endregion


    public Node(MatrixStatus[,] ticTacToeStatus, MatrixStatus actualTurnOrder)
    {
        if (actualTurnOrder == MatrixStatus.Empty)
        {
            throw new Exception("You can't assign Empty as the actual player");
        }
        GameResult result = Calculs.EvaluateWin(ticTacToeStatus);
        _turnOrder = Calculs.CopyMatrixStatusByValue(actualTurnOrder);
        _predictionTable = Calculs.CopyMatrixByValue(ticTacToeStatus);
        
        if (result!=GameResult.NotFinished)
        {
            _nodeValue = Convert.ToSingle((int)result) * -1;
        }
        else
        {
            for (int i = 0; i < _predictionTable.GetLength(0); i++)
            {
                for(int j = 0; j < _predictionTable.GetLength(1); j++)
                {
                    MatrixStatus[,] nextMove = Calculs.CopyMatrixByValue(ticTacToeStatus);
                    MatrixStatus nextTurn = actualTurnOrder == MatrixStatus.Player ? MatrixStatus.IA : MatrixStatus.Player;
                    if (nextMove[i, j] == MatrixStatus.Empty)
                    {
                        nextMove[i, j] = Calculs.CopyMatrixStatusByValue(actualTurnOrder);
                        _nodeNextStep.Add(new Node(nextMove,nextTurn,this)); 
                    }
                    
                }
            }
        }
    }
    private Node(MatrixStatus[,] tictacToeStatus, MatrixStatus actualTurnOrder,Node parentNode) : this(tictacToeStatus,actualTurnOrder)
    {
        _parentNode = parentNode;
    }

    private void GenerateRoute()
    {
        if (_nodeValue != null)
        {
            return;
        }
        float possibleValue = 0f;
        
        foreach (Node node in _nodeNextStep)
        {
            if (PerformProoning()) return;
            node.GenerateRoute();
            if(_turnOrder == MatrixStatus.IA)
            {
                MaxOperation(node);
            }
            else
            {
                MinOperation(node);
            }
        }
    }
    private void MinOperation(Node potentialNewValue)
    {
        if (!_nodeValue.HasValue)
        {
            _choosenNode = potentialNewValue;
            _nodeValue = potentialNewValue._nodeValue;
        }
        if(potentialNewValue._nodeValue < _nodeValue)
        {
            _choosenNode = potentialNewValue;
            _nodeValue = potentialNewValue._nodeValue;
        }
        if (potentialNewValue._nodeValue < _beta) _beta = potentialNewValue._nodeValue.Value;
    }
    private void MaxOperation(Node potentialNewValue)
    {
        if (!_nodeValue.HasValue)
        {
            _choosenNode = potentialNewValue;
            _nodeValue = potentialNewValue._nodeValue;
        }
        if (potentialNewValue._nodeValue > _nodeValue)
        {
            _choosenNode = potentialNewValue;
            _nodeValue = potentialNewValue._nodeValue;
        }
        if (potentialNewValue._nodeValue > _alpha) _alpha = potentialNewValue._nodeValue.Value;
    }
    private bool PerformProoning()
    {
        return _alpha>=_beta;
    }
    
}