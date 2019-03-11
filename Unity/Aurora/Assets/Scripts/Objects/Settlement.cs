using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Settlement : MonoBehaviour
{

    private static System.Random random = new System.Random();
    private CivType civType;
    private bool isUpgraded; 
    private bool isPlaced;

    [SerializeField]
    private float maxSelfAdjacencyDistance = 4f;

    GameObject settlement; // This object holds the mesh for displaying the settlement game piece

    // Start is called before the first frame update
    void Start()
    {
        isPlaced = false;
        isUpgraded = false;
    }

    public void placeSettlementWithActiveCiv(bool isUpgraded)
    {
        this.isUpgraded = isUpgraded;
        civType = GameManager.Instance.activePlayer.civType;
        ReplaceSettlement();
        MakeBaseVisible();
    }

    public void placeSettlementWithRandomCiv(bool isUpgraded)
    {
        this.isUpgraded = isUpgraded;
        System.Array civilizations = CivType.GetValues(typeof(CivType));
        CivType randomCiv = (CivType)civilizations.GetValue(random.Next(0,civilizations.Length));
        this.civType = randomCiv;
        ReplaceSettlement();
        MakeBaseVisible();
    }

    public bool IsPlaced()
    {
        return isPlaced;
    }

     private void ReplaceSettlement() {
        
        isPlaced = true;

        // destroy any existing mesh that is there (if upgrading for instance)
        Destroy(settlement);
        settlement = null;

        if(this.isUpgraded)
        {
            switch(this.civType)
            {
                case CivType.Fisherman: settlement = Instantiate(Resources.Load("Prefabs/City_Fisherman")) as GameObject; break;
                case CivType.Knight: settlement = Instantiate(Resources.Load("Prefabs/City_Knight")) as GameObject; break;
                case CivType.Noble: settlement = Instantiate(Resources.Load("Prefabs/City_Noble")) as GameObject; break;
                case CivType.Samurai: settlement = Instantiate(Resources.Load("Prefabs/City_Samurai")) as GameObject; break;
                case CivType.Viking: settlement = Instantiate(Resources.Load("Prefabs/City_Viking")) as GameObject; break;
                
                default: settlement = Instantiate(Resources.Load("Prefab/City_Viking")) as GameObject; break;  
            }
        }
        else 
        {
            switch(this.civType)
            {
                case CivType.Fisherman: settlement = Instantiate(Resources.Load("Prefabs/Settlement_Fisherman")) as GameObject; break;
                case CivType.Knight: settlement = Instantiate(Resources.Load("Prefabs/Settlement_Knight")) as GameObject; break;
                case CivType.Noble: settlement = Instantiate(Resources.Load("Prefabs/Settlement_Noble")) as GameObject; break;
                case CivType.Samurai: settlement = Instantiate(Resources.Load("Prefabs/Settlement_Samurai")) as GameObject; break;
                case CivType.Viking: settlement = Instantiate(Resources.Load("Prefabs/Settlement_Viking")) as GameObject; break;
                
                default: settlement = Instantiate(Resources.Load("Prefab/Settlement_Viking")) as GameObject; break;  
            }
        }
       
        settlement.transform.position = this.transform.position;
        settlement.transform.rotation = this.transform.rotation;
    }

    public List<Settlement> GetAdjacentSettlements()
    {
        Settlement[] settlements = FindObjectsOfType(typeof(Settlement)) as Settlement[];
        List<Settlement> adjacents = new List<Settlement>();
        foreach(Settlement settlement in settlements)
        {
            float distance = Vector3.Distance(transform.position, settlement.transform.position);
        
            if (distance < maxSelfAdjacencyDistance)
            {
                adjacents.Add(settlement);
            }
        }    
        return adjacents;
    }

    public CivType GetCivType()
    {
        return this.civType;
    }

    // Current settlement is placable if there are no active adjacent settlements
    public bool isPlaceable()
    {
        List<Settlement> adjacents = GetAdjacentSettlements();
        foreach(Settlement settlement in adjacents)
        {
           if(settlement.IsPlaced() == true)
           {
               return false;
           }
        }   
        return true; 
    }

    // public bool isAdjacentTo(Settlemenet settlement)
    // {
    //     List<Settlement> adjacents = this.GetAdjacentSettlements();
    //     return adjacents.Contains(settlement);
    // }

   
    
    private void MakeBaseVisible()
    {
        foreach(Transform child in transform)
        {
            if(child.tag == "Base")
            {
                MeshRenderer meshComponent = child.GetComponent<MeshRenderer>();
                meshComponent.enabled = true;
            }
        }   
    }


/****Debug *****/
 public static float[] GetMinMaxAvgDistancesToAllSettlements()
    {
        float minDistance = 10000;
        float maxDistance = 0;
        float totalDistance = 0;
        int count = 0;
        Settlement[] settlements = FindObjectsOfType(typeof(Settlement)) as Settlement[];
        
        foreach(Settlement settlement1 in settlements) 
        {
            foreach(Settlement settlement2 in settlements)
            {
                if(settlement1 == settlement2) continue;

                float distance = Vector3.Distance(settlement1.transform.position, settlement2.transform.position);
            
                if (distance < minDistance )
                {
                    minDistance = distance;
                }

                if (distance > maxDistance )
                {
                    maxDistance = distance;
                }

                totalDistance += distance; 
                count++;
            }    
         }
         return new float[3] {minDistance, maxDistance, totalDistance/count};
    }
  
}