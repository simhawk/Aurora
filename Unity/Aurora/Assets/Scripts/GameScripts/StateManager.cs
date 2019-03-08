using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateManager : MonoBehaviour
{

    public StateManager Instance { get; private set;}
    public PieceSetupEnum pieceSetup;

    void Awake()
    {
        if(Instance == null) 
        {
           Instance = this;
           DontDestroyOnLoad(gameObject);
        } 
        else 
        {
           Destroy(gameObject);
        }
    }

}
