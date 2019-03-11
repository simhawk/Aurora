using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pulsing : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float scaleValue = 0.83f+ 0.02f*Mathf.Sin(4*Time.time);
        this.transform.localScale = new Vector3(scaleValue, scaleValue, scaleValue);
    }
}
