using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomCostGraph
{
    public List<Vector2> nodes = new List<Vector2>();
    public Dictionary<Vector2Int, int> edgeCost = new Dictionary<Vector2Int, int>();
    
    public RandomCostGraph(int sizeX, int sizeY)
    {
        for (int i = 0; i < sizeX; i++)
        {
            for (int j = 0; j < sizeY; j++)
            {
                Vector2 node = new Vector2(i, j);
                nodes.Add(node);
            }
        }

    }

    public List<Vector2> Neighbours(Vector2 node)
    {
        List<Vector2> directions = new List<Vector2> {Vector2.right,
                                                      Vector2.up,
                                                      Vector2.left,
                                                      Vector2.down };

        List<Vector2> result = new List<Vector2>();
        foreach (Vector2 direction in directions)
        {
            Vector2 neighbour = node + direction;
            if (nodes.Contains(neighbour))
            {
                result.Add(neighbour);
                int i = nodes.IndexOf(node);
                int j = nodes.IndexOf(neighbour);
                int cost = Random.Range(0, 100);
                edgeCost.Add(new Vector2Int(i, j), cost);
            }
        }
        return result;
    }

    public List<Vector2> NodesWithout(int i)
    {
        List<Vector2> result = nodes;
        result.RemoveAt(i);
        return result;
    }


}
