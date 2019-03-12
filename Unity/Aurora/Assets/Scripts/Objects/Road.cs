using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Road : MonoBehaviour
{

    private CivType civType;
    GameObject road;
    public ParticleSystem puff;

    private bool isPlaced = false;
    public bool isSelected = false;

    [SerializeField]
    private float maxSettlementAdjacencyDistance = 2.5f;

    [SerializeField]
    private float maxRoadAdjacencyDistance = 4f;
    // Start is called before the first frame update
    void Start()
    {
        civType = CivType.Noble;
    }

    // Update is called once per frame
    void Update()
    {
        MeshRenderer[] meshRenderers = transform.GetComponentsInChildren<MeshRenderer>();
        foreach(MeshRenderer mesh in meshRenderers) 
        {
            if(mesh.transform.tag.Equals("Selector"))
            {
                mesh.enabled = isSelected;
            }
        }
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

    public List<Road> GetAdjacentRoads()
    {
        Road[] roads = FindObjectsOfType(typeof(Road)) as Road[];
        List<Road> adjacents = new List<Road>();
        foreach(Road road in roads)
        {
            float distance = Vector3.Distance(transform.position, road.transform.position);
            if (distance < maxRoadAdjacencyDistance)
            {
                adjacents.Add(road);
            }
        }    
        return adjacents;
    }
    
    public bool IsPlaced()
    {
        return isPlaced;
    }

    public CivType GetCivType() 
    {
        return civType;
    }

    // Current settlement is placable if there are no active adjacent settlements
    public bool isPlaceable()
    {
        // road isn't placeable if its already active
        if(isPlaced)
        {
            return false;
        }

        List<Settlement> adjacentSettlements = GetAdjacentSettlements();
        foreach(Settlement settlement in adjacentSettlements)
        {
            // only allow if there's at least one civ of the same type nearby or a road of the same type
           if(settlement.IsPlaced() && settlement.GetCivType() == GameManager.Instance.activePlayer.civType)
           {
               return true;
           }
        }  

        List<Road> adjacentRoads = GetAdjacentRoads();
        foreach(Road road in adjacentRoads)
        {
            // only allow if there's at least one civ of the same type nearby or a road of the same type
           if(road.IsPlaced() && road.GetCivType() == GameManager.Instance.activePlayer.civType)
           {
               return true;
           }
        }  
         
        return false; 
    }

    public void placeRoadWithActiveCiv()
    {
        puff.Play();
        
        this.isPlaced = true;
        civType = GameManager.Instance.activePlayer.civType;
        MeshRenderer mesh = gameObject.GetComponentInChildren<MeshRenderer>();
        mesh.enabled = true;

        switch(this.civType)
        {
            case CivType.Fisherman: mesh.material = Resources.Load("Materials/Road_Fisherman") as Material; break;
            case CivType.Knight: mesh.material = Resources.Load("Materials/Road_Knight") as Material; break;
            case CivType.Noble: mesh.material = Resources.Load("Materials/Road_Noble") as Material; break;
            case CivType.Samurai: mesh.material = Resources.Load("Materials/Road_Samurai") as Material; break;
            case CivType.Viking: mesh.material = Resources.Load("Materials/Road_Viking") as Material; break;
            
            default:  mesh.material = Resources.Load("Materials/Road_Viking") as Material; break;  
        }
    }

    
}
