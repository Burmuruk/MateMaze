using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Mate.Clase.Algorithm
{
    public class BFS_Behaviour : MonoBehaviour
    {
        public GameObject nodeSprite;
        SimpleGraph simpleGraph;

        Queue<Vector3> frontier;
        List<Vector3> reached;

        private void Awake()
        {
            simpleGraph = new SimpleGraph(true, 20, 10);
        }

        void Start()
        {
            CreateGraphSprites();
            SetStartNode();
            SetEndNode();
            InvokeRepeating("BFS_Algorithm", 1f, 0.1f);
        }

        void BFS_Algorithm()
        {
            if (frontier.Count > 0)
            {
                Vector3 current = frontier.Dequeue();
                foreach (Vector3 next in simpleGraph.Neighbours(current))
                {
                    if (!reached.Contains(next))
                    {
                        frontier.Enqueue(next);
                        reached.Add(next);
                        LightNode(next);
                    }
                }
            }
        }

        void CreateGraphSprites()
        {
            foreach (Vector3 v in simpleGraph.nodes)
            {
                GameObject gO = Instantiate(nodeSprite, v, Quaternion.identity);
                gO.transform.parent = this.transform;
            }
        }

        void SetStartNode()
        {
            int index = Random.Range(0, simpleGraph.nodes.Count);
            Vector3 startPosition = simpleGraph.nodes[index];
            GameObject.Find("StartNode").transform.position = startPosition;
            //
            frontier = new Queue<Vector3>();
            frontier.Enqueue(startPosition);
            reached = new List<Vector3>();
            reached.Add(startPosition);
        }

        void SetEndNode()
        {
            int index = Random.Range(0, simpleGraph.nodes.Count);
            Vector3 endPosition = simpleGraph.nodes[index];
            GameObject.Find("EndNode").transform.position = endPosition;
        }

        void LightNode(Vector3 node)
        {
            foreach (Transform t in this.transform)
            {
                if (t.position == node)
                    t.gameObject.GetComponent<SpriteRenderer>().color = Color.white;
            }
        }

    } 
}
