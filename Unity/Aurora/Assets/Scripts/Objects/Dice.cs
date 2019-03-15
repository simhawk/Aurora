using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dice : MonoBehaviour
{
    private static System.Random random = new System.Random();
    
    public static Tuple<int, int, int> RollDice()
    {
        int firstDie = random.Next(1,7);        
        int secondDie = random.Next(1,7);
        return new Tuple<int, int, int>(firstDie+secondDie, firstDie, secondDie);
    }
}
