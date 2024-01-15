using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Astar 
{
    public List<Vector2Int> FindPathToTarget(Vector2Int startPos, Vector2Int endPos, Cell[,] grid)
    {
        List<Vector2Int> path = new List<Vector2Int>();

        List<Node> openList = new List<Node>();
        List<Node> closedList = new List<Node>();

        Node startNode = new Node(startPos, null, 0, 0);
        Node endNode = null;

        //initialize the end and start node
        openList.Add(startNode);

        while (openList.Count != 0)
        {

            // Check if the endnode had been found
            if (endNode != null)
            {
                break;
            }

            Node lowestFNode = null;

            //Get the node with the lowest F value
            foreach (Node node in openList)
            {
                if (lowestFNode == null) { lowestFNode = node; }
                else if (lowestFNode.FScore > node.FScore)
                {
                    lowestFNode = node;
                }
            }

            List<Cell> neighbourCells = grid[lowestFNode.position.x, lowestFNode.position.y].GetNeighbours(grid);
            List<Node> neighbourNodes = new List<Node>();
            List<Node> newNeighbourNodes = new List<Node>();

            // Make nodes from the given cells
            foreach (Cell cell in neighbourCells)
            {
                if (cell.gridPosition.y > lowestFNode.position.y && cell.HasWall(Wall.DOWN)) { continue; }
                if (cell.gridPosition.y < lowestFNode.position.y && cell.HasWall(Wall.UP)) { continue; }
                if (cell.gridPosition.x > lowestFNode.position.x && cell.HasWall(Wall.LEFT)) { continue; }
                if (cell.gridPosition.x < lowestFNode.position.x && cell.HasWall(Wall.RIGHT)) { continue; }

                Node neighbourNode = new Node(
                    cell.gridPosition,
                    lowestFNode, // Set parent to be the current node
                    lowestFNode.GScore + 1,
                    Vector2Int.Distance(cell.gridPosition, endPos)
                );

                neighbourNodes.Add(neighbourNode);
            }

            // Filter nodes that already exist in open list with lower F value
            foreach (Node node in neighbourNodes)
            {
                if (closedList.Contains(node)) { continue; }

                Node existingNode = openList.Find(n => n.position == node.position);
                if (existingNode != null)
                {
                    if (node.GScore >= existingNode.GScore)
                    {
                        continue; // If the new path is not better, skip
                    }
                    else
                    {
                        // If the new path is better, update the existing node
                        existingNode.parent = node.parent;
                        existingNode.GScore = node.GScore;
                    }
                }
                else
                {
                    openList.Add(node);
                    newNeighbourNodes.Add(node);
                }
            }

            // Check each neighbouring Node
            foreach (Node neighbour in newNeighbourNodes)
            {
                neighbour.parent = lowestFNode;

                if (neighbour.position == endPos)
                {
                    endNode = neighbour;
                    break;
                }
            }

            openList.Remove(lowestFNode);
            closedList.Add(lowestFNode);
        }

        // Collect the path into a list
        Node currentNode = endNode;

        while (currentNode != null)
        {
            path.Insert(0, currentNode.position);
            currentNode = currentNode.parent;
        }

        return path;
    }

    public class Node
    {
        public Vector2Int position; //Position on the grid
        public Node parent; //Parent Node of this node

        public float FScore
        { //GScore + HScore
            get { return GScore + HScore; }
        }
        public float GScore; //Current Travelled Distance
        public float HScore; //Distance estimated based on Heuristic

        public Node() { }
        public Node(Vector2Int position, Node parent, float GScore, float HScore)
        {
            this.position = position;
            this.parent = parent;
            this.GScore = GScore;
            this.HScore = HScore;
        }
    }
}
