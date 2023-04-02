using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapElement : MonoBehaviour
{
    GridMap gridMap;
    void Start()
    {
        SetGrid();
        PlaceObjectOnGrid();
    }

    private void SetGrid()
    {
        gridMap = transform.parent.GetComponent<GridMap>();
    }

    private void PlaceObjectOnGrid()
    {
        Transform t = transform;
        Vector3 pos = t.position;
        int x_pos = (int) pos.x;
        int y_pos = (int) pos.y;
        gridMap.SetCharacter(this, x_pos, y_pos);
        Debug.Log("x = "+ x_pos.ToString() + "y = " + y_pos.ToString());
    }
}
