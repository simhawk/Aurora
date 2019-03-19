using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    public static GameManager Instance { get; private set;}
    public bool usingBoard = true;
   public GameObject RollPanel; 
   public GameObject rollPanelNumber;
   public GameObject rollpanelSubtext;
   public float rollpanelDelay;

   public bool placingThief = false;
   public bool placingRobin = false;


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
      buildType = BuildType.NotSelected;


   }
    /******************************
       Game State Properties
    *******************************/
    public GameState gameState;
    public Player activePlayer;
    public Player[] players;
    public Plane boardPlane;
    public float objectDistanceThreshold;
    public Hex selectedHex;
    
    public Tuple<int, int, int> rollResults; 

    private bool selectingBackward;
    public BuildType buildType;

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
      if(Input.GetMouseButtonUp(0))
      {
         HideRollPanel();
      }
      Vector3 mousePoint = getMousePointOnPlane();
      Debug.Log(mousePoint.ToString());
      // StateMachine
      switch(gameState) { 
      case GameState.InitialSettlementPlacement:
      {
         if(Input.GetMouseButtonUp(0))
         {
            Settlement closestSettlement = findClosestSettlementToAndDeselectAll(mousePoint, objectDistanceThreshold);
            if(closestSettlement != null && closestSettlement.isPlaceable())
            {
               closestSettlement.placeSettlementWithActiveCiv(false);
               this.gameState = GameState.InitialRoadPlacement;
            }
         }
         break;
      }

      case GameState.InitialRoadPlacement:
      {
         if(Input.GetMouseButtonUp(0))
         {
            Road closestRoad = findClosestRoadToAndDeselectAll(mousePoint, objectDistanceThreshold);
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
      }
      
      case GameState.ResourceRoll:
      {
         // Wait for the player to click on the dice roll! don't really have to 
         // do too much here unless you want cool animations as soon as they roll the dice
         break;
      }
         

      case GameState.ResourceRollDone:
      {

         // Show UI for rolling the number
         RollPanel.SetActive(true);
         TextMeshProUGUI number = rollPanelNumber.GetComponent<TextMeshProUGUI>();
         number.text = this.rollResults.Item1.ToString();
         TextMeshProUGUI subtext = rollpanelSubtext.GetComponent<TextMeshProUGUI>();
         subtext.text = this.rollResults.Item1 == 7 ? "Place the Thief or Terra" : "Collect your Resources!";
         
         

         if(this.rollResults.Item1 == 7)
         {
            Invoke("HideRollPanel", rollpanelDelay);
            gameState = GameState.PlaceThief;
            break;
         }

         Hex[] hexes = FindObjectsOfType(typeof(Hex)) as Hex[];
         foreach(Hex hex in hexes)
         {
            // skip if the roll number doesn't match up or if the thief is on the hex
            if(hex.getNumber() != rollResults.Item1 || hex.isThiefOnHex) {
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

         //TODO: change this to the correct state(trading)
         gameState = GameState.Trading;
         break;
      }

      case GameState.Trading:
      {
         // TODO: do stuff for trading
         gameState = GameState.BuildOrDevelopmentCard;
         break;
      }

      case GameState.BuildOrDevelopmentCard:
      {
         if(Input.GetMouseButtonUp(0) && buildType != BuildType.NotSelected)
         {
            if(buildType == BuildType.Road)
            {
               Road closestRoad  = findClosestRoadToAndDeselectAll(mousePoint,objectDistanceThreshold);
               if(closestRoad != null && closestRoad.isPlaceable())
               {
                  closestRoad.isSelected = true;
               }
            }
            else if(buildType == BuildType.City)
            {
               Settlement closestSettlement  = findClosestSettlementToAndDeselectAll(mousePoint,objectDistanceThreshold);
               if(closestSettlement != null && !closestSettlement.IsUpgraded() && closestSettlement.IsPlaced())
               {
                  closestSettlement.isSelected = true;
               }
            }
            else if(buildType == BuildType.Settlement)
            {
               Settlement closestSettlement  = findClosestSettlementToAndDeselectAll(mousePoint,objectDistanceThreshold);
               if(closestSettlement != null && closestSettlement.isPlaceable())
               {
                  closestSettlement.isSelected = true;
               }
            }
            
         }
         break;
      }

      case GameState.BuildingSelected:
      {
         // TODO: do stuff for Build or Development
         break;
      }
         
      

      case GameState.PlaceThief:
      {
         if(InputController.DetectedThief())
         {
            Vector3 thiefPoint = getThiefPointOnPlane();
            Hex closestHex = findClosestHexToAndDeselectAll(thiefPoint, 4.0f);
            
            if(closestHex != null && !closestHex.isThiefOnHex)
            {
               selectedHex = closestHex;
               closestHex.isSelectedThief = true;
               HelpingText.AdditionalText = HelpingText.AdditionalText + "Detected Thief";
               placingThief = true;
            }else {
               placingThief = false;
            }
            placingRobin  = false;
         } else if(InputController.DetectedRobinHood())
         {
             Vector3 robinLocation = getRobinHoodPointOnPlane();
             Hex closestHex = findClosestHexToAndDeselectAll(robinLocation, 4.0f);
             
             if(closestHex != null && !closestHex.isRobinOnHex)
             {
                selectedHex = closestHex;
               closestHex.isSelectedRobin = true;
               HelpingText.AdditionalText = HelpingText.AdditionalText + "Detected Robin";
               placingRobin  = true;
             } else {
               placingRobin  = false;
             }
             placingThief = false;
         }
         else 
         {
            HelpingText.AdditionalText = HelpingText.AdditionalText + "not detected";
            placingThief = false;
            placingRobin  = false;
         }
         break;
      }
         
      case GameState.PlaceThiefDone:
      {
         
         Hex[] hexes = FindObjectsOfType(typeof(Hex)) as Hex[];

         // reset thief
         if(placingThief)
         {
            foreach(Hex hex in hexes)
            {
               hex.isSelectedThief = false;
               hex.isThiefOnHex = false;
            }
            if(selectedHex != null)
            {
               selectedHex.isThiefOnHex = true; 
            }
         } else if(placingRobin)
         {
           foreach(Hex hex in hexes)
            {
               hex.isSelectedRobin = false;
               hex.isRobinOnHex = false;
            }
            if(selectedHex != null)
            {
               selectedHex.isRobinOnHex = true; 
               //reset old robin if there
               List<Hex> adj = selectedHex.GetAdjacentHexes();
               foreach(Hex hex in hexes)
               {
                  //put all non adjacent hexes back to normal
                  if(!adj.Contains(hex))
                  {
                     hex.resource = hex.originalResource;
                     hex.displayedResource = hex.originalResource;
                  }
               }

               // update adjacent hexes with their displayed value
               foreach(Hex hex in adj) 
               {
                  hex.resource = hex.displayedResource;
                  hex.InstantiateHexWith(hex.resource);
               }
            }
        }
         
         gameState = GameState.Trading;
         break;
      }

      }
   }

   public void BuildSelectedItem()
   {
      // Selected roads or settlements are already valid
      // i.e. roads aren't already built and are connected, and settlements aren't adjacent
       if(buildType == BuildType.Road)
      {
         Road[] roads = FindObjectsOfType(typeof(Road)) as Road[];
         foreach(Road road in roads)
         {
            if(road.isSelected)
            {
               //check if they have the required resources
               if(activePlayer.resources[Resource.Brick] >= 1
                  && activePlayer.resources[Resource.Wood] >= 1)
               {
                  // place the road
                  road.placeRoadWithActiveCiv();
                  
                  //decrement their resource counters
                  activePlayer.resources[Resource.Brick]--;
                  activePlayer.resources[Resource.Wood]--;
                  return;
               }
               
            }
         }
      }
      else if(buildType == BuildType.City)
      {
         Settlement[] settlements = FindObjectsOfType(typeof(Settlement)) as Settlement[];
         foreach(Settlement settlement in settlements)
         {
            // ensure that the settlement to be upgraded is selected, and is of the same type
            if(settlement.isSelected && settlement.GetCivType().Equals(activePlayer.civType))
            {
               // check if they have the required resouces
               if(activePlayer.resources[Resource.Rock] >= 3
                  && activePlayer.resources[Resource.Wheat] >= 2)
               {
                  // replace settlement with city
                  settlement.placeSettlementWithActiveCiv(true);
                  // dectrement resource counters;
                  activePlayer.resources[Resource.Rock] -= 3;
                  activePlayer.resources[Resource.Wheat] -= 2;

                  return;
               }
            }
         }
      }
      else if(buildType == BuildType.Settlement)
      {
         Settlement[] settlements = FindObjectsOfType(typeof(Settlement)) as Settlement[];
         foreach(Settlement settlement in settlements)
         {
            if(settlement.isSelected)
            {
               if(activePlayer.resources[Resource.Wheat] >= 1
                  && activePlayer.resources[Resource.Sheep] >= 1
                  && activePlayer.resources[Resource.Brick] >= 1
                  && activePlayer.resources[Resource.Wood] >= 1)
               {

                  // place settlement if they have the resources
                  settlement.placeSettlementWithActiveCiv(false);

                  // decrement the resource counter
                  activePlayer.resources[Resource.Wheat]--;
                  activePlayer.resources[Resource.Sheep]--;
                  activePlayer.resources[Resource.Brick]--;
                  activePlayer.resources[Resource.Wood]--;

                  return;
               }
            }
         }
      }
   }

   public void deselectAll()
   {
      Road[] roads = FindObjectsOfType(typeof(Road)) as Road[];
         
      foreach(Road road in roads)
      {
         road.isSelected = false;
      }

      Settlement[] settlements = FindObjectsOfType(typeof(Settlement)) as Settlement[];
      
      foreach(Settlement settlement in settlements)
      {
         settlement.isSelected = false;
      }
   }

   public bool isSomethingSelected()
   {
      if(buildType == BuildType.Road)
      {
         Road[] roads = FindObjectsOfType(typeof(Road)) as Road[];
         foreach(Road road in roads)
         {
            if(road.isSelected)
            {
               return true;
            }
         }
      }
      else if(buildType == BuildType.City)
      {
         Settlement[] settlements = FindObjectsOfType(typeof(Settlement)) as Settlement[];
         foreach(Settlement settlement in settlements)
         {
            if(settlement.isSelected)
            {
               return true;
            }
         }
      }
      else if(buildType == BuildType.Settlement)
      {
         Settlement[] settlements = FindObjectsOfType(typeof(Settlement)) as Settlement[];
         foreach(Settlement settlement in settlements)
         {
            if(settlement.isSelected)
            {
               return true;
            }
         }
      }
      return false;
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

    public Vector3 getRobinHoodPointOnPlane()
    {
      int layer_mask = LayerMask.GetMask("BoardPlane");
       Vector2 robinPos = InputController.getRobinHoodPosition();
       Ray ray = Camera.main.ScreenPointToRay(robinPos);
       RaycastHit hit;
       Physics.Raycast(ray, out hit, layer_mask);
       return hit.point;
    }

   public Vector3 getThiefPointOnPlane()
      {
         int layer_mask = LayerMask.GetMask("BoardPlane");
         Vector2 thiefPos = InputController.getThiefPosition();
         Ray ray = Camera.main.ScreenPointToRay(thiefPos);
         RaycastHit hit;
         Physics.Raycast(ray, out hit, layer_mask);
         return hit.point;
      }

    public Settlement findClosestSettlementToAndDeselectAll(Vector3 point, float minThreshold = float.PositiveInfinity) 
    {
         Settlement[] settlements = FindObjectsOfType(typeof(Settlement)) as Settlement[];
        
         float minDistance = 10000;
         Settlement closestSettlement = null;

         foreach(Settlement settlement in settlements)
         {
            settlement.isSelected = false;
           float distance = Vector3.Distance(settlement.transform.position, point);
            if (distance < minDistance && distance < minThreshold)
            {
               minDistance = distance;
               closestSettlement = settlement;
            }
             
         }
         return closestSettlement;
    }

    public Hex findClosestHexToAndDeselectAll(Vector3 point, float minThreshold = float.PositiveInfinity) 
    {

         Hex[] hexes = FindObjectsOfType(typeof(Hex)) as Hex[];
        
         float minDistance = 10000;
         Hex closestHex = null;

         foreach(Hex hex in hexes)
         {
            hex.isSelectedRobin = false;
            hex.isSelectedThief = false;
            // only click valid hexes
            if(hex.resource == Resource.Brick || 
            hex.resource == Resource.Wood || 
            hex.resource == Resource.Wheat || 
            hex.resource == Resource.Rock || 
            hex.resource == Resource.Sheep)
            {
               float distance = Vector3.Distance(hex.transform.position, point);
               if (distance < minDistance && distance < minThreshold)
               {
                  minDistance = distance;
                  closestHex = hex;
               }
            }
         }
         return closestHex;
    }

    public Road findClosestRoadToAndDeselectAll(Vector3 point, float minThreshold = float.PositiveInfinity) 
    {
         Road[] roads = FindObjectsOfType(typeof(Road)) as Road[];
        
         
         float minDistance = 10000;
         Road closestRoad = null;

         foreach(Road road in roads)
         {
            road.isSelected = false;
           float distance = Vector3.Distance(road.transform.position, point);
            if (distance < minDistance && distance < minThreshold)
            {
               minDistance = distance;
               closestRoad = road;
            }
             
         }
         return closestRoad;
    }

    public void HideRollPanel()
    {
       RollPanel.SetActive(false);
    }

}
