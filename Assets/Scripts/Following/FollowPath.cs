using Mate.Clase.Algorithm;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Mate.Clase.Following
{
    public class FollowPath : MonoBehaviour
    {
        private bool arrived = false;
        private int positionIndex = 0;
        private float speed = 1.5f;
        private float radius = 0.05f;
        public Transform obj;
        Rigidbody rb;

        private void Awake()
        {
            rb = GetComponent<Rigidbody>();
        }

        void FixedUpdate()
        {
            if (!arrived && BFS_Path.completed)
            {
                float dt = Time.fixedDeltaTime;
                Vector3 direction = BFS_Path.path[positionIndex + 1] - BFS_Path.path[positionIndex];
                obj.Translate(speed * direction * dt);

                if (Vector3.Distance(obj.position, BFS_Path.path[positionIndex + 1]) < radius)
                {
                    positionIndex++;
                }

                if (positionIndex == BFS_Path.path.Count - 1)
                    arrived = true;
            }
            else if (arrived)
                BFS_Path.Restart();
        }
    } 


}
