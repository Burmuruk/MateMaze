using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DataStructures.Queue;
using Mate.Clase.Algorithm;

public class GBFS_Behaviour : MonoBehaviour
{
    public GameObject nodeSprite;
    SimpleGraph simpGraph;
    PriorityQueue<float, Vector3> frontier;
    Dictionary<Vector3, Vector3> cameFrom;

    private Vector3 startNode, endNode;

    void Awake()
    {
        simpGraph = new SimpleGraph(true, 40, 20);
    }

    void Start()
    {
        CreateGraphSprites();
        SetStartNode();
        InvokeRepeating("GBFS_Algorithm", 1f, 0.1f);
    }

    void GBFS_Algorithm()
    {
        if (frontier.Count > 0)
        {
            Vector3 current = frontier.Dequeue();


            if (current == endNode)
            {
                CancelInvoke("GBFS_Algorithm");
                ReconstructPath();
            }


            foreach (Vector3 next in simpGraph.Neighbours(current))
            {

                if (!cameFrom.ContainsKey(next))
                {
                    float priority = Heuristic(endNode, next);
                    frontier.Enqueue(priority, next);
                    cameFrom.Add(next, current);
                    LightNode(next);
                }
            }
        }
    }

    void CreateGraphSprites()
    {
        
        Color green = new Color(0f, 0.5f, 0.12f);
        foreach (Vector3 v in simpGraph.nodes)
        {
            GameObject gO = Instantiate(nodeSprite, v, Quaternion.identity);
            gO.transform.parent = this.transform;
            gO.GetComponent<SpriteRenderer>().color = green;
        }
    }

    void SetStartNode()
    {
        int index1 = Random.Range(0, simpGraph.nodes.Count);
        startNode = simpGraph.nodes[index1];
        GameObject.Find("StartNode").transform.position = startNode;
        //
        int index2 = Random.Range(0, simpGraph.nodes.Count);
        endNode = simpGraph.nodes[index2];
        GameObject.Find("EndNode").transform.position = endNode;
        //
        frontier = new PriorityQueue<float, Vector3>();
        frontier.Enqueue(0, startNode);
        cameFrom = new Dictionary<Vector3, Vector3>();
        cameFrom.Add(startNode, startNode);
        
    }

    void LightNode(Vector3 node)
    {
        Color lightGreen = new Color(0.47f, 1f, 0.60f);
        foreach (Transform t in this.transform)
        {
            if (t.position == node)
            {
                t.gameObject.GetComponent<SpriteRenderer>().color = lightGreen;
            }
        }
    }

    void ReconstructPath()
    {

        Vector3 current = endNode;

        List<Vector3> path = new List<Vector3>();

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
        foreach (Vector3 v in path)
        {
            GetComponent<LineRenderer>().SetPosition(i, v);
            i++;
        }
    }

    float Heuristic(Vector3 node1, Vector3 node2)
    {
        float result = Mathf.Abs(node1.x - node2.x) + Mathf.Abs(node1.y - node2.y);
        return result;
    }
}
