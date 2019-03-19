using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HelpingText : MonoBehaviour
{

    TextMeshProUGUI txt;
    
    public static string AdditionalText = "";
    bool Debuging = false;
    // Start is called before the first frame update
    void Start()
    {
        txt = GetComponent<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {
        GameState gameState = GameManager.Instance.gameState;
        Player activePlayer = GameManager.Instance.activePlayer;

        string text;

        switch(gameState)
        {
            case GameState.InitialSettlementPlacement:
                text = activePlayer.DisplayName + ", pick a settlement location."; break;
            case GameState.InitialRoadPlacement:
                text = activePlayer.DisplayName + " now pick an adjacent road location."; break;
            case GameState.ResourceRoll:
                text = activePlayer.DisplayName + ", it is your turn to roll the dice!"; break;
            case GameState.ResourceRollDone:
                text = activePlayer.DisplayName + ", You rolled a " + GameManager.Instance.rollResults.Item1 + "! Collect your resources!";  break;
            case GameState.BuildOrDevelopmentCard:
                text =  "You rolled a " + GameManager.Instance.rollResults.Item1 + "Build?"; break;
            case GameState.PlaceThief:
                text = "You rolled a 7! " + activePlayer.DisplayName + " place the thief on any tile to disable it!" ; break;
            default:
                text = "";
            break;
        }

if(Debuging)
{
    txt.SetText("GameState = " + GameManager.Instance.gameState.ToString() + System.Environment.NewLine + text + string.Concat(System.Environment.NewLine,AdditionalText));
   
}else {
txt.SetText(text);
   
}
     }
}
