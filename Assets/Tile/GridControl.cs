using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.Tilemaps;

public class GridControl : MonoBehaviour
{
    [SerializeField] Tilemap targetTilemap;
    [SerializeField] GridManager gridManager;
    Pathfinding pathfinding;

    int currentX = 0;
    int currentY = 0;
    int targetPosX = 0;
    int targetPosY = 0;

    [SerializeField] TileBase highlightTile;

    void Start()
    {
      pathfinding = gridManager.GetComponent<Pathfinding>();
    }
    void Update()
  {
    MouseInput();
  }

  private void MouseInput()
  {
      Vector3 worldPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
      Vector3Int clickPosition = targetTilemap.WorldToCell(worldPoint);
    if (Input.GetMouseButtonDown(0))
    {
      targetTilemap.ClearAllTiles();
      //Vector3 worldPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
      //Vector3Int clickPosition = targetTilemap.WorldToCell(worldPoint);
      // Debug.Log(clickPosition);
      //gridManager.Set(clickPosition.x, clickPosition.y, 1); // true);
      targetPosX = clickPosition.x;
      targetPosY = clickPosition.y;
      List<PathNode> path = pathfinding.FindPath(currentX, currentY, targetPosX, targetPosY);

      if(path != null)
      {
        for(int i=0; i<path.Count; i++)
        {
          targetTilemap.SetTile(new Vector3Int(path[i].xPos, path[i].yPos, 0),
              highlightTile);
        }
        currentX = targetPosX;
        currentY = targetPosY;
      }
    }

    if(Input.GetMouseButtonDown(1))
    {
      Character c = gridManager.GetCharacter(clickPosition.x, clickPosition.y);
      if (c!=null)
      {
        Debug.Log("chracter in the cell " +c.Name);
      }
      else
      {
        Debug.Log("2");
      }
    }
  }
}
