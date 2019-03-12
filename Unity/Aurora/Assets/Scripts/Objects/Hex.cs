using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
 

public class Hex : MonoBehaviour
{
    public bool isSelected = false;
    public Resource resource;
    private static System.Random random = new System.Random();
    private NumberHolder numberHolder;

    public bool isThiefOnHex = false;


    [SerializeField]
    public int x, y, z;

    public float maxSettlementAdjacencyDistance = 3.5f;

    // The q, r, and s axes of the Hex indicate the cubic positions of the Hex itself
    // The coordinate invariant is q+r+s=0
    // Start is called before the first frame update
    void Start()
    {
        BoardMode boardMode = GameManager.Instance.boardMode;
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

    /// <summary>
    /// Update is called every frame, if the MonoBehaviour is enabled.
    /// </summary>
    void Update()
    {
        
        MeshRenderer[] meshRenderers = transform.GetComponentsInChildren<MeshRenderer>();
        foreach(MeshRenderer mesh in meshRenderers) 
        {
            if(mesh.transform.tag.Equals("Selector"))
            {
                mesh.enabled = isSelected;
            }
            if(mesh.transform.tag.Equals("Thief"))
            {
                mesh.enabled = isThiefOnHex;
            }
        }

    }
    
    public int getNumber()
    {
        NumberHolder[] numberHolders = FindObjectsOfType(typeof(NumberHolder)) as NumberHolder[];
        float minDistance = 10000;
        NumberHolder minNumberHolder = numberHolders[0];
        foreach(NumberHolder numberHolder in numberHolders)
        {
            float distance = Vector3.Distance(transform.position, numberHolder.transform.position);
            if(distance < minDistance)
            {
                minNumberHolder = numberHolder;
                minDistance = distance;
            }
        }
        return minNumberHolder.rollNumber;
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
    
    public List<Settlement> GetAdjacentSettlements()
    {
        Settlement[] settlements = FindObjectsOfType(typeof(Settlement)) as Settlement[];
        List<Settlement> adjacents = new List<Settlement>();
        foreach(Settlement settlement in settlements)
        {
            float distance = Vector3.Distance(transform.position, settlement.transform.position);
            if (distance < maxSettlementAdjacencyDistance)
            {
                adjacents.Add(settlement);
            }
        }    
        return adjacents;
    }
}
