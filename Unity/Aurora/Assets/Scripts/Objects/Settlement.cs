using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Settlement : MonoBehaviour
{

    private static System.Random random = new System.Random();
    private CivType civType;
    private bool isUpgraded; 

    GameObject settlement; // This object holds the mesh for displaying the settlement game piece

    // Start is called before the first frame update
    void Start()
    {
        civType = CivType.Noble;
        isUpgraded = false;
    }

    public void placeSettlementWithActiveCiv(bool isUpgraded)
    {
        this.isUpgraded = isUpgraded;
        civType = GameManager.Instance.activePlayer.civType;
        ReplaceSettlement();
    }

    public void placeSettlementWithRandomCiv(bool isUpgraded)
    {
        this.isUpgraded = isUpgraded;
        System.Array civilizations = CivType.GetValues(typeof(CivType));
        CivType randomCiv = (CivType)civilizations.GetValue(random.Next(0,civilizations.Length));
        this.civType = randomCiv;
        ReplaceSettlement();
    }

     private void ReplaceSettlement() {
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

}
