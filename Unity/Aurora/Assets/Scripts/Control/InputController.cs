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
    
    public void Update()
    {
        /*Getting random index out of bounds error */
        // foreach(Touch touch in Input.touches)
        // {
        //     if (touch.fingerId == 0)
        //     {
        //         if (Input.GetTouch(0).phase == TouchPhase.Began)
        //         {
        //             Debug.Log("First finger entered!");
        //         }
        //         if (Input.GetTouch(0).phase == TouchPhase.Ended)
        //         {
        //             Debug.Log("First finger left.");
        //         }
        //     }

        //     if (touch.fingerId == 1)
        //     {
        //         if (Input.GetTouch(1).phase == TouchPhase.Began)
        //         {
        //             Debug.Log("Second finger entered!");
        //         }
        //         if (Input.GetTouch(1).phase == TouchPhase.Ended)
        //         {
        //             Debug.Log("Second finger left.");
        //         }
        //     }
        // }
    }
}
