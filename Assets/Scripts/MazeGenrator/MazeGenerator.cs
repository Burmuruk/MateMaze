using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading;
using System;

namespace Mate.Clase.Maze
{
    public class MazeGenerator : MonoBehaviour
    {
        public GameObject nodeSprite;
        public GameObject wallPref;
        public Vector2Int size;
        public RandomCostGraph graph;

        private List<Vector2> S1 = new List<Vector2>();
        private List<Vector2> S2 = new List<Vector2>();
        private List<Vector2Int> E = new List<Vector2Int>();
        private List<Vector2Int> T = new List<Vector2Int>();

        public bool mazeCompleted = false;
        public List<Vector2> mazeNodes;
        private bool areWallsReady = false;

        private List<Wall> Walls = new List<Wall>();
        private int wallsIndex = 0;
        private bool needMoveWalls = false;
        bool continueAlgo = false;
        bool initialized = false;

        Thread Algorithem;
        Thread Initialize;

        //Process
        bool areNodesReady = false;

        private class Wall
        {
            public GameObject wall;
            public Vector3 newPosition;
            public float distance = 0;
            //public bool Needed

            public Wall(Vector3 newPosition)
            {
                this.newPosition = newPosition;
            }

            public Wall(GameObject wall, Vector3 newPosition)
            {
                this.wall = wall;
                this.newPosition = newPosition;
            }
        }

        #region Unity methods
        void Awake()
        {
            Walls = new List<Wall>();
            Algorithem = new Thread(CalculateAlgo);
            Initialize = new Thread(InitializeGrid);
        }

        void Start()
        {
            Initialize.Start();
            Invoke("CreateGraphSprites", .5f);
        }

        void Update()
        {
            if (!initialized) return;

            MST_Algorithm();

            if (Input.GetKeyDown(KeyCode.H) && mazeCompleted)
            {
                RestartVariables();

                Initialize.Start();
            }

            if (needMoveWalls)
            {
                foreach (var wall in Walls)
                {
                    wall.wall.transform.Translate((wall.newPosition - wall.wall.transform.position) * Time.deltaTime * .8f, Space.World);
                    Debug.DrawRay(wall.wall.transform.position, wall.newPosition - wall.wall.transform.position);
                }
            }
        }
        #endregion

        #region public methods
        public List<Vector2> MazeNeighbours(Vector2 node)
        {
            List<Vector2> directions =
                        new List<Vector2> {Vector2.right, Vector2.up,
                                        Vector2.left, Vector2.down };

            List<Vector2> result = new List<Vector2>();
            foreach (Vector2 direction in directions)
            {
                Vector2 neighbour = node + direction;

                int i = mazeNodes.IndexOf(node);
                int j = mazeNodes.IndexOf(neighbour);

                Vector2Int pair1 = new Vector2Int(i, j);
                Vector2Int pair2 = new Vector2Int(j, i);

                if (mazeNodes.Contains(neighbour) && (T.Contains(pair1) || T.Contains(pair2)))
                    result.Add(neighbour);
            }
            return result;
        }

        public void Restart_Maze()
        {
            if (!mazeCompleted) return;

            RestartVariables();
            Initialize.Start();
        }
        #endregion

        #region Private methods
        void InitializeGrid()
        {
            graph = new RandomCostGraph(size.x, size.y);

            Debug.Log("GridCreated");

            // Initialize S1
            var rand = new System.Random();
            int i = rand.Next(0, graph.nodes.Count);
            Vector2 u = graph.nodes[i];
            S1.Add(u);

            // Initialize S2
            S2 = graph.GraphWithout(u);

            // Initialize E
            foreach (Vector2 v in S2)
            {
                int j = graph.nodes.IndexOf(v);
                Vector2Int pair = new Vector2Int(i, j);
                if (graph.edgeCost.ContainsKey(pair))
                {
                    E.Add(pair);
                }
            }

            Debug.Log("finishGrid");
            initialized = true;
        }

        private void RestartVariables()
        {
            continueAlgo = false;
            mazeCompleted = false;
            initialized = false;
            S1 = new List<Vector2>();
            T = new List<Vector2Int>();
            S2 = new List<Vector2>();
            E = new List<Vector2Int>();

            Initialize = new Thread(InitializeGrid);
            Algorithem = new Thread(CalculateAlgo);
        }

        void MST_Algorithm()
        {
            if (initialized && Algorithem.ThreadState == ThreadState.Unstarted)
            {
                print("Vamos");
                print(Algorithem.ThreadState);
                Algorithem.Start();
                Algorithem.Join();
            }

            if (!mazeCompleted)
            {
                print("Continue");
                Debug.Log("END!");

                mazeCompleted = true;

                CreateMazeBorders();
                CreateMazeWalls();
                mazeNodes = new List<Vector2>();
                mazeNodes = graph.nodes;
                mazeNodes = new List<Vector2>();
            }
        }

