using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetNodes : MonoBehaviour
{
    public static bool setStartNode = false;   
    public static bool setEndNode = false;

    public static Vector3 startPosition;
    public static Vector3 endPosition;

    void Update()
    {
        if (Input.GetMouseButtonDown(0) && !setStartNode)
        {
            setStartNode = true;
            Vector3 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            int x = Mathf.RoundToInt(pos.x);
            int y = Mathf.RoundToInt(pos.y);
            startPosition = new Vector3(x, y, 0);
            GameObject.Find("StartNode").transform.position = startPosition;
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
