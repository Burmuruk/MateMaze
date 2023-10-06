using System.Collections.Generic;
using UnityEngine;
using System.Threading;
using System;

namespace Mate.Clase.Maze


{
    public enum Direction
    {
        None,
        Right,
        Up,
        Left,
        Down,
    }

    public class Node
    {
        public Dictionary<Direction, int> Weights;
    }

    public class RandomCostGraph
    {
        public List<Vector2> nodes = new List<Vector2>();
        public Dictionary<Vector2Int, int> edgeCost = new Dictionary<Vector2Int, int>();
        public bool GraphCompleted = false;
        Vector2Int size = default;
        public Node[,] nodes2 = default;
        Vector2Int startNode = default;

        Dictionary<Direction, Vector2Int> directions = new Dictionary<Direction, Vector2Int>()
        {
            {Direction.Right, new Vector2Int(1 , 0)},
            {Direction.Up, new Vector2Int(0 , 1)},
            {Direction.Left, new Vector2Int(-1 , 0)},
            {Direction.Down, new Vector2Int(0 , -1)},
        };

        Thread random;

        public RandomCostGraph(int sizeX, int sizeY, Vector2Int startNode)
        {
            size = new Vector2Int(sizeX, sizeY);
            random = new Thread(Set_Sizes);
            this.startNode = startNode;
        }

        public void Initialize() => Initialize_Grid(size.x, size.y);

        private void Initialize_Grid(int sizeX, int sizeY)
        {
            random.Start();
            nodes2 = new Node[sizeX, sizeY];

            //for (int i = 0; i < sizeX; i++)
            //{
            //    for (int j = 0; j < sizeY; j++)
            //    {
            //        nodes2[i, j] = new Node();
            //    }
            //}

            CircleMovement circleMovment = new CircleMovement(size, startNode);
            circleMovment.printProgess = true;
            try
            {
                float progress = 1;
                nodes2[startNode.x, startNode.y] = new Node();

                while (true)
                {
                    Vector2Int? nextNode = circleMovment.Next();
                    //Debug.Log("Progress: " + ((size.x * size.y) / circleMovment.findedNodes).ToString());

                    if (!nextNode.HasValue)
                        break;
                    //Debug.Log("Progress: " + ((size.x * size.y) / circleMovment.findedNodes).ToString());
                    if (nodes2[nextNode.Value.x, nextNode.Value.y] == null)
                    {
                        nodes2[nextNode.Value.x, nextNode.Value.y] = new Node();
                        var ho = ((progress / (size.x * size.y)) * 100f);
                        //Debug.Log("Progress: " + ho.ToString());
                        progress++;
                    }


                    //MST_Node node = new MST_Node(curPos.x + indexX, curPos.y + indexY);

                    //var wallIdx = nodes2[startNode.x, startNode.y].Walls[move];
                }
            }
            catch (StackOverflowException)
            {
                Debug.LogError("Error en ciclo While: null value");
            }

            GraphCompleted = true;
        }

        private void Set_Sizes()
        {
            Debug.Log("Set size");
            CircleMovement circleMovment = new CircleMovement(size, startNode);
            circleMovment.printProgess = true;

            while (nodes2 == null) { }

            try
            {
                while (nodes2[startNode.x, startNode.y] == null) { }

                Set_SortedWeights(new Vector2Int(startNode.x, startNode.y));

                while (true)
                {
                    Vector2Int? node = circleMovment.Next();

                    if (!node.HasValue) break;

                    if (nodes2[node.Value.x, node.Value.y] == null)
                        while (true) { }

                    if (nodes2[node.Value.x, node.Value.y].Weights == null)
                        Set_SortedWeights(new Vector2Int(node.Value.x, node.Value.y));
                }
            }
            catch (StackOverflowException)
            {
                Debug.LogError("Error Second thread in While: null value.");
            }

            //for (int i = 0; i < size.x; i++)
            //{
            //    for (int j = 0; j < size.y; j++)
            //    {
            //        if (nodes2[i, j] == null)
            //            while (true)
            //            {

            //            }
            //        List<(Direction, int)> neighbours = Get_Weights(rand);

            //        Sort_Weights(neighbours);

            //        nodes2[i, j].Weights = new Dictionary<Direction, int>();
            //        foreach (var neighbour in neighbours)
            //        {
            //            nodes2[i, j].Weights.Add(neighbour.Item1, neighbour.Item2);
            //        }
            //    }
            //}

            Debug.Log("Weights settled");

            void Sort_Weights(List<(Direction, int)> neighbours)
            {
                bool hadChanges = true;

                while (hadChanges)
                {
                    hadChanges = false;

                    for (int k = 0; k < neighbours.Count; k++)
                    {
                        if (k + 1 < neighbours.Count)
                        {
                            if (neighbours[k].Item2 > neighbours[k + 1].Item2)
                            {
                                var change = neighbours[k + 1];
                                neighbours[k + 1] = neighbours[k];
                                neighbours[k] = change;

                                hadChanges = true;
                            }
                        }

                    }
                }
            }

            List<(Direction, int)> Get_Weights(System.Random rand)
            {
                return new List<(Direction, int)>()
                    {
                        (Direction.Right, rand.Next(0, size.x)),
                        (Direction.Up, rand.Next(0, size.y)),
                        (Direction.Left, rand.Next(0, size.x)),
                        (Direction.Down, rand.Next(0, size.y)),
                    };
            }

            void Set_SortedWeights(Vector2Int position)
            {
                var node = nodes2[position.x, position.y];

                var rand = new System.Random();
                List<(Direction dir, int weight)> neighbours = Get_Weights(rand);

                Sort_Weights(neighbours);

                var weights = new Dictionary<Direction, int>();
                var availableDir = new List<Direction>();

                if (position.x > 0)
                    availableDir.Add(Direction.Left);
                if (position.x < size.x - 1)
                    availableDir.Add(Direction.Right);
                if (position.y > 0)
                    availableDir.Add(Direction.Down);
                if (position.y < size.y - 1)
                    availableDir.Add(Direction.Up);

                foreach (var neighbour in neighbours)
                {
                    if (availableDir.Contains(neighbour.dir))
                        weights.Add(neighbour.Item1, neighbour.weight);
                }

                node.Weights = weights;
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

        public Vector2Int Get_NextRandomNode()
        {
            CircleMovement circleMovment = new CircleMovement(size, startNode);

            return circleMovment.Next().Value;
        }
    }
}