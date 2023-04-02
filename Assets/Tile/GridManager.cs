using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.Tilemaps;

[RequireComponent(typeof(GridMap))]
[RequireComponent(typeof(Tilemap))]
public class GridManager : MonoBehaviour
{
    Tilemap tilemap;
    GridMap grid;
    SaveLoadMap saveLoadMap;
    // [SerializeField] TileBase tileBase; 
    // [SerializeField] TileBase tileBase2;
    [SerializeField] TileSet tileSet;
    
    void Start()
    {
        tilemap = GetComponent<Tilemap>();
        grid = GetComponent<GridMap>();
        saveLoadMap = GetComponent<SaveLoadMap>();

        saveLoadMap.Load(grid);
        /*
        grid.Init(10, 8);
        for (int x=1; x<5; x++)
        {
          for(int y=1; y<5; y++)
          {
            Set(x, y, 2);
          }
        }
        */
        UpdateTileMap();
    }

    internal void Clear()
    {
        if (tilemap == null) { tilemap = GetComponent<Tilemap>(); }
        tilemap.ClearAllTiles();
        tilemap = null;
    }
    internal void SetTile(int x, int y, int tileId)
    {
      if (tileId == -1) {return;}
        if (tilemap == null) { tilemap = GetComponent<Tilemap>(); }
        tilemap.SetTile(new Vector3Int(x,y,0), tileSet.tiles[tileId]);
        tilemap = null;
    }

    void UpdateTileMap()
    {
        for(int x = 0; x < grid.length; x++)
        {
            for (int y = 0; y < grid.height; y++)
      {
        UpdateTile(x, y);
      }
    }
    }

  private void UpdateTile(int x, int y)
  {
    int tileId = grid.GetTile(x,y);//Get(x, y);
    if(tileId == -1)
    {
        return;
    }

    tilemap.SetTile(new Vector3Int(x,y,0), tileSet.tiles[tileId]);
    /*
    if (grid.Get(x, y))
    {
      tilemap.SetTile(new Vector3Int(x, y, 0), tileBase);
    }
    else
    {
      tilemap.SetTile(new Vector3Int(x, y, 0), tileBase2);
    }
    */
  }

    internal Character GetCharacter(int x, int y)
    {
        return grid.GetCharacter(x, y);
    }

    public void Set(int x, int y, int to) // bool to)
    {
        grid.SetTile(x,y,to);//Set(x, y, to);
        //UpdateTileMap();
        UpdateTile(x, y);
    }

    public int[,] ReadTileMap()
    {
      
      if (tilemap == null)
      {
        tilemap = GetComponent<Tilemap>();
      }
      
      int size_x = tilemap.size.x;
      int size_y = tilemap.size.y;
      int[,] tilemapdata = new int[size_x, size_y];

      for(int x=0; x<size_x; x++)
      {
        for (int y=0; y<size_y; y++)
        {
          TileBase tileBase = tilemap.GetTile(new Vector3Int(x,y, 0));
          int indexTile = tileSet.tiles.FindIndex(x => x == tileBase);
          tilemapdata[x,y] = indexTile;
        }
      }

      return tilemapdata;
    }
}