        void CalculateAlgo()
        {
            continueAlgo = false;

            while (S2.Count > 0 && !continueAlgo)
            {
                Debug.Log("hi");
                Vector2Int minCostPair = Vector2Int.zero;
                int minCost = 1000000;
                foreach (Vector2Int pair in E)
                {
                    if (graph.edgeCost[pair] < minCost)
                    {
                        minCost = graph.edgeCost[pair];
                        minCostPair = pair;
                    }
                }

                Vector2 u = graph.nodes[minCostPair.x];
                Vector2 v = graph.nodes[minCostPair.y];

                if (!S1.Contains(v))
                {
                    T.Add(minCostPair);
                    S1.Add(v);
                }

                E.Remove(minCostPair);
                S2.Remove(v);


                int j = graph.nodes.IndexOf(v);
                foreach (Vector2 w in S2)
                {
                    int k = graph.nodes.IndexOf(w);
                    Vector2Int pair = new Vector2Int(j, k);
                    if (graph.edgeCost.ContainsKey(pair))
                        E.Add(pair);
                }

                if (S2.Count == 0)
                {
                    Debug.Log("finished");
                    continueAlgo = true;
                }
            }
        }

        void CreateGraphSprites()
        {
            //foreach (Vector2 v in graph.nodes)
            //{
            //    GameObject gO = Instantiate(nodeSprite, v, Quaternion.identity, this.transform.Find("Nodes"));
            //}

            StartCoroutine(Create_Sprites());
        }

        void CreateMazeBorders()
        {
            for (int i = 0; i < size.x; i++)
            {
                Vector2 posBottom = new Vector2(i, -0.5f);
                Vector2 posTop = new Vector2(i, size.y - 0.5f);
                GameObject bottom = Instantiate(wallPref, posBottom, Quaternion.identity, this.transform.Find("Borders"));
                GameObject top = Instantiate(wallPref, posTop, Quaternion.identity, this.transform.Find("Borders"));
                bottom.transform.localScale = new Vector3(1, 0.1f, 1);
                top.transform.localScale = new Vector3(1, 0.1f, 1);
            }
            for (int j = 0; j < size.y; j++)
            {
                Vector2 posLeft = new Vector2(-0.5f, j);
                Vector2 posRight = new Vector2(size.x - 0.5f, j);

                GameObject left = Instantiate(wallPref, posLeft, Quaternion.identity, this.transform.Find("Borders"));
                GameObject right = Instantiate(wallPref, posRight, Quaternion.identity, this.transform.Find("Borders"));

                left.transform.localScale = new Vector3(0.1f, 1, 1);
                right.transform.localScale = new Vector3(0.1f, 1, 1);
            }
        }

        void CreateMazeWalls()
        {
            for (int i = 0; i < graph.nodes.Count; i++)
                for (int j = 0; j < graph.nodes.Count; j++)
                {
                    if (i != j && graph.AreNeighbours(i, j))
                    {
                        Vector2Int pair1 = new Vector2Int(i, j);
                        Vector2Int pair2 = new Vector2Int(j, i);

                        if (!T.Contains(pair1) && !T.Contains(pair2))
                        {
                            Vector2 nodei = graph.nodes[i];
                            Vector2 nodej = graph.nodes[j];
                            Vector2 wallPos = 0.5f * (nodei + nodej);
                            ChangeWallsPosition(wallPos, nodei.x - nodej.x, nodei.y - nodej.y);
                        }
                    }
                }

            wallsIndex = 0;
            areWallsReady = true;
        }

        private void ChangeWallsPosition(Vector2 wallPos, float sizeX, float sizeY)
        {
            if (!areWallsReady)
            {
                GameObject wall = Instantiate(wallPref, wallPos, Quaternion.identity, this.transform.Find("Walls"));
                Vector3 scaleVector = new Vector3(Mathf.Abs(sizeX), Mathf.Abs(sizeY), 0);
                wall.transform.localScale = Vector3.one - 0.9f * scaleVector;

                Walls.Add(new Wall(wall, wallPos));
            }
            else
            {
                Walls[wallsIndex].newPosition = wallPos;
                Walls[wallsIndex].distance = (Walls[wallsIndex].newPosition * Time.deltaTime * .8f).magnitude;
                Vector3 scaleVector = new Vector3(Mathf.Abs(sizeX), Mathf.Abs(sizeY), 0);
                Walls[wallsIndex].wall.transform.localScale = Vector3.one - 0.9f * scaleVector;

                needMoveWalls = true;
                wallsIndex++;
            }
        }

        void Create_Wall()
        {

        } 
        #endregion

        #region Corutines
        private IEnumerator Create_Sprites()
        {
            int index = 0;

            while (true)
            {
                if (graph == null)
                {
                    yield return new WaitForSeconds(.1f);
                    continue;
                }

                if (index < graph.nodes.Count)
                {
                    Instantiate(nodeSprite, graph.nodes[index], Quaternion.identity, this.transform.Find("Nodes"));
                    index++;
                    yield return new WaitForSeconds(.07f);
                }
                else if (!areNodesReady)
                    yield return new WaitForSeconds(1);
                else
                    break;
            }

            yield break;
        }

        private IEnumerator Create_Walls()
        {


            yield break;
        } 
        #endregion
    }
}