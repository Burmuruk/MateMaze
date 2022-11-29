using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CostGraph
{
    public List<Vector3> nodes = new List<Vector3>();
    public Dictionary<Vector3, int> nodeKind = new Dictionary<Vector3, int>();

    public CostGraph(int sizeX, int sizeY)
    {
        for (int i = 0; i < sizeX; i++)
            for (int j = 0; j < sizeY; j++)
            {
                Vector3 node = new Vector3(i, j, 0);
                nodes.Add(node);

                /*
                if (Random.Range(0, 10) > 4)
                    nodeKind.Add(node, 1);
                else
                    nodeKind.Add(node, 2);
                */

                if (!ForestCond(i, j, sizeX, sizeY))
                    nodeKind.Add(node, 1);
                else
                    nodeKind.Add(node, 2);
            }
    }

    public List<Vector3> Neighbours(Vector3 node)
    {
        List<Vector3> directions = new List<Vector3> {Vector3.right,
                                                      Vector3.up,
                                                      Vector3.left,
                                                      Vector3.down };

        List<Vector3> result = new List<Vector3>();
        foreach (Vector3 direction in directions)
        {
            Vector3 neighbour = node + direction;
            if (nodes.Contains(neighbour))
                result.Add(neighbour);
        }
        return result;
    }

    public int Cost(Vector3 node1, Vector3 node2)
    {
        int result = 0;

        if (nodeKind[node2] == 2)
            result = 10;
        if (nodeKind[node2] == 1)
            result = 1;

        return result;
    }

    private bool ForestCond(int i, int j, int sX, int sY)
    {
        int r2 = (sY / 3) * (sY / 3);
        bool cond1 = i*i +(j-sY+1)*(j-sY+1) < r2;
        bool cond2 = i * i + j*j < r2;
        bool cond3 = (i - sX / 2) *(i-sX/2) + (j - sY/2) * (j - sY / 2) < r2;
        bool cond = cond1 || cond2 || cond3;
        return cond;
    }

}
