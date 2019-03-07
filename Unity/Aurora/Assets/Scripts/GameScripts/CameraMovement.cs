using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    public float translationSpeed = 5;
    public float rotationSpeed = 3.0f;
    
    private float yaw = 0.0f;
    private float pitch = 0.0f;


    // Start is called before the first frame update
    void Start()
    {
        translationSpeed = 10;
        rotationSpeed = 4.0f;
    }

    // Update is called once per frame
    void Update()
    {
        // translate camera on WASD 
        if(Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D))
        {
            transform.Translate(new Vector3(translationSpeed * Time.deltaTime,0,0));
        }
        if(Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A))
        {
            transform.Translate(new Vector3(-translationSpeed * Time.deltaTime,0,0));
        }
        if(Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.S))
        {
            transform.Translate(new Vector3(0,0,-translationSpeed * Time.deltaTime));
        }
        if(Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.W))
        {
            transform.Translate(new Vector3(0,0,translationSpeed * Time.deltaTime));
        }

        // rotate camera on right mouse button down
        if(Input.GetMouseButton(1)) 
        {
            yaw += rotationSpeed * Input.GetAxis("Mouse X");
            pitch -= rotationSpeed * Input.GetAxis("Mouse Y");
            transform.eulerAngles = new Vector3(pitch, yaw, 0);
        }
    }
}
