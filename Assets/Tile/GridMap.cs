using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node
{
    public int tileId;
    public Character character;
}
public class GridMap : MonoBehaviour
{
    [SerializeField] public int height;
    [SerializeField] public int length;

    // bool[,] grid;
    //int[,] grid;
    Node[,] grid;

    public void Init(int length, int height)
    {
        grid = new Node[length, height]; //int[length, height] //bool[length, height];

        for(int x = 0; x < length; x++)
        {
            for (int y= 0; y<height; y++)
            {
                grid[x,y] = new Node();
            }
        }
        this.length = length;
        this.height = height;
    }

    internal void SetCharacter(MapElement mapElement, int x_pos, int y_pos)
    {
        grid[x_pos, y_pos].character = mapElement.GetComponent<Character>();
    }

    public void SetTile(int x, int y, int to)//Set(int x, int y, int to) // bool to)
    {
        if(CheckPosition(x,y)==false)
        {
            Debug.LogWarning("Trying to Set a cell outside the Grid boundries "
                + x.ToString() + ":" + y.ToString());
            return;
        }
        grid[x,y].tileId = to; //grid[x,y] = to;
    }

    public int GetTile(int x, int y)//Get(int x, int y) //bool Get(int x, int y)
    {
        if(CheckPosition(x,y)==false)
        {
            Debug.LogWarning("Trying to Get an cell outside the Grid boundaries" +
                x.ToString() + ":" + y.ToString());
            return -1;
        } // false; }
        return grid[x,y].tileId; //grid[x, y];
    }

    public bool CheckPosition(int x, int y) {
        if(x < 0 || x >= length || y < 0 || y >= height)
        {
            Debug.LogWarning("Trying to Get an cell outside the Grid boundaries" +
                x.ToString() + ":" + y.ToString());
            return false;
        }
        return true;
    }

    public Character GetCharacter(int x, int y)
    {
        return grid[x, y].character;
    }

    internal bool CheckWalkable(int xPos, int yPos)
    {
        return grid[xPos, yPos].tileId == 0;//grid[xPos, yPos] == 0;
    }
}


