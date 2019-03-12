using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pulsingWithWeirdScale : MonoBehaviour
{
    private Vector3 initialScale;
    public float a;
    public float b;
    public float freq = 4;
    // Start is called before the first frame update
    void Start()
    {
        initialScale = transform.localScale;
    }

    // Update is called once per frame
    void Update()
    {
        float scaleValue = a+b*Mathf.Sin(freq*Time.time);
        this.transform.localScale = initialScale + new Vector3(initialScale.x*scaleValue, initialScale.y, initialScale.z*scaleValue);
    }
}
