using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NumberHolder : MonoBehaviour
{
    public Color ScarceColor;
    public Color CommonColor;

    public int rollNumber;
    private static System.Random random = new System.Random();  
    
    /// <summary>
    /// Called when the script is loaded or a value is changed in the
    /// inspector (Called in the editor only).
    /// </summary>
    void OnValidate()
    {
        setupText();
    }

    private void setupText()
    {
        // Get Resource Number child object
        TextMesh ResourceNumberTextMesh = gameObject.transform.GetChild(0).GetComponent<TextMesh>();
        TextMesh DotTextMesh = gameObject.transform.GetChild(1).GetComponent<TextMesh>();

        string dotText = ".";
        Color textColor = ScarceColor;
        

        if(rollNumber == 2 || rollNumber == 12)
        {
            dotText = ".";
        }
        else if(rollNumber == 3 || rollNumber == 11)
        {
            dotText = "..";
        }
        else if(rollNumber == 4 || rollNumber == 10)
        {
            dotText = "...";
        }
        else if(rollNumber == 5 || rollNumber == 9)
        {
            dotText = "....";
        }
        else if(rollNumber == 6 || rollNumber == 8)
        {
            dotText = ".....";
            textColor = CommonColor;
        }

        if(ResourceNumberTextMesh != null) {
            ResourceNumberTextMesh.text = rollNumber.ToString();
            ResourceNumberTextMesh.color = textColor;
        }

        if(DotTextMesh != null) {
            DotTextMesh.text = dotText;
            DotTextMesh.color = textColor;
        }
    }

    private static int getRandomRollNumber()
    {
        int randomNumber = random.Next(2,11);
        if(randomNumber >= 7) 
            randomNumber++;
        return randomNumber;
    }

    private static int getRandomPooledNumber()
    {
        List<int> rollNumbers = GameManager.Instance.startingNumbers;
        int randomListIndex = random.Next(0,rollNumbers.Count);
        int randomNumberFromList = rollNumbers[randomListIndex];
        rollNumbers.RemoveAt(randomListIndex);
        return randomNumberFromList;
    }

    // Start is called before the first frame update
    void Start()
    {
        BoardMode boardMode = GameManager.Instance.boardMode;
        switch(boardMode)
        {
            case BoardMode.Random:
                this.rollNumber = getRandomRollNumber();
            break;
            case BoardMode.RandomPooled:
                this.rollNumber = getRandomPooledNumber();
            break;
            default: 
                this.rollNumber = getRandomPooledNumber();
            break;
        }
        setupText();
    }

    // Update is called once per frame
    void Update()
    {
         
    }
}
