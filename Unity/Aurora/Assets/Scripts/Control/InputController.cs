
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputController : MonoBehaviour
{
    public static InputController Instance { get; private set; }
    public bool usingTouchScreen = true;
    
    private Vector3 position;
    private float width;
    private float height;

    public float distanceForRobinHood;
    public float distanceForThief;
    public float margin;

    private bool detectedRobinHood = false;
    private bool detectedThief = false;

    private Vector2 thiefPosition;
    private Vector2 robinHoodPosition;

    public Vector2 getThiefPosition()
    {
        return thiefPosition;
    }
     
    public Vector2 getRobinHoodPosition()
    {
        return robinHoodPosition;
    }

    public bool DetectedRobinHood()
    {
        return detectedRobinHood;
    }

    public bool DetectedThief()
    {
        return detectedThief;
    }




    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);

            width = (float)Screen.width / 2.0f;
            height = (float)Screen.height / 2.0f;

            // Position used for the cube.
            position = new Vector3(0.0f, 0.0f, 0.0f);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public float distanceBetween(Touch a, Touch b)
    {
        float dx = a.position.x - b.position.x;
        float dy = a.position.y - b.position.y;

        return Mathf.Sqrt(dx*dx+dy*dy);
    }

    public Vector2 positionBetween(Touch a, Touch b)
    {
        float ax = a.position.x;
        float ay = a.position.y;
        float bx = b.position.x;
        float by = b.position.y;

        return new Vector2((ax+bx)/2, (ay+by)/2);
    }
    
    public void Update()
    {
        for(int i = 0; i < Input.touches.Length; i++)
        {
            Touch touch = Input.touches[i];
            Debug.Log("I'm touch number:" + i + "at point: (" + touch.deltaPosition.ToString());
        }


        Input.multiTouchEnabled = true;
        HelpingText.AdditionalText = "TouchCount " + Input.touchCount;


        foreach(Touch a in Input.touches) 
        {
            foreach(Touch b in Input.touches)
            {
                // disregard if they are the same input
                if(a.Equals(b)) continue;

                float distance = distanceBetween(a,b);

                // code for detecting robin hood
                if(Mathf.Abs(distance - distanceForRobinHood) < margin)
                {
                    //there is a robin hood at mean position
                    robinHoodPosition = positionBetween(a,b);
                    detectedRobinHood = true;
                } else if(Mathf.Abs(distance - distanceForThief) < margin)
                {
                    //there is a thief at mean position
                    thiefPosition = positionBetween(a,b);
                    detectedThief = true;
                }
                else 
                {
                    detectedThief = false;
                    detectedRobinHood = false;
                }
            }
        }
    }
}
