using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateToPlayer : MonoBehaviour
{
    private Quaternion InitialRotation;
    
    // Start is called before the first frame update
    void Start()
    {
        InitialRotation = transform.rotation;    
    }

    // Update is called once per frame
    void Update()
    {
        transform.eulerAngles = new Vector3(0, GameManager.Instance.activePlayer.orientation,0);
    }
}
