using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Mate.Trees.Mine
{
    public class Prim : MonoBehaviour
    {
        [SerializeField] int x;
        [SerializeField] int y;
        [SerializeField] GameObject nodePref;
        [SerializeField] Vector2Int start;
        [SerializeField] Vector2Int end;
        private Node[,] S1;
        private List<Node> S2;
        private List<Vector2Int> G;
        private Queue<(Node, Node)> T;

        public struct Node
        {
            public int weight;
            public List<Vector2Int> neighbours;
            public GameObject item;
            Vector2Int position;

            public int Weight { get => weight; set => weight = value; }
            public Vector2Int Position { get => position; }

            public Node(int x, int y)
            {
                weight = Random.Range(0, 101);
                neighbours = new List<Vector2Int>();
                item = default;
                position = new(x, y);
            }

            //public Node (int x, int y, GameObject item) : Node(int x, int y)
            //{
            //    this.item = item;
            //}
        }

        // Start is called before the first frame update
        void Start()
        {
            Initialize();
        }

        // Update is called once per frame
        void Update()
        {

        }

        private void Initialize()
        {
            S1 = new Node[x, y];

            for (int i = 0; i < y; i++)
            {
                for (int j = 0; j < x; j++)
                {
                    S1[j, y] = new Node(j, i);
                    S1[j, y].item = Instantiate(nodePref);
                }
            }

            //S2 = (node)S1.Clone();
        }
    } 
}
