using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NumberHolder : MonoBehaviour
{
    public Color ScarceColor;
    public Color CommonColor;

    public int number;
    private static System.Random random = new System.Random();  
    
    /// <summary>
    /// Called when the script is loaded or a value is changed in the
    /// inspector (Called in the editor only).
    /// </summary>
    void OnValidate()
    {
        // Get Resource Number child object
        TextMesh ResourceNumberTextMesh = gameObject.transform.GetChild(0).GetComponent<TextMesh>();
        TextMesh DotTextMesh = gameObject.transform.GetChild(1).GetComponent<TextMesh>();

        string dotText = ".";
        Color textColor = ScarceColor;
        

        if(number == 2 || number == 12)
        {
            dotText = ".";
        }
        else if(number == 3 || number == 11)
        {
            dotText = "..";
        }
        else if(number == 4 || number == 10)
        {
            dotText = "...";
        }
        else if(number == 5 || number == 9)
        {
            dotText = "....";
        }
        else if(number == 6 || number == 7 || number == 8)
        {
            dotText = ".....";
            textColor = CommonColor;
        }

        if(ResourceNumberTextMesh != null) {
            ResourceNumberTextMesh.text = number.ToString();
            ResourceNumberTextMesh.color = textColor;
        }

        if(DotTextMesh != null) {
            DotTextMesh.text = dotText;
            DotTextMesh.color = textColor;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        number = random.Next(2,12);

         // Get Resource Number child object
        TextMesh ResourceNumberTextMesh = gameObject.transform.GetChild(0).GetComponent<TextMesh>();
        TextMesh DotTextMesh = gameObject.transform.GetChild(1).GetComponent<TextMesh>();

        string dotText = ".";
        Color textColor = ScarceColor;
        

        if(number == 2 || number == 12)
        {
            dotText = ".";
        }
        else if(number == 3 || number == 11)
        {
            dotText = "..";
        }
        else if(number == 4 || number == 10)
        {
            dotText = "...";
        }
        else if(number == 5 || number == 9)
        {
            dotText = "....";
        }
        else if(number == 6 || number == 7 || number == 8)
        {
            dotText = ".....";
            textColor = CommonColor;
        }

        if(ResourceNumberTextMesh != null) {
            ResourceNumberTextMesh.text = number.ToString();
            ResourceNumberTextMesh.color = textColor;
        }

        if(DotTextMesh != null) {
            DotTextMesh.text = dotText;
            DotTextMesh.color = textColor;
        }
    }

    // Update is called once per frame
    void Update()
    {
         
    }
}
