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
                
                txt.text = activePlayer.name + ", pick a settlement location.";
            break;
            case GameState.InitialRoadPlacement:
                txt.text = activePlayer.name + " now pick an adjacent road location.";
            break;
            default:
            break;
        }
    }
}
