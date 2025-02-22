using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GridBuildingSystem : MonoBehaviour
{
    public static GridBuildingSystem current;

    public GridLayout gridLayout;
    public Tilemap MainTilemap;
    public Tilemap TempTilemap;          

    public static Dictionary<TileType, TileBase> tileBase = new Dictionary<TileType, TileBase>();

    #region Unity Methods
    void Awake(){
        current = this;
    }

    void Start()
    {
        
    }


    void Update()
    {
        
    }
    #endregion

    #region Tilemap Management

    #endregion

    #region Building Placement

    #endregion

    public enum TileType{
        Empty,
        White,
        Green,
        Red
    }
}
