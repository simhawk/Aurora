using System.Collections;
using System.Collections.Generic;
using UnityEngine;

 [ExecuteInEditMode]
public class Board : MonoBehaviour
{
    public enum BoardModeEnum {Normal, SingleLayered}
    public BoardModeEnum BoardMode {get; set;}
    
    public Hex[] hexes;
    
    [SerializeField]
    private BoardModeEnum boardMode;
    private Dictionary<HexKey, Hex> hexHashMap = new Dictionary<HexKey, Hex>();

    public int x, y, z;

    void Start()
    {
        // This will be used during normal play
        if(!Application.isEditor) 
        {
            BoardMode = BoardModeEnum.Normal;
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
        if(selectedHex != null) {
            selectedHex.isSelected = !selectedHex.isSelected;
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

}
