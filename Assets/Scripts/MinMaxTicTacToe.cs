
public class MinMaxTicTacToe
{
    public MinMaxTicTacToe(MatrixStatus[,] ticTacToeTable, MatrixStatus actualTurn) 
    {
        if (actualTurn == MatrixStatus.Empty)
        {
            throw new System.Exception("You can't assign the Empty as the actual player");
        }
    }
}