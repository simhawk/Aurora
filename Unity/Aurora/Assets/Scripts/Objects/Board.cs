using System;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class Board : MonoBehaviour
{ 
    public BoardMode BoardMode {get; set;}
    public BoardMode boardMode;
    public Hex[] hexes;
    
    [SerializeField]
    private Dictionary<HexKey, Hex> hexHashMap = new Dictionary<HexKey, Hex>();

    public int x, y, z;

    void Start()
    {
        // This will be used during normal play
        if(!Application.isEditor) 
        {
           // BoardMode = BoardMode.Normal;
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

    // Creates a hashmap for all the hexes to be accessed via this.GetHexAt()
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

    private Vector3 position;
    private float width;
    private float height;

    void Awake()
    {
        width = (float)Screen.width / 2.0f;
        height = (float)Screen.height / 2.0f;

        // Position used for the cube.
        position = new Vector3(0.0f, 0.0f, 0.0f);
    }


    void Update()
    {

        // //Update touch
        // Touch touch = Input.GetTouch(0);

        // // Move the cube if the screen has the finger moving.
        // if (touch.phase == TouchPhase.Moved)
        // {
        //     Vector2 pos = touch.position;
        //     pos.x = (pos.x - width) / width;
        //     pos.y = (pos.y - height) / height;
        //     position = new Vector3(-pos.x, pos.y, 0.0f);

        //     Debug.Log("x: " + pos.x);
        //     // Position the cube.
        //     // transform.position = position;
        // }

        //Input.getmouseBuytton() 0left, 1right, 2wheel)

        if(Input.GetMouseButtonUp(0))
        {
            
            //Create a ray from the Mouse click position
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        }
    }

}
