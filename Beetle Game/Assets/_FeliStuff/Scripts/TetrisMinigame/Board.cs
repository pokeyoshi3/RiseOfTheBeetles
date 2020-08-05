//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//public class Board : MonoBehaviour
//{
//    public GameObject ReplayButton;
//    public TMPro.TextMeshProUGUI Result;

//    //TODO: make it possible to get those from txt?xml?
//    [SerializeField]
//    private int[] goalPos;
//    [SerializeField]
//    private int[] obstaclePos;
//    private TetrisMovement player;

//[SerializeField]
//    private int width = 10;
//    [SerializeField]
//    private int height = 19;
//    [SerializeField]
//    private TetrisMovement[] playerTilePrefab;
//    [SerializeField]
//    private Tile tilePrefab;
//    public eMinigameState State { get; private set; }
//    public int Width { get; private set; }
//    public int Height { get; private set; }
//    public TetrisTileInfo[] tiles { get; private set; }
//    public int Size { get; private set; }  //tilesize for positioning
//    public int XOffset { get; private set; }  //spawn X
//    public int YOffset { get; private set; }  //spawn Y

//    ////HACK for gate 1
//    //private eAbility tempAbility;

//    //public static event System.Action<eAbility, bool> OnGameEnd = delegate { };

//    private void Awake()
//    {
//        State = eMinigameState.paused;
//        //ReplayButton.SetActive(false);
//        Size = 50;
//        XOffset = 200;
//        YOffset = 450;
//        Width = width;
//        Height = height;
//        BoardSetup();
//        //Queen.OnMinigameStart += BoardSetup;
//    }

//    private void OnDisable()
//    {
//        State = eMinigameState.paused;
//    }

//    private void BoardSetup()
//    {
//        //tempAbility = ability;

//        //HACK for gate 1        
//        goalPos = new int[4];
//        goalPos[0] = 156;
//        goalPos[1] = 166;
//        goalPos[2] = 167;
//        goalPos[3] = 176;

//        obstaclePos = new int[13];
//        obstaclePos[0] = 44;
//        obstaclePos[1] = 41;
//        obstaclePos[2] = 42;
//        obstaclePos[3] = 43;
//        obstaclePos[4] = 86;
//        obstaclePos[5] = 87;
//        obstaclePos[6] = 95;
//        obstaclePos[7] = 96;
//        obstaclePos[8] = 97;
//        obstaclePos[9] = 133;
//        obstaclePos[10] = 134;
//        obstaclePos[11] = 135;
//        obstaclePos[12] = 136;

//        SpawnBoard();
//        SpawnPlayerTile(ePlayerShape.TBlock);
//        SetGoal();
//        SetObstacles();
//    }

//    private void Update()
//    {
//        if (Input.GetButtonUp("Down") && State == eMinigameState.ready)
//        {
//            ChangeGameState(eMinigameState.running);
//            StartCoroutine(player.AutomaticDropping());
//        }
//    }

//    private void SpawnBoard()
//    {
//        tiles = new TetrisTileInfo[Height * Width];
//        int tileCount = 0;

//        for (int y = 0; y < Height; y++)
//        {
//            for (int x = 0; x < Width; x++)
//            {
//                Tile newTile = Instantiate(tilePrefab);
//                newTile.transform.SetParent(this.gameObject.transform);                
//                newTile.rectTrans.anchoredPosition = new Vector3(x*Size-XOffset, -y* Size+YOffset, 0);
//                newTile.name = "tile("+tileCount+")";
//                newTile.PosInArray = tileCount;
//                newTile.rectTrans.localScale = new Vector3(1.0f, 1.0f, 1.0f);

//                if (x == 0 || x == Width-1 || y == Height-1)
//                { 
//                    tiles[tileCount] = new TetrisTileInfo(x, y, newTile, ePosState.border);
//                    newTile.SetColor(Color.blue);
//                }
//                else
//                {
//                    tiles[tileCount] = new TetrisTileInfo(x, y,newTile, ePosState.free);
//                    newTile.SetColor(Color.cyan);
//                }
//                tileCount++;
//            }
//        }
//    }

//    private void SetObstacles()
//    {
//        for (int i = 0; i < obstaclePos.Length; i++)
//        {
//            tiles[obstaclePos[i]].Update(ePosState.obstacle);
//            tiles[obstaclePos[i]].tile.SetColor(Color.gray);            
//        }
//    }

//    private void SetGoal()
//    {
//        for (int i = 0; i < goalPos.Length; i++)
//        {
//            tiles[goalPos[i]].Update(ePosState.goal);
//            tiles[goalPos[i]].tile.SetColor(Color.magenta);
//        }
//    }

//    // LBlock = 0, TBlock = 1, IBlock = 2, OBlock = 3, SBlock = 4, ZBlock = 5, JBlock = 6 
//    private void SpawnPlayerTile(ePlayerShape playerShape)
//    {
//        player = Instantiate(playerTilePrefab[(int)playerShape]);
//        player.transform.SetParent(this.gameObject.transform);
//        player.rectTrans.anchoredPosition = new Vector3
//                                            (player.startPos * Size - XOffset, -0 * Size + YOffset, 0);
//        player.rectTrans.localScale = new Vector3(1.0f, 1.0f, 1.0f);
//        player.SetBoard(this);
//    }

//    public void ChangeGameState(eMinigameState newState)
//    {
//        State = newState;
//    }

//    public void GameResult(bool win)
//    {
//        if (win)
//        {
//            print("u win");
//            //ChangeGameState(eMinigameState.Win);
//        }
//        else
//        {
//            print("u lose");
//            //ChangeGameState(eMinigameState.Lose);
//        }
//        print(State.ToString());

//        //StartCoroutine(EndGame());
//    }

//    //private IEnumerator EndGame()
//    //{
//    //    while (BlackScreen.alpha < 1.0f)
//    //    {
//    //        BlackScreen.alpha += 0.1f;
//    //        yield return new WaitForSeconds(0.1f);
//    //    }
//    //    OnGameEnd(tempAbility, true);
//    //    this.gameObject.SetActive(false);
//    //}

//        //HACK temporary button call to reset the minigame
//        public void ReplayGame()
//    {
//        string currentSceneName = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name;
//        UnityEngine.SceneManagement.SceneManager.LoadScene(currentSceneName);
//    }
//}


