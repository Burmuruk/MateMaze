using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Mate.Clase.Following
{
    public class SimpleGraph
    {
        public List<Vector3> nodes = new List<Vector3>();

        public SimpleGraph(bool obstacles, int sizeX, int sizeY)
        {
            for (int i = -sizeX / 2; i < sizeX / 2; i++)
                for (int j = -sizeY / 2; j < sizeY / 2; j++)
                {
                    Vector3 node = new Vector3(i, j, 0);
                    nodes.Add(node);

                    if (obstacles && Random.Range(0, 10) > 7)
                        nodes.Remove(node);
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

    } 
}
