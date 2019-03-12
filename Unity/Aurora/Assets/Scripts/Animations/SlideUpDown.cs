using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlideUpDown : MonoBehaviour
{
    public enum UpDirection {Left, Down, Up, Right};

    public float distance;
    public float speed;
    public UpDirection upDirection;

    private Vector3 shownPosition;
    private Vector3 hiddenPosition;
    private Vector3 target;
    
    private bool hidden =true;


    public void toggleView()
    {
        target = hidden ? shownPosition : hiddenPosition;
        hidden = !hidden;
    }

    // Assumes initialized at hidden position, direction should be direction to go from hidden to shown
    void Start()
    {
        this.hiddenPosition = this.gameObject.transform.position;
        target = hiddenPosition;

        if(upDirection == UpDirection.Up)
        {
            this.shownPosition = this.hiddenPosition + Vector3.up * distance;
        } 
        else if(upDirection == UpDirection.Down)
        {
            this.shownPosition = this.hiddenPosition + Vector3.down * distance;
        } 
        else if(upDirection == UpDirection.Left)
        {
            this.shownPosition = this.hiddenPosition + Vector3.left * distance;
        } 
        else 
        {
            this.shownPosition = this.hiddenPosition + Vector3.right * distance;
        }
        
    }

    void Update()
    {    
        // Check if the position of the cube and sphere are approximately equal.
        if (Vector3.Distance(transform.position, target) > 0.001f)
        {
            float step =  speed * Time.deltaTime; // calculate distance to move
            transform.position = Vector3.MoveTowards(transform.position, target, step);
        }
    }
}
