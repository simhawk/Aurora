using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UserInterface : MonoBehaviour
{
   public GameObject helpingText;
   public GameObject rollButton;
  
    void Update()
    {
       GameState state = GameManager.Instance.gameState;
       switch(state)
       {
           case GameState.InitialSettlementPlacement:
           case GameState.InitialRoadPlacement:
                DisableUIElement(rollButton);
                EnableUIElement(helpingText);
           break;

           case GameState.ResourceRoll:
                EnableUIElement(rollButton);
                EnableUIElement(helpingText);
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
