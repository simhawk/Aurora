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
         switch(GameManager.Instance.gameState)
         {
            
            case GameState.InitialRoadPlacement:
                  ButtonFinished.SetActive(false);
                  ButtonRoll.SetActive(false);
                  ButtonCancel.SetActive(false);
                  ButtonConfirm.SetActive(false);
            break;

            case GameState.ResourceRoll:
                  ButtonFinished.SetActive(false);
                  ButtonRoll.SetActive(true);
                  ButtonCancel.SetActive(false);
                  ButtonConfirm.SetActive(false);
            break;

            case GameState.ResourceRollDone:
            break;

            case GameState.PlaceThief:
            break;
         }
      }
      
   }


   public void OnRoadClick()
   {
       
   }

   public void OnSettlementClick()
   {

   }

   public void OnCityClick()
   {

   }

   public void OnDevCardClick()
   {

   }

   public void OnFinishedClick()
   {

   }
   
   public void OnRollClick()
   {
      // as soon as it's clicked set the value of the roll results
      GameManager.Instance.rollResults = Dice.RollDice();
      GameManager.Instance.gameState = GameState.ResourceRollDone;
   }

   public void OnConfirmClick()
   {
      GameManager.Instance.gameState = GameState.PlaceThiefDone;
   }

   public void OnCancelClick()
   {

   }
}
