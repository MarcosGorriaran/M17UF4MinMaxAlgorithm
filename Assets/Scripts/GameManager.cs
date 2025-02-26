using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;
public enum States
{
    CanMove,
    CantMove
}
/**
 * <summary>
 * This enum represents in which state a Matrix is in, which should make it easier to read on the code
 * when it is evaluating in which state is a cell or when it updating the Matrix status.
 * </summary>
 */
public enum MatrixStatus : int
{
    IA = -1,
    Player = 1,
    Empty = 0
}
public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public BoxCollider2D collider;
    public GameObject token1, token2;
    public int Size = 3;
    public MatrixStatus[,] Matrix;
    [SerializeField] private States state = States.CanMove;
    public Camera camera;
    [SerializeField]
    private LoadingIcon _thinkingFeedback;
    [SerializeField]
    private AudioSource _drawSound;
    [SerializeField]
    private ToggleButton _toggleButton;
    void Start()
    {
        Instance = this;
        Matrix = new MatrixStatus[Size, Size];
        Calculs.CalculateDistances(collider, Size);
        for (int i = 0; i < Size; i++)
        {
            for (int j = 0; j < Size; j++)
            {
                Matrix[i, j] = MatrixStatus.Empty; // 0: desocupat, 1: fitxa jugador 1, -1: fitxa IA;
            }
        }
        if(camera == null)
        {
            camera = Camera.main;
        }
    }
    private void Update()
    {
        if (state == States.CanMove)
        {
            Vector3 m = Input.mousePosition;
            m.z = 10f;
            Vector3 mousepos = camera.ScreenToWorldPoint(m);
            if (Input.GetMouseButtonDown(0))
            {
                if (Calculs.CheckIfValidClick((Vector2)mousepos, Matrix))
                {
                    state = States.CantMove;
                    if(Calculs.EvaluateWin(Matrix)==GameResult.NotFinished)
                        StartCoroutine(WaitingABit());
                }
            }
        }
    }
    private IEnumerator WaitingABit()
    {
        yield return new WaitForSeconds(1f);
        _thinkingFeedback.LoadingHasStarted();
        if (_toggleButton.IsOn())
        {
            RandomAI();
        }
        else
        {
            yield return new WaitUntil(() => MinMaxAI().GetAwaiter().IsCompleted);
        }
        
        _thinkingFeedback.LoadingHasStoped();
    }
    public void RandomAI()
    {
        int x;
        int y;
        do
        {
            x = Random.Range(0, Size);
            y = Random.Range(0, Size);
        } while (Matrix[x, y] != MatrixStatus.Empty);
        DoMove(x, y, MatrixStatus.IA);
        state = States.CanMove;
    }
    public async Task MinMaxAI()
    {
        Node node = new Node(Matrix,MatrixStatus.IA);
        MatrixStatus[,] newTableState= node.ChoosenNode.PredictedTable;

        for(int i = 0; i < newTableState.GetLength(0); i++)
        {
            for(int j = 0; j < newTableState.GetLength(1); j++)
            {
                if (newTableState[i,j] != Matrix[i, j])
                {
                    DoMove(i,j, MatrixStatus.IA);
                }
            }
        }
        state = States.CanMove;
        return;
    }
    public void DoMove(int x, int y, MatrixStatus team)
    {
        Matrix[x, y] = team;
        if (team == MatrixStatus.Player)
            Instantiate(token1, Calculs.CalculatePoint(x, y), Quaternion.identity);
        else
            Instantiate(token2, Calculs.CalculatePoint(x, y), Quaternion.identity);
        GameResult result = Calculs.EvaluateWin(Matrix);
        switch (result)
        {
            case GameResult.Draw:
                StartCoroutine(ReloadScene());
                break;
            case GameResult.Victory:
                FindFirstObjectByType<ChangeScene>().LoadScene("VictoryScreen");
                break;
            case GameResult.Defeat:
                FindFirstObjectByType<ChangeScene>().LoadScene("DefeatScreen");
                break;
            case GameResult.NotFinished:
                if(state == States.CantMove)
                    state = States.CanMove;
                break;
        }
    }
    private IEnumerator ReloadScene()
    {
        _drawSound.Play();
        yield return new WaitUntil(()=>!_drawSound.isPlaying);
        FindFirstObjectByType<ChangeScene>().LoadScene("SampleScene");
    }
}
