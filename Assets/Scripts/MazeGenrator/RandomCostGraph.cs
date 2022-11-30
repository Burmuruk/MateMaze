using System.Collections.Generic;
using UnityEngine;

namespace Mate.Clase.Maze
{
    public class RandomCostGraph
    {
        public List<Vector2> nodes = new List<Vector2>();
        public Dictionary<Vector2Int, int> edgeCost = new Dictionary<Vector2Int, int>();

        public RandomCostGraph(int sizeX, int sizeY)
        {
            // Init Graph Nodes
            for (int i = 0; i < sizeX; i++)
            {
                for (int j = 0; j < sizeY; j++)
                {
                    Vector2 node = new Vector2(i, j);
                    nodes.Add(node);
                }
            }
            // Init Edge Costs/Weights
            for (int i = 0; i < nodes.Count; i++)
            {
                for (int j = 0; j < nodes.Count; j++)
                {
                    Vector2Int pair1 = new Vector2Int(i, j);
                    Vector2Int pair2 = new Vector2Int(j, i);

                    bool cond0 = AreNeighbours(i, j);
                    bool cond1 = edgeCost.ContainsKey(pair1);
                    bool cond2 = edgeCost.ContainsKey(pair2);

                    if ((i != j) && cond0 && !cond1 && !cond2)
                    {
                        var rand = new System.Random();
                        int cost = rand.Next(0, 101);
                        edgeCost.Add(pair1, cost);
                        edgeCost.Add(pair2, cost);
                    }
                }
            }
        }

        public List<Vector2> Neighbours(Vector2 node)
        {
            List<Vector2> directions =
                        new List<Vector2> {Vector2.right, Vector2.up,
                                        Vector2.left, Vector2.down };

            List<Vector2> result = new List<Vector2>();
            foreach (Vector2 direction in directions)
            {
                Vector2 neighbour = node + direction;
                if (nodes.Contains(neighbour))
                    result.Add(neighbour);
            }
            return result;
        }


        public List<Vector2> GraphWithout(Vector2 u)
        {
            List<Vector2> result = new List<Vector2>();
            foreach (Vector2 v in nodes)
            {
                if (v != u)
                    result.Add(v);
            }
            return result;
        }

        public bool AreNeighbours(int i, int j)
        {
            Vector2 node1 = nodes[i];
            Vector2 node2 = nodes[j];

            List<Vector2> neighbours = Neighbours(node1);

            if (neighbours.Contains(node2))
                return true;

            else
                return false;
        }

    }


}