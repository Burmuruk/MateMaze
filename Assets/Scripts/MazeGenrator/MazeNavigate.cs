using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Mate.Clase.Maze
{
    public class MazeNavigate : MonoBehaviour
    {

        public MazeGenerator mazeGen;

        // Variables y estructuras para establecer los nodos inicial y final
        private bool setStartNode = false;
        private bool setEndNode = false;

        private Vector3 startPosition;
        private Vector3 endPosition;

        // Variables y estructuras para el algoritmo BFS
        private Queue<Vector3> frontier;
        private Dictionary<Vector3, Vector3> cameFrom;
        public bool completed = false;
        private bool initBFS = false;

        // Variables y estructuras para el movimiento del agente
        private List<Vector3> path;

        private bool arrived = false;
        private int positionIndex = 0;
        private float speed = 1.5f;
        private float radius = 0.05f;
        public Transform obj;

        void Update()
        {
            SetStartEndNodes();

            if (mazeGen.mazeCompleted && setStartNode && setEndNode && !initBFS)
            {
                initBFS = true;
                InitBFSAlgorithm();
            }

            if (initBFS && !completed)
            {
                BFSAlgorithm();
            }
        }

        void FixedUpdate()
        {
            if (!arrived && completed)
            {
                ObjectMovement();
            }
        }

        // Funciones para el movimiento del agente
        private void ObjectMovement()
        {
            float dt = Time.fixedDeltaTime;
            Vector3 direction = path[positionIndex + 1] - path[positionIndex];
            obj.Translate(speed * direction * dt);
            if (Vector3.Distance(obj.position, path[positionIndex + 1]) < radius)
            {
                positionIndex++;
            }

            if (positionIndex == path.Count - 1)
                arrived = true;
        }
        // Funciones para el algoritmo de BFS
        private void InitBFSAlgorithm()
        {
            frontier = new Queue<Vector3>();
            frontier.Enqueue(startPosition);
            cameFrom = new Dictionary<Vector3, Vector3>();
            cameFrom.Add(startPosition, startPosition);
        }

        void BFSAlgorithm()
        {
            if (frontier.Count > 0)
            {
                Vector3 current = frontier.Dequeue();

                foreach (Vector3 next in mazeGen.MazeNeighbours(current))
                {
                    if (!cameFrom.ContainsKey(next))
                    {
                        frontier.Enqueue(next);
                        cameFrom.Add(next, current);
                    }
                }

                if (current == endPosition)
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

        private void ReconstructPath()
        {
            Vector3 current = endPosition;
            path = new List<Vector3>();

            while (current != startPosition)
            {
                path.Add(current);
                current = cameFrom[current];
            }
            path.Add(startPosition);
            path.Reverse();
        }

        // Funciones para establecer el nodo inicial y final
        private void SetStartEndNodes()
        {
            if (Input.GetMouseButtonDown(0) && !setStartNode)
            {
                setStartNode = true;
                Vector3 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                int x = Mathf.RoundToInt(pos.x);
                int y = Mathf.RoundToInt(pos.y);
                startPosition = new Vector3(x, y, 0);
                GameObject.Find("StartNode").transform.position = startPosition;
                obj.position = startPosition;
            }

            if (Input.GetMouseButtonDown(1) && !setEndNode)
            {
                setEndNode = true;
                Vector3 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                int x = Mathf.RoundToInt(pos.x);
                int y = Mathf.RoundToInt(pos.y);
                endPosition = new Vector3(x, y, 0);
                GameObject.Find("EndNode").transform.position = endPosition;
            }
        }

    } 
}


