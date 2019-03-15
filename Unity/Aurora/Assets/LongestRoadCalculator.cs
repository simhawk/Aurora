using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LongestRoadCalculator : MonoBehaviour
{

    // anyone who breaks this floor automatically increases the floor,
    // if longest road is broken, then follow the following rules
    // if no one has this value the longest road anymore it resets back to 4 and no one has the longest road,
    // if you are tied, then you keep the longest road,
    //
    public int LongestRoadFloor = 4;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void CalculateLongestRoad()
    {
        Road[] roads = FindObjectsOfType(typeof(Road)) as Road[];

        foreach(Road road in roads)
        {

        }

    }

    public Player whoGetsLongestRoad()
    {
        // TODO: actually calculate longest roads
        
        return null;
    }
}
