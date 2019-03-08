using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
 

public class Hex : MonoBehaviour
{
    public bool isSelected = false;

    public enum Resource {Brick, Sheep, Wheat, Rock, Wood, Desert}

    public Resource resource;
    private static System.Random random = new System.Random();

    [SerializeField]
    public int x, y, z;

    // The q, r, and s axes of the Hex indicate the cubic positions of the Hex itself
    // The coordinate invariant is q+r+s=0
    
    // Start is called before the first frame update
    void Start()
    {
        resource = getRandomResource();
        redrawResource();
    }

      // Update is called once per frame
    void Update()
    {
       
    }

    private void redrawResource() {
        MeshRenderer mesh = gameObject.GetComponent<MeshRenderer>();
        if(mesh != null && mesh.sharedMaterial != null) 
        {
            mesh.sharedMaterial.color = GetResourceColor(resource);
        } 
        else
        {
            Debug.Log("Error: Could not render hex material mesh color");
        }
    }

    public static Resource getRandomResource()
    {
        System.Array resources = Resource.GetValues(typeof(Resource));
        Resource randomResource = (Resource)resources.GetValue(random.Next(resources.Length));
        return randomResource;
    }

    private Color GetResourceColor(Resource resource) {
        Color color = Color.magenta;
        switch(resource)
        {
            case Resource.Brick: color = new Color(181/255.0f, 53/255.0f, 33/255.0f); break;
            case Resource.Sheep: color = new Color(153/255f, 1f, 102/255f); break;
            case Resource.Wheat: color = Color.yellow; break;
            case Resource.Rock: color = new Color(0.2f,0.2f,0.2f); break;
            case Resource.Wood: color = new Color(0, 82/255f, 0); break; // brownish
            case Resource.Desert: color = new Color(237/255.0f, 214/255.0f, 155/255.0f); break; // beigish
        }
        return color;
    }

   
}
