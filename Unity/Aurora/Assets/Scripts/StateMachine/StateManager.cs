using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateManager : MonoBehaviour
{

    public StateManager Instance { get; private set;}
    

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

    /*******************
       Game Properties
    *******************/


    /*******************
       Game Properties
    *******************/
    

}
