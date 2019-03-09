using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Settlement : MonoBehaviour
{

    private static System.Random random = new System.Random();


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public static Resource getRandomResource()
    {
        System.Array resources = Resource.GetValues(typeof(Resource));
        Resource randomResource = (Resource)resources.GetValue(random.Next(0,resources.Length-1));
        return randomResource;
    }

     private void InstantiateSettlementWith(Resource resource) {
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
