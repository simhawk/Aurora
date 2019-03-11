using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UserInterface : MonoBehaviour
{
   public GameObject helpingTextObj;
   public GameObject buttonObj;
   public Button button;
  
    /// <summary>
    /// Start is called on the frame when a script is enabled just before
    /// any of the Update methods is called the first time.
    /// </summary>
    void Start()
    {

    }

    void Update()
    {
       GameState state = GameManager.Instance.gameState;
       Text ButtonText = buttonObj.GetComponentInChildren<Text>();
       switch(state)
       {
           case GameState.InitialSettlementPlacement:
           case GameState.InitialRoadPlacement:
                DisableUIElement(buttonObj);
                EnableUIElement(helpingTextObj);
           break;

           case GameState.ResourceRoll:
                ButtonText.text = "Roll";
                EnableUIElement(buttonObj);
                EnableUIElement(helpingTextObj);
           break;

           case GameState.ResourceRollDone:
                DisableUIElement(buttonObj);
                EnableUIElement(helpingTextObj);
           break;

            case GameState.PlaceThief:
                ButtonText.text = "Confirm";
                EnableUIElement(buttonObj);
                EnableUIElement(helpingTextObj);
           break;
       }
    }    

    private void DisableUIElement(GameObject element)
    {
       element.SetActive(false);
    }

    private void EnableUIElement(GameObject element)
    {
       element.SetActive(true);
    }
}
