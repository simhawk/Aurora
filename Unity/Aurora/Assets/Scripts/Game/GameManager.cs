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
    public float objectDistanceThreshold;

    private bool selectingBackward = false;

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
    

    public void Update()
    {
       //Conduct state machine cycle
       StateMachine();
    }

   private void StateMachine()
   {
      Vector3 mousePoint = getMousePointOnPlane();

      // StateMachine
      switch(gameState) 
      {
      case GameState.InitialSettlementPlacement:
         if(Input.GetMouseButtonUp(0))
         {
            Settlement closestSettlement = findClosestSettlementTo(mousePoint, objectDistanceThreshold);
            if(closestSettlement != null && closestSettlement.isPlaceable())
            {
               closestSettlement.placeSettlementWithActiveCiv(false);
               this.gameState = GameState.InitialRoadPlacement;
            }
         }
      break;

      case GameState.InitialRoadPlacement:
         if(Input.GetMouseButtonUp(0))
         {
            Road closestRoad = findClosestRoadTo(mousePoint, objectDistanceThreshold);
            if(closestRoad != null && closestRoad.isPlaceable())
            {
               closestRoad.placeRoadWithActiveCiv();

               bool endingPlayersTurn = getActivePlayerIndex()==players.Length-1;
               bool firstPlayersTurn = getActivePlayerIndex()==0;

               if(selectingBackward) {
                  if(!firstPlayersTurn)
                  {
                     SetPreviousActivePlayer();
                  } 
               } else {
                  if(endingPlayersTurn) 
                  {
                     selectingBackward = true;
                  } 
                  else 
                  {
                     SetNextActivePlayer();
                  } 
               }

               if(AllInitialRoadsAndSettlementsComplete())
               {
                  this.gameState = GameState.ResourceRoll;
               } 
               else 
               {
                  this.gameState = GameState.InitialSettlementPlacement;  
               }
            }
         }
      break;

      case GameState.ResourceRoll;l
      }
   }

   public List<Settlement> GetPlacedSettlements()
   {
         Settlement[] settlements = FindObjectsOfType(typeof(Settlement)) as Settlement[];
         List<Settlement> placedSettlements = new List<Settlement>();
         foreach(Settlement settlement in settlements)
         {
          if(settlement.IsPlaced())
            placedSettlements.Add(settlement);             
         }
         return placedSettlements;
   }

   public List<Road> GetPlacedRoads()
   {
         Road[] roads = FindObjectsOfType(typeof(Road)) as Road[];
         List<Road> placedRoads = new List<Road>();
         foreach(Road road in roads)
         {
          if(road.IsPlaced())
            placedRoads.Add(road);             
         }
         return placedRoads;
   }

   private bool AllInitialRoadsAndSettlementsComplete()
   {
      return (GetPlacedRoads().Count == players.Length*2 && GetPlacedSettlements().Count == players.Length*2);
   }

    public int getActivePlayerIndex()
    {
       return System.Array.IndexOf(players, activePlayer);
    }

    public void SetNextActivePlayer() 
    {
      int currIndex = getActivePlayerIndex();
      if(currIndex < players.Length-1)
      {
         activePlayer = players[currIndex+1];
      }
      else
      {
         activePlayer = players[0];
      } 
    }

     public void SetPreviousActivePlayer() 
    {
      int currIndex = getActivePlayerIndex();
      if(currIndex > 0)
      {
         activePlayer = players[currIndex-1];
      }
      else
      {
         activePlayer = players[players.Length - 1];
      } 
    }

    public Vector3 getMousePointOnPlane() {
      int layer_mask = LayerMask.GetMask("BoardPlane");

      //Create a ray from the Mouse click position
      Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
      RaycastHit hit; 
      Physics.Raycast(ray, out hit, layer_mask); 
      return hit.point;
    }

   //  public bool canInstantiateThisSettlement(Settlement settlement)
   //  {

   //  }

    public Settlement findClosestSettlementTo(Vector3 point, float minThreshold = float.PositiveInfinity) 
    {
         Settlement[] settlements = FindObjectsOfType(typeof(Settlement)) as Settlement[];
        
         float minDistance = 10000;
         Settlement closestSettlement = null;

         foreach(Settlement settlement in settlements)
         {
           float distance = Vector3.Distance(settlement.transform.position, point);
            if (distance < minDistance && distance < minThreshold)
            {
               minDistance = distance;
               closestSettlement = settlement;
            }
             
         }
         return closestSettlement;
    }

    public Road findClosestRoadTo(Vector3 point, float minThreshold = float.PositiveInfinity) 
    {
         Road[] roads = FindObjectsOfType(typeof(Road)) as Road[];
        
         float minDistance = 10000;
         Road closestRoad = null;

         foreach(Road road in roads)
         {
           float distance = Vector3.Distance(road.transform.position, point);
            if (distance < minDistance && distance < minThreshold)
            {
               minDistance = distance;
               closestRoad = road;
            }
             
         }
         return closestRoad;
    }
}
