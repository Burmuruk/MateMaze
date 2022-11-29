using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Mate.Clase.Following
{
    public class BFS_Path : MonoBehaviour
    {
        public GameObject bfsNode;
        private SimpleGraph simpleGraph;

        private Queue<Vector3> frontier;
        private Dictionary<Vector3, Vector3> cameFrom;
        public static bool completed = false;
        private bool initBFS = false;

        [System.NonSerialized]
        public static List<Vector3> path;

        void Awake()
        {
            simpleGraph = new SimpleGraph (true, 40, 20);
        }

        void Start()
        {
            CreateGraphSprites();
        }

        void Update()
        {
            if (SetNodes.setStartNode && SetNodes.setEndNode && !initBFS && !completed)
            {
                initBFS = true;
                frontier = new Queue<Vector3>();
                frontier.Enqueue(SetNodes.startPosition);
                cameFrom = new Dictionary<Vector3, Vector3>();
                cameFrom.Add(SetNodes.startPosition, SetNodes.startPosition);
            }

            if (initBFS && !completed)
            {
                BFS_Algorithm();
            }
        }

        void BFS_Algorithm()
        {
            if (frontier.Count > 0)
            {
                Vector3 current = frontier.Dequeue();

                foreach (Vector3 next in simpleGraph.Neighbours(current))
                {
                    if (!cameFrom.ContainsKey(next))
                    {
                        frontier.Enqueue(next);
                        cameFrom.Add(next, current);
                        LightNode(next);
                    }
                }

                if (current == SetNodes.endPosition)
                {
                    completed = true;
                    ReconstructPath();
                }
            }

            if (!completed && frontier.Count == 0)
            {
                completed = true;
                ReconstructPath();
            }
        }

        void CreateGraphSprites()
        {
            foreach (Vector3 v in simpleGraph.nodes)
            {
                GameObject gO = Instantiate(bfsNode, v, Quaternion.identity);
                gO.transform.parent = this.transform;
            }
        }


        void LightNode(Vector3 node)
        {
            foreach (Transform t in this.transform)
            {
                if (t.position == node)
                    t.gameObject.GetComponent<SpriteRenderer>().color = Color.white;
            }
        }

        void DarkNode(Vector3 node)
        {
            foreach (Transform t in this.transform)
            {
                if (t.position == node)
                    t.gameObject.GetComponent<SpriteRenderer>().color = Color.white;
            }
        }

        void ReconstructPath()
        {
            Vector3 current = SetNodes.endPosition;

            path = new List<Vector3>();

            while (current != SetNodes.startPosition)
            {
                path.Add(current);
                current = cameFrom[current];
            }
            path.Add(SetNodes.startPosition);
            path.Reverse();

            var line = GetComponent<LineRenderer>();
            line.enabled = true;
            line.positionCount = path.Count;
            line.widthMultiplier = 0.5f;
            int i = 0;
            foreach (Vector3 v in path)
            {
                GetComponent<LineRenderer>().SetPosition(i, v);
                i++;
            }

            initBFS = false;
        }


        public static void Restart()
        {
            //initBFS = false;
            //completed = false;
            path.Clear();
        }
    } 
}
