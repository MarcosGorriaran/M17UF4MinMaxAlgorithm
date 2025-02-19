using Unity.VisualScripting;
using UnityEngine;

/**
 * <summary>
 * This enum is responsible of representing the different GameResult the method
 * GameResult EvaluateWin(MatrixStatus[,]) can return which should make it easier
 * to read.
 * </summary>
 */
public enum GameResult : int
{
    Draw = 0,
    Victory = 1,
    Defeat = -1,
    NotFinished = 2
}
public static class Calculs
{
    public static float LinearDistance;
    public static Vector2 FirstPosition;
    private static float offset = 0.1f;
    public static void CalculateDistances(BoxCollider2D coll, float size)
    {
        LinearDistance = coll.size.x / size;
        FirstPosition = new Vector2(-size / 2f, size / 2f);
    }
    public static Vector2 CalculatePoint(int x, int y)
    {
        return FirstPosition + new Vector2(x * LinearDistance, -y* LinearDistance);
    }
    public static GameResult EvaluateWin(MatrixStatus[,] matrix)
    {
        int counterX = 0;
        int counterY = 0;
        int counterD1 = 0;
        int counterD2 = 0;
        for(int i=0; i<matrix.GetLength(0); i++)
        {
            for (int j = 0; j < matrix.GetLength(1);j++)
            {
                counterY += (int)matrix[i, j];
                counterX += (int)matrix[j, i];
            }
            if (counterY == 3 || counterX == 3) return GameResult.Victory;
            else if (counterY == -3 || counterX ==-3) return GameResult.Defeat;
            counterX = 0;
            counterY = 0;
            counterD1 += (int)matrix[i, i];
            counterD2 += (int)matrix[2-i, i];
        }
        if (counterD1 == 3 || counterD2 == 3) return GameResult.Victory;
        else if(counterD1 == -3 || counterD2 == -3)  return GameResult.Defeat;
        for(int i=0; i<matrix.GetLength(0);i++)
        {
            for(int j = 0; j < matrix.GetLength(1);j++)
            {
                if (matrix[i, j] == 0) return GameResult.NotFinished;
            }
        }
        return 0; // 0 empat, 1 guanya 1, -1 guanya 2, 2 no s'ha acabat
    }
    public static bool CheckIfValidClick(Vector2 mousePosition, MatrixStatus[,] matrix)
    {
        for (int i = 0; i < matrix.GetLength(0); i++)
        {
            for (int j = 0; j < matrix.GetLength(1); j++)
            {
                Vector2 point = CalculatePoint(i, j);
                if (Mathf.Abs(mousePosition.x - point.x) < LinearDistance / 2f - offset
                    && Mathf.Abs(mousePosition.y - point.y) < LinearDistance / 2f - offset)
                {
                    if (matrix[i, j] == MatrixStatus.Empty)
                    {
                        GameManager.Instance.DoMove(i, j, MatrixStatus.Player);
                        return true;
                    }
                }
                //Debug.Log(CalculatePoint(i, j));
            }
        }
        return false;
    }
    public static MatrixStatus[,] CopyMatrixByValue(MatrixStatus[,] matrixStatus)
    {
        MatrixStatus[,] newTable = new MatrixStatus[matrixStatus.GetLength(0), matrixStatus.GetLength(1)];
        for (int i = 0; i < newTable.GetLength(0); i++)
        {
            for (int j = 0; j < newTable.GetLength(1); j++)
            {
                newTable[i, j] = matrixStatus[i, j].CloneViaSerialization();
            }
        }
        return newTable;
    }
    public static MatrixStatus CopyMatrixStatusByValue(MatrixStatus status) 
    {
        return status.CloneViaSerialization();
    }
}
