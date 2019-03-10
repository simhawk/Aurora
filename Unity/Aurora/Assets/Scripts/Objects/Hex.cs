using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
 

public class Hex : MonoBehaviour
{
    public bool isSelected = false;
    public Resource resource;
    private static System.Random random = new System.Random();

    [SerializeField]
    public int x, y, z;

    // The q, r, and s axes of the Hex indicate the cubic positions of the Hex itself
    // The coordinate invariant is q+r+s=0
    
    // Start is called before the first frame update
    void Start()
    {
        BoardMode boardMode = GameManager.Instance.boardMode;
        Resource resource;
        switch(boardMode)
        {
            case BoardMode.Random:
                resource = getRandomResource();
            break;
            case BoardMode.RandomPooled:
                resource = getRandomPooledResource();
            break;
            default: 
                resource = getRandomPooledResource();
            break;
        }
        InstantiateHexWith(resource);
    }

    private static Resource getRandomResource()
    {
        System.Array resources = Resource.GetValues(typeof(Resource));
        Resource randomResource = (Resource)resources.GetValue(random.Next(0,resources.Length-1));
        return randomResource;
    }

    private static Resource getRandomPooledResource()
    {
        List<Resource> resourceList = GameManager.Instance.startingResources;
        int randomListIndex = random.Next(0,resourceList.Count);
        Resource randomResourceFromList = resourceList[randomListIndex];
        resourceList.RemoveAt(randomListIndex);
        return randomResourceFromList;
    }

    private void InstantiateHexWith(Resource resource) {
        GameObject hex;
        switch(resource)
        {
            case Resource.Brick: hex = Instantiate(Resources.Load("Prefabs/BrickHex")) as GameObject; break;
            case Resource.Sheep: hex = Instantiate(Resources.Load("Prefabs/SheepHex")) as GameObject; break;
            case Resource.Wheat: hex = Instantiate(Resources.Load("Prefabs/WheatHex")) as GameObject; break;
            case Resource.Rock: hex = Instantiate(Resources.Load("Prefabs/RockHex")) as GameObject; break;
            case Resource.Wood: hex = Instantiate(Resources.Load("Prefabs/WoodHex")) as GameObject; break;
            case Resource.Desert: hex = Instantiate(Resources.Load("Prefabs/DesertHex")) as GameObject; break; 
            case Resource.Water: hex = Instantiate(Resources.Load("Prefabs/WaterHex")) as GameObject; break; 
            default: hex = Instantiate(Resources.Load("Prefab/WoodHex")) as GameObject; break;  // On Default, use Wood
        }
        hex.transform.position = this.transform.position;
    }
}
