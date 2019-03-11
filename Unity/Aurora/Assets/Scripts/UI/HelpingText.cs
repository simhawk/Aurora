using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HelpingText : MonoBehaviour
{

    Text txt;
    
    // Start is called before the first frame update
    void Start()
    {
        txt = gameObject.GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        GameState gameState = GameManager.Instance.gameState;
        Player activePlayer = GameManager.Instance.activePlayer;

        switch(gameState)
        {
            case GameState.InitialSettlementPlacement:
                txt.text = activePlayer.name + ", pick a settlement location."; break;
            case GameState.InitialRoadPlacement:
                txt.text = activePlayer.name + " now pick an adjacent road location."; break;
            case GameState.ResourceRoll:
                txt.text = activePlayer.name + ", it is your turn to roll the dice!"; break;
            case GameState.ResourceRollDone:
                txt.text = activePlayer.name + ", You rolled a " + GameManager.Instance.rollResults.Item1 + "! Collect your resources!";  break;
            case GameState.PlaceThief:
                txt.text = "You rolled a 7! " + activePlayer.name + " place the thief on any tile to disable it!" ; break;
            default:
            break;
        }
    }
}
