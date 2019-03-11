using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveAroundPoint : MonoBehaviour
{

    public float speed;
    public GameObject moveAroundObject;
    
    private Vector3 initialPosition;
    private Vector3 initialEulerAngles;

    // Start is called before the first frame update
    void Start()
    {
        initialPosition = this.transform.position;
        initialEulerAngles = this.transform.eulerAngles;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 anchor = moveAroundObject.transform.position;   
        Vector3 delta = this.transform.position - anchor; 
        this.transform.RotateAround(anchor,Vector3.up, speed);

    }
}
