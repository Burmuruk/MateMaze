using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DataStructures.Queue;

public class AStar_Behaviour : MonoBehaviour
{
    public GameObject nodeSprite;
    CostGraph costGraph;
    PriorityQueue<float, Vector3> frontier;
    Dictionary<Vector3, Vector3> cameFrom;
    Dictionary<Vector3, float> costSoFar;

    private Vector3 startNode, endNode;

    void Awake()
    {
        costGraph = new CostGraph(40, 20);
    }

    void Start()
    {
        CreateGraphSprites();
        SetStartNode();
        InvokeRepeating("AStar_Algorithm", 1f, 0.1f);
    }

    void AStar_Algorithm()
    {
        if (frontier.Count > 0)
        {
            Vector3 current = frontier.Dequeue();


            if (current == endNode)
            {
                CancelInvoke("AStar_Algorithm");
                ReconstructPath();
            }


            foreach (Vector3 next in costGraph.Neighbours(current))
            {
                float newCost = costSoFar[current] + costGraph.Cost(current, next);

                if (!costSoFar.ContainsKey(next) || newCost < costSoFar[next])
                {
                    costSoFar.Add(next, newCost);
                    float priority = newCost+ Heuristic(endNode, next);
                    frontier.Enqueue(priority, next);
                    cameFrom.Add(next, current);
                    LightNode(next);
                }
            }
        }
    }

    void CreateGraphSprites()
    {
        Color ground = new Color(0.5f, 0.3f, 0f);
        Color green = new Color(0f, 0.5f, 0.12f);
        foreach (Vector3 v in costGraph.nodes)
        {
            GameObject gO = Instantiate(nodeSprite, v, Quaternion.identity);
            gO.transform.parent = this.transform;
            if (costGraph.nodeKind[v] == 1)
                gO.GetComponent<SpriteRenderer>().color = ground;
            else
                gO.GetComponent<SpriteRenderer>().color = green;

        }
    }

    void SetStartNode()
    {
        int index1 = Random.Range(0, costGraph.nodes.Count);
        startNode = costGraph.nodes[index1];
        GameObject.Find("StartNode").transform.position = startNode;
        //
        int index2 = Random.Range(0, costGraph.nodes.Count);
        endNode = costGraph.nodes[index2];
        GameObject.Find("EndNode").transform.position = endNode;
        //
        frontier = new PriorityQueue<float, Vector3>();
        frontier.Enqueue(0, startNode);
        cameFrom = new Dictionary<Vector3, Vector3>();
        cameFrom.Add(startNode, startNode);
        costSoFar = new Dictionary<Vector3, float>();
        costSoFar.Add(startNode, 0);
    }

    void LightNode(Vector3 node)
    {
        Color lightGround = new Color(0.92f, 0.72f, 0.43f);
        Color lightGreen = new Color(0.47f, 1f, 0.60f);
        foreach (Transform t in this.transform)
        {
            if (t.position == node)
            {
                if (costGraph.nodeKind[node] == 1)
                    t.gameObject.GetComponent<SpriteRenderer>().color = lightGround;
                else
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
