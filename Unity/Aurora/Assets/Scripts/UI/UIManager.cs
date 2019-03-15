using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;


// create all callbacks for each button on the UI menu, the
// close toggle is attached to SlidingPanels object in hierarchy
public class UIManager : MonoBehaviour
{
   public Player player;   

   public GameObject BrickTextObj;
   public GameObject WoodTextObj;
   public GameObject RockTextObj;
   public GameObject WheatTextObj;
   public GameObject SheepTextObj;

   public GameObject PlayerNameObj;

   public GameObject ButtonFinished;
   public GameObject ButtonRoll;
   public GameObject ButtonConfirm;
   public GameObject ButtonCancel;
   

   public 

   /// <summary>
   /// Start is called on the frame when a script is enabled just before
   /// any of the Update methods is called the first time.
   /// </summary>
   void Start()
   {
       
   }

   /// <summary>
   /// Update is called every frame, if the MonoBehaviour is enabled.
   /// </summary>
   void Update()
   {
      // Print playerName
      TextMeshProUGUI playerName = PlayerNameObj.GetComponent<TextMeshProUGUI>();       
      if(playerName != null)
         playerName.SetText(player.DisplayName);

      // Print all resources values onto the screen
      TextMeshProUGUI brickText = BrickTextObj.GetComponent<TextMeshProUGUI>();       
      if(brickText != null)
         brickText.SetText(player.resources[Resource.Brick].ToString());

      TextMeshProUGUI woodText = WoodTextObj.GetComponent<TextMeshProUGUI>();       
      if(woodText != null)
         woodText.SetText(player.resources[Resource.Wood].ToString());

      TextMeshProUGUI rockText = RockTextObj.GetComponent<TextMeshProUGUI>();       
      if(rockText != null)
         rockText.SetText(player.resources[Resource.Rock].ToString());

      TextMeshProUGUI wheatText = WheatTextObj.GetComponent<TextMeshProUGUI>();       
      if(wheatText != null)
         wheatText.SetText(player.resources[Resource.Wheat].ToString());

      TextMeshProUGUI sheepText = SheepTextObj.GetComponent<TextMeshProUGUI>();       
      if(sheepText != null)
         sheepText.SetText(player.resources[Resource.Sheep].ToString());

      
      // Set the buttons on the right to active if it's currently that player's turn
      if(!GameManager.Instance.activePlayer.Equals(this.player))
      {
         ButtonFinished.SetActive(false);
         ButtonRoll.SetActive(false);
         ButtonCancel.SetActive(false);
         ButtonConfirm.SetActive(false);
      } 
      else // if this ui manager is for the active player, check the state to determine which button to show 
      {
         GameState state= GameManager.Instance.gameState;
         
         // Set them all off by default
         ButtonFinished.SetActive(false);
         ButtonRoll.SetActive(false);
         ButtonCancel.SetActive(false);
         ButtonConfirm.SetActive(false);

         switch(GameManager.Instance.gameState)
         {
            
            // maybe add confirm buttons here
            case GameState.InitialSettlementPlacement:   
            break;

            case GameState.InitialRoadPlacement:   
            break;

            case GameState.ResourceRoll:
                  ButtonRoll.SetActive(true);
            break;

            case GameState.ResourceRollDone:
            break;

            case GameState.PlaceThief:
                  ButtonConfirm.SetActive(true);
            break;

            case GameState.PlaceThiefDone:
            break;

            case GameState.Trading:

            break;

            case GameState.BuildOrDevelopmentCard:
                  if(GameManager.Instance.isSomethingSelected())
                  {
                     ButtonCancel.SetActive(true);
                     ButtonConfirm.SetActive(true);
                  } 
                  else
                  {
                     ButtonFinished.SetActive(true);
                  }
            break;

            case GameState.BuildingSelected:
               
                  ButtonCancel.SetActive(true);
                  ButtonConfirm.SetActive(true);
            break;
         }
      }
   }

   public void OnRoadClick()
   {
      GameState state = GameManager.Instance.gameState;
       if(state.Equals(GameState.BuildOrDevelopmentCard))
       {
          GameManager.Instance.deselectAll();
          GameManager.Instance.buildType = BuildType.Road;
       }
   }

   public void OnSettlementClick()
   {
      GameState state = GameManager.Instance.gameState;
      if(state.Equals(GameState.BuildOrDevelopmentCard))
       {
          GameManager.Instance.deselectAll();
          GameManager.Instance.buildType = BuildType.Settlement;
       }
   }

   public void OnCityClick()
   {
      GameState state = GameManager.Instance.gameState;
      if(state.Equals(GameState.BuildOrDevelopmentCard))
       {
          GameManager.Instance.deselectAll();
          GameManager.Instance.buildType = BuildType.City;
       }
   }

   public void OnDevCardClick()
   {
      GameState state = GameManager.Instance.gameState;
      if(state.Equals(GameState.BuildOrDevelopmentCard))
       {
          GameManager.Instance.deselectAll();
          GameManager.Instance.buildType = BuildType.DevCard;
       }
   }

   public void OnFinishedClick()
   {
      GameState state = GameManager.Instance.gameState;

      if(state.Equals(GameState.BuildOrDevelopmentCard))
       {
          // disable the roll panel
          GameManager.Instance.RollPanel.SetActive(false);

          GameManager.Instance.buildType = BuildType.NotSelected;
          GameManager.Instance.deselectAll();
          GameManager.Instance.SetNextActivePlayer();
          GameManager.Instance.gameState = GameState.ResourceRoll;
       }
   }

   public void OnRollClick()
   {
      // as soon as it's clicked set the value of the roll results
      GameManager.Instance.rollResults = Dice.RollDice();
      GameManager.Instance.gameState = GameState.ResourceRollDone;
   }

   public void OnConfirmClick()
   {
      GameState state = GameManager.Instance.gameState;
     
      if(state.Equals(GameState.PlaceThief))
      {
         GameManager.Instance.gameState = GameState.PlaceThiefDone;
      }
      else if(state.Equals(GameState.BuildOrDevelopmentCard))
      {
         //BUILD THE DAMN THING HERE
         GameManager.Instance.BuildSelectedItem();
          GameManager.Instance.deselectAll();
          GameManager.Instance.buildType = BuildType.NotSelected;
      }
   }

   public void OnCancelClick()
   {
      // when building 1 and change to road / settlement, do the same for confirm
      GameState state = GameManager.Instance.gameState;
      if(state.Equals(GameState.BuildOrDevelopmentCard))
      {
          GameManager.Instance.deselectAll();
          GameManager.Instance.buildType = BuildType.NotSelected;
      }
   }
}
