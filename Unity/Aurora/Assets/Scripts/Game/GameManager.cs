using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    public static GameManager Instance { get; private set;}
    
    void Awake()
    {
        if(Instance == null) 
        {
           Instance = this;
           DontDestroyOnLoad(gameObject);
           Initialize();
        } 
        else 
        {
           Destroy(gameObject);
        }
    }

   private void Initialize()
   {
      gameState = GameState.InitialSettlementPlacement;
   }
    /******************************
       Game State Properties
    *******************************/
    public GameState gameState;
    public Player activePlayer;
    public Player[] players;
    public Plane boardPlane;

    /******************************
       Static Game Properties
    *******************************/
    public const int VictoryPointsToWin = 10;
    

   // This game is different from Catan, and since it's board is flattened, a desert would be devastating to be near, as such the desert has been removed
   // Since this is the case, a board with 18 hexes (1 less than the normal 19 hex board) still has the same roll numbers.
    public List<Resource> startingResources = new List<Resource>() 
    {
        Resource.Wood, Resource.Wood, Resource.Wood, Resource.Wood,
        Resource.Wheat, Resource.Wheat, Resource.Wheat, Resource.Wheat,
        Resource.Sheep, Resource.Sheep, Resource.Sheep, Resource.Sheep,
        Resource.Rock, Resource.Rock, Resource.Rock, 
        Resource.Brick, Resource.Brick, Resource.Brick,
    };

    public List<int> startingNumbers =new List<int>() {2,3,3,4,4,5,5,6,6,8,8,9,9,10,10,11,11,12};

    /******************************
       Instance Game Properties
    *******************************/
    public BoardMode boardMode = BoardMode.RandomPooled;
    

    public /// <summary>
    /// Update is called every frame, if the MonoBehaviour is enabled.
    /// </summary>
    void Update()
    {
        if(Time.time > 10)
        {
           Debug.Log(boardPlane.ToString());
           activePlayer = players[1];
        }
    }
}
