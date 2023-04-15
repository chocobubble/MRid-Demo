using System.Collections.Generic;
using UnityEngine;

namespace MRidDemo
{
    public class PathNode
    {
        public int xPos;
        public int yPos;
        public int gValue;
        public int hValue;
        public bool isObstacle = false;
        public PathNode parentNode;
        public int fValue
        {
            get { return gValue + hValue; }
        }
        public Vector2 nodePos
        {
            get { return new Vector2(xPos, yPos); }
        }

        public PathNode(int xPos, int yPos)
        {
            this.xPos = xPos;
            this.yPos = yPos;
        }
    }

    [RequireComponent(typeof(GridMap))]
    public class Pathfinding : MonoBehaviour
    {
        public int xLength;
        public int yLength;
        PathNode[,] pathNodes;
        private void Start()
        {
            pathNodes = new PathNode[xLength, yLength];

            for (int x = 0; x < xLength; x++)
            {
                for (int y = 0; y < yLength; y++)
                {
                    pathNodes[x, y] = new PathNode(x, y);
                    RaycastHit2D hit2D = Physics2D.Raycast(new Vector2(x, y), Vector2.zero, ((1 << 9)));
                    if (hit2D.collider != null)
                    {
                        pathNodes[x, y].isObstacle = true;
                        Debug.Log($"x : {x}, y : {y}");
                    }
                }
            }
        }

        // It's for finding escape pathway.
        public List<Vector2> FindPath(int startX, int startY, int endX, int endY)
        {
            PathNode startNode = pathNodes[startX, startY];
            PathNode endNode = pathNodes[endX, endY];

            List<PathNode> openList = new List<PathNode>();
            List<PathNode> closedList = new List<PathNode>();

            openList.Add(startNode);

            while (openList.Count > 0)
            {
                PathNode currentNode = openList[0];

                for (int i = 0; i < openList.Count; i++)
                {
                    if (currentNode.fValue > openList[i].fValue)
                    {
                        currentNode = openList[i];
                    }
                    if (currentNode.fValue == openList[i].fValue
                        && currentNode.hValue > openList[i].hValue)
                    {
                        currentNode = openList[i];
                    }
                }

                openList.Remove(currentNode);
                closedList.Add(currentNode);

                if (currentNode == endNode)
                {
                    // we finished searching ours path
                    return RetracePath(startNode, endNode);
                }

                List<PathNode> neighbourNodes = new List<PathNode>();
                for (int x = -1; x < 2; x++)
                {
                    for (int y = -1; y < 2; y++)
                    {
                        if (x == 0 && y == 0) continue;
                        if (currentNode.xPos + x < 0 || currentNode.xPos + x >= xLength || currentNode.yPos + y < 0 || currentNode.yPos + y >= yLength) continue;
                        neighbourNodes.Add(pathNodes[currentNode.xPos + x, currentNode.yPos + y]);
                    }
                }

                for (int i = 0; i < neighbourNodes.Count; i++)
                {
                    if (closedList.Contains(neighbourNodes[i])) continue;
                    if (neighbourNodes[i].isObstacle == true) continue;

                    int movementCost = currentNode.gValue + CalculateDistance(currentNode, neighbourNodes[i]);

                    if (openList.Contains(neighbourNodes[i]) == false ||
                        movementCost < neighbourNodes[i].gValue)
                    {
                        neighbourNodes[i].gValue = movementCost;
                        neighbourNodes[i].hValue = CalculateDistance(neighbourNodes[i], endNode);
                        neighbourNodes[i].parentNode = currentNode;

                        if (openList.Contains(neighbourNodes[i]) == false)
                        {
                            openList.Add(neighbourNodes[i]);
                        }
                    }
                }
            }
            return null;
        }

        // It's for finding place to attack
        public List<Vector2> FindAtkPath(int startX, int startY, int endX, int endY, float distance)
        {
            PathNode startNode = pathNodes[startX, startY];
            PathNode endNode = pathNodes[endX, endY];

            List<PathNode> openList = new List<PathNode>();
            List<PathNode> closedList = new List<PathNode>();

            openList.Add(startNode);

            while (openList.Count > 0)
            {
                PathNode currentNode = openList[0];

                for (int i = 0; i < openList.Count; i++)
                {
                    if (currentNode.fValue > openList[i].fValue)
                    {
                        currentNode = openList[i];
                    }
                    if (currentNode.fValue == openList[i].fValue
                        && currentNode.hValue > openList[i].hValue)
                    {
                        currentNode = openList[i];
                    }
                }

                openList.Remove(currentNode);
                closedList.Add(currentNode);

                // this is diffrence with above FindPath method
                if (currentNode.hValue < distance)
                {
                    // we finished searching ours path

                    RaycastHit2D hit2 = Physics2D.Raycast(currentNode.nodePos, Vector2.zero, 0.01f, ((1 << 6)));
                    if (hit2.collider == null) return RetracePath(startNode, currentNode);
                    else Debug.Log(hit2.collider.name + " hit");
                }

                List<PathNode> neighbourNodes = new List<PathNode>();
                for (int x = -1; x < 2; x++)
                {
                    for (int y = -1; y < 2; y++)
                    {
                        if (x == 0 && y == 0) continue;
                        // if(x < 0 || x >= xLength || y < 0 || y >= yLength) continue;
                        if (currentNode.xPos + x < 0 || currentNode.xPos + x >= xLength || currentNode.yPos + y < 0 || currentNode.yPos + y >= yLength) continue;
                        neighbourNodes.Add(pathNodes[currentNode.xPos + x, currentNode.yPos + y]);
                    }
                }

                for (int i = 0; i < neighbourNodes.Count; i++)
                {
                    if (closedList.Contains(neighbourNodes[i])) continue;
                    if (neighbourNodes[i].isObstacle == true) continue;

                    int movementCost = currentNode.gValue + CalculateDistance(currentNode, neighbourNodes[i]);

                    if (openList.Contains(neighbourNodes[i]) == false ||
                        movementCost < neighbourNodes[i].gValue)
                    {
                        neighbourNodes[i].gValue = movementCost;
                        neighbourNodes[i].hValue = CalculateDistance(neighbourNodes[i], endNode);
                        neighbourNodes[i].parentNode = currentNode;

                        if (openList.Contains(neighbourNodes[i]) == false)
                        {
                            openList.Add(neighbourNodes[i]);
                        }
                    }
                }
            }
            return null;
        }

        private List<Vector2> RetracePath(PathNode startNode, PathNode endNode)
        {
            List<Vector2> path = new List<Vector2>();

            PathNode currentNode = endNode;

            while (currentNode != startNode)
            {
                path.Add(currentNode.nodePos);
                currentNode = currentNode.parentNode;
            }
            path.Reverse();

            return path;
        }

        private int CalculateDistance(PathNode current, PathNode target)
        {
            return (int)Vector2.Distance(current.nodePos, target.nodePos);
        }
    }
}
