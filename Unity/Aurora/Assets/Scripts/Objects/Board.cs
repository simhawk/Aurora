using System;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class Board : MonoBehaviour
{ 
    public BoardMode BoardMode {get; set;}
    public Hex[] hexes;
    
    [SerializeField]
    private BoardMode boardMode;
    private Dictionary<HexKey, Hex> hexHashMap = new Dictionary<HexKey, Hex>();

    public int x, y, z;

    void Start()
    {
        // This will be used during normal play
        if(!Application.isEditor) 
        {
            BoardMode = BoardMode.Normal;
        }

        InitializeHexes();
    }

    /// <summary>
    /// Called when the script is loaded or a value is changed in the
    /// inspector (Called in the editor only).
    /// </summary>
    void OnValidate()
    {
        BoardMode = boardMode;

        Hex selectedHex = this.GetHexAt(x, y, z);
        if(selectedHex != null)
        {
            List<Hex> nearHexes = getAdjacentHexes(selectedHex);
            foreach(Hex hex in nearHexes)
            {
                
            }

        }
        
    } 


    private void InitializeHexes() 
    {
        for(int i = 0; i < hexes.Length; i++)
        {
            Hex hex = hexes[i];
            
            if(hex == null) {
                Debug.Log("found null inbetween end");
                continue;
            }

            HexKey key = new HexKey(hex.x, hex.y, hex.z);
            
            if(hexHashMap.ContainsKey(key)) {
                Debug.Log("Hex Hash Map already contains key: ("+x + ", " + y + ", " + z + ")");
                continue;
            }
            
            Debug.Log("Added hex key: ("+x + ", " + y + ", " + z + ") to hex hash map");
            hexHashMap.Add(key, hex);

        }
    }

    public Hex GetHexAt(int x, int y, int z) 
    {
        HexKey key = new HexKey(x, y, z);

        if(hexHashMap.ContainsKey(key)) {
            return hexHashMap[key];
        }

        return null;
    }

    public static int getDistanceBetweenHexes(Hex a, Hex b)
    {
        return System.Math.Max(Math.Max(Math.Abs(a.x - b.x), Math.Abs(a.y - b.y)), Math.Abs(a.z - b.z));
    }

    public bool areHexesAdjacent(Hex a, Hex b) 
    {
        return getDistanceBetweenHexes(a, b) == 1;
    }

    public List<Hex> getAdjacentHexes(Hex hex) 
    {
        List<int> cubicDeltas = new List<int>(new int[]{1, -1, 0});
        List<Hex> adjacentHexes = new List<Hex>();

        var permutations = cubicDeltas.Permutations();

        foreach (var list in permutations)
        {
            List<int> indices = list.ToList();
            Hex adjacentHex = GetHexAt(hex.x + indices[0], hex.y + indices[1], hex.z + indices[2]);
            adjacentHexes.AddIfNotNull(adjacentHex);
        }
        return adjacentHexes;
    }
}
