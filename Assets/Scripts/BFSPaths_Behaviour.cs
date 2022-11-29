using Mate.Clase.Algorithm;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BFSPaths_Behaviour : MonoBehaviour
{
    List<Vector3> path = new List<Vector3>();
    public GameObject nodeSprite;
    SimpleGraph simpleGraph;

    Queue<Vector3> frontier;
    Dictionary<Vector3, Vector3> cameFrom;
    public bool completed = false;

    private Vector3 startNode, endNode;

    public List<Vector3> Path { get => path; private set => path = value; }

    void Awake()
    {
        simpleGraph = new SimpleGraph(true, 20, 10);
    }

    void Start()
    {
        CreateGraphSprites();
        SetStartNode();
        
        //InvokeRepeating("BFS_Algorithm", 1f, 0.5f);
    }

    private void Update()
    {
        BFS_Algorithm();
    }

    void BFS_Algorithm()
    {
        if (frontier.Count > 0)
        {
            Vector3 current = frontier.Dequeue();

            /*
            if (current == endNode)
            {
                CancelInvoke("BFS_Algorithm");
                ReconstructPath();
            }
            */

            foreach (Vector3 next in simpleGraph.Neighbours(current))
            {
                if (!cameFrom.ContainsKey(next))
                {
                    frontier.Enqueue(next);
                    cameFrom.Add(next, current);
                    LightNode(next);
                }
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
            GameObject gO = Instantiate(nodeSprite, v, Quaternion.identity);
            gO.transform.parent = this.transform;
        }
    }

    void SetStartNode()
    {
        int index1 = Random.Range(0, simpleGraph.nodes.Count);
        startNode = simpleGraph.nodes[index1];
        GameObject.Find("StartNode").transform.position = startNode;
        //
        int index2 = Random.Range(0, simpleGraph.nodes.Count);
        endNode = simpleGraph.nodes[index2];
        GameObject.Find("EndNode").transform.position = endNode;
        //
        frontier = new Queue<Vector3>();
        frontier.Enqueue(startNode);
        cameFrom = new Dictionary<Vector3, Vector3>();
        cameFrom.Add(startNode, startNode);
    }

    void SetEndNode()
    {
        
    }

    void LightNode(Vector3 node)
    {
        foreach (Transform t in this.transform)
        {
            if (t.position == node)
                t.gameObject.GetComponent<SpriteRenderer>().color = Color.white;
        }
    }

    void ReconstructPath()
    {
        
        Vector3 current = endNode;

        while (current != startNode)
        {
            path.Add(current);
            current = cameFrom[current];
        }
        path.Add(startNode);
        path.Reverse();

        var line = GetComponent<LineRenderer>();
        line.positionCount = path.Count;
        line.enabled = true;

        int i = 0;
        foreach(Vector3 v in path)
        {
            GetComponent<LineRenderer>().SetPosition(i, v);
            i++;
        }
    }

}
