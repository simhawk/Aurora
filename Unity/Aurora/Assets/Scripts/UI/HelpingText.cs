using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HelpingText : MonoBehaviour
{

    TextMeshProUGUI txt;
    
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
                text = activePlayer.name + ", pick a settlement location."; break;
            case GameState.InitialRoadPlacement:
                text = activePlayer.name + " now pick an adjacent road location."; break;
            case GameState.ResourceRoll:
                text = activePlayer.name + ", it is your turn to roll the dice!"; break;
            case GameState.ResourceRollDone:
                text = activePlayer.name + ", You rolled a " + GameManager.Instance.rollResults.Item1 + "! Collect your resources!";  break;
            case GameState.PlaceThief:
                text = "You rolled a 7! " + activePlayer.name + " place the thief on any tile to disable it!" ; break;
            default:
                text = "";
            break;
        }

        txt.SetText(text);
    }
}
