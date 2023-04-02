using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveLoadMap : MonoBehaviour
{
    [SerializeField] MapData mapData;

    [SerializeField] GridMap gridmap;
    [SerializeField] GridManager gridManager;
    public void Save()
    {
        int[,] map = gridManager.ReadTileMap();
        mapData.Save(map);
        Debug.Log("saveloadmap's save");
    }

    public void Load()
    {
        gridManager.Clear();

        int width = mapData.width;
        int height = mapData.height;

        int i=0;
        for (int x=0; x<width; x++)
        {
            for (int y=0; y<height; y++)
            {
                gridManager.SetTile(x,y, mapData.map[i]);
                i++;
            }
        }
    }

    internal void Load(GridMap grid)
    {
        mapData.Load(grid);
    }
}
