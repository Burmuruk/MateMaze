using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Mate.Clase.Following
{
    public class SetNodes : MonoBehaviour
    {
        public static bool setStartNode = false;
        public static bool setEndNode = false;

        public static Vector3 startPosition;
        public static Vector3 endPosition;

        void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                if (!setStartNode)
                {
                    setStartNode = true;
                    SetPoint(out startPosition, "StartNode");
                }
                else if (setStartNode && BFS_Path.completed)
                {
                    setEndNode = false;
                    SetPoint(out startPosition, "StartNode");
                }
            }

            if (Input.GetMouseButtonDown(1))
            {
                if (!setEndNode && !BFS_Path.completed)
                {
                    setEndNode = true;
                    SetPoint(out endPosition, "EndNode"); 
                }
                else if (setStartNode && BFS_Path.completed)
                {
                    setEndNode = true;
                    BFS_Path.completed = false;
                    SetPoint(out startPosition, "EndNode");
                }
            }
        }

        private void SetPoint(out Vector3 point, string name)
        {
            Vector3 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            int x = Mathf.RoundToInt(pos.x);
            int y = Mathf.RoundToInt(pos.y);
            point = new Vector3(x, y, 0);
            GameObject.Find(name).transform.position = point;
        }
    } 
}
