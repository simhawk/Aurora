using System;
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

      startingResources = new List<Resource>() 
      {
         Resource.Wood, Resource.Wood, Resource.Wood, Resource.Wood,
         Resource.Wheat, Resource.Wheat, Resource.Wheat, Resource.Wheat,
         Resource.Sheep, Resource.Sheep, Resource.Sheep, Resource.Sheep,
         Resource.Rock, Resource.Rock, Resource.Rock, 
         Resource.Brick, Resource.Brick, Resource.Brick,
      };
      startingNumbers = new List<int>() {2,3,3,4,4,5,5,6,6,8,8,9,9,10,10,11,11,12};
      selectingBackward = false;
   }
    /******************************
       Game State Properties
    *******************************/
    public GameState gameState;
    public Player activePlayer;
    public Player[] players;
    public Plane boardPlane;
    public float objectDistanceThreshold;

    public Tuple<int, int, int> rollResults; 

    private bool selectingBackward;

    /******************************
       Static Game Properties
    *******************************/
    public const int VictoryPointsToWin = 10;
    

   // This game is different from Catan, and since it's board is flattened, a desert would be devastating to be near, as such the desert has been removed
   // Since this is the case, a board with 18 hexes (1 less than the normal 19 hex board) still has the same roll numbers.
    public List<Resource> startingResources;

    public List<int> startingNumbers; 

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

      case GameState.ResourceRoll:
         // Wait for the player to click on the dice roll! don't really have to 
         // do too much here unless you want cool animations as soon as they roll the dice
      break;

      case GameState.ResourceRollDone:
         Hex[] hexes = FindObjectsOfType(typeof(Hex)) as Hex[];
         foreach(Hex hex in hexes)
         {
            if(hex.getNumber() != rollResults.Item1 ) {
               continue;
            }

            Resource hexResource = hex.resource;

            List<Settlement> settlements = hex.GetAdjacentSettlements();
            foreach(Settlement settlement in settlements)
            {
               if(!settlement.IsPlaced())
               {
                  continue;
               }
               CivType civType = settlement.GetCivType();
               int numberToAdd = settlement.IsUpgraded() ? 2 : 1;
               getPlayerWithCiv(civType).addToResourceBank(hexResource, numberToAdd);
            }
         }

          printResourcesForPlayer(0);
          printResourcesForPlayer(1);
          printResourcesForPlayer(2);
          printResourcesForPlayer(3);

         gameState = GameState.MainMenu;
      break;
      }
   }

   public void printResourcesForPlayer(int player)
   {
      Debug.Log("Player one has: " + 
         players[player].resources[Resource.Brick] + " Brick, " + 
         players[player].resources[Resource.Wood] + " Wood, " + 
         players[player].resources[Resource.Sheep] + " Sheep, " + 
         players[player].resources[Resource.Rock] + " Rock, " + 
         players[player].resources[Resource.Wheat] + " Wheat!");
   }

   public Player getPlayerWithCiv(CivType civType)
   {
      foreach(Player p in players)
      {
         if(p.civType == civType) return p;
      }
      Debug.Log("Error, could not find the player with civType:" + civType.ToString());
      return players[0];
   }

   public void GivePlayersResources()
   {

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

    public void OnRollButtonClicked()
    {
        rollResults = Dice.RollDice();
        gameState = GameState.ResourceRollDone;
    }
}
