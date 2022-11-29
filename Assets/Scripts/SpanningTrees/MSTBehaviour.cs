using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MSTBehaviour : MonoBehaviour
{
    public GameObject nodeSprite;
    public GameObject mazeSprite;
    RandomCostGraph rcGraph;

    private int minW;
    private int minI;
    private int minJ;

    private List<Vector2> S1 = new List<Vector2>();
    private List<Vector2> S2 = new List<Vector2>();
    private Dictionary<Vector2, Vector2> T = new Dictionary<Vector2, Vector2>();
    private Dictionary<int, int> E = new Dictionary<int, int>();
    

    void Awake()
    {
        rcGraph = new RandomCostGraph(10, 10);   
    }

    void Start()
    {
        CreateGraphSprites();
        // Initialize S1
        int i = Random.Range(0, rcGraph.nodes.Count);
        Vector2 u = rcGraph.nodes[i];
        S1.Add(u);

        // Initialize S2
        S2 = rcGraph.NodesWithout(i);

        // Initialize E
        foreach (Vector2 v in S2)
        {
            int j = rcGraph.nodes.IndexOf(v);
            if (rcGraph.edgeCost.ContainsKey(new Vector2Int(i, j)))
            {
                E.Add(i, j);
            }
        }

        InvokeRepeating("MST_Algorithm", 1f, 0.1f);

    }

    void MST_Algorithm()
    {
        if (S2.Count > 0)
        {
            FindMinPair();
            T.Add(rcGraph.nodes[minI], rcGraph.nodes[minJ]);
            E.Remove(minI);



            S1.Add(rcGraph.nodes[minJ]);
            S2.Remove(rcGraph.nodes[minJ]);

            foreach (Vector2 w in S2)
            {
                int k = rcGraph.nodes.IndexOf(w);
                
                if (rcGraph.edgeCost.ContainsKey(new Vector2Int(minJ, k)))
                {
                    E.Add(minJ, k);
                }
            }

            if (S2.Count == 0)
            {
                Debug.Log("END!");
                CreateMazeSprites();
            }
        }
    }

    void CreateGraphSprites()
    {
        foreach (Vector2 v in rcGraph.nodes)
        {
            GameObject gO = Instantiate(nodeSprite, v, Quaternion.identity);
            gO.transform.parent = this.transform;
        }
    }

    void CreateMazeSprites()
    {
        foreach (KeyValuePair<Vector2, Vector2> entry in T)
        {
            GameObject gO1 = Instantiate(mazeSprite, entry.Key, Quaternion.identity);
            GameObject gO2 = Instantiate(mazeSprite, entry.Value, Quaternion.identity);
        }
    }

    private void FindMinPair()
    {
        minW = 1000;
        for (int i = 0; i < S1.Count; i++)
        {
            for (int j = 0; j < S2.Count; j++)
            {
                
                Vector2Int pair = new Vector2Int(rcGraph.nodes.IndexOf(S1[i]), rcGraph.nodes.IndexOf(S2[j]));
                if (rcGraph.edgeCost.ContainsKey(pair) && rcGraph.edgeCost[pair] < minW)
                {
                    minW = rcGraph.edgeCost[pair];
                    minI = rcGraph.nodes.IndexOf(S1[i]);
                    minJ = rcGraph.nodes.IndexOf(S2[j]);
                }
            }
        }
    }

}
