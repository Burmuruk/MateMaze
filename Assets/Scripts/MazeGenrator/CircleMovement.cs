using System.Collections;
using UnityEngine;

namespace Mate.Clase.Maze
{
    public class CircleMovement: IEnumerator
    {
        public bool printProgess = false;
        (int x, int y)[] movement;
        (int x, int y) current = (0, 0);
        Vector2Int startNode;
        Vector2Int size;
        (int x, int y, int times)[] directions =
        {
                (0, 1, 1),
                (1, 0, 1),
                (0, -1, 2),
                (-1, 0, 2),
                (1, 0, 2),
                (0, 1, 1),
            };
        int offset = 1;
        (int dir, int times) curDir = default;

        public CircleMovement(Vector2Int size, Vector2Int startNode)
        {
            this.size = size;
            this.startNode = startNode;
        }

        public object Current => throw new System.NotImplementedException();

        public bool MoveNext()
        {
            throw new System.NotImplementedException();
        }

        public Vector2Int? Next()
        {
            Vector2Int? next = null;

            if (curDir.times + 1 < directions[curDir.dir].times * offset)
                curDir.times++;
            else if (curDir.dir + 1 < directions.Length)
                curDir = (curDir.dir++, 0);
            else
            {
                curDir = (0, 0);
                offset++;
                current.y += startNode.y + offset;
            }

            if (current.x + directions[curDir.dir].x is var x && x < size.x &&
                current.y + directions[curDir.dir].y is var y && y < size.y)
            {
                current.x += directions[curDir.dir].x;
                current.y += directions[curDir.dir].y;

                return new Vector2Int(current.x, current.y);
            }


            return next;
        }

        public void Reset()
        {
            current = (startNode.x, startNode.y);
            offset = 1;
            curDir = (0, 0);
        }
    }
}
