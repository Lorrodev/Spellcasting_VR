using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Valve.VR.InteractionSystem
{
    public class CastPointTest : MonoBehaviour
    {
        public float cacheSeconds = 3f; //chacheSeconds * samplesPerSeconds needs to be an int
        public int samplesPerSecond = 10;

        public List<Vector3> pointCache = new List<Vector3>();

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        public void testFunc()
        {

        }

        void OnDrawGizmos()
        {
            for (int p = 0; p < pointCache.Count-1; p++)
            {
                Gizmos.color = Color.yellow;
                Gizmos.DrawLine(pointCache[p], pointCache[p+1]);
                Gizmos.color = Color.red;
                Gizmos.DrawSphere(pointCache[p], 0.007f);
            }
            Gizmos.color = Color.yellow;
            Gizmos.DrawSphere(transform.position, 0.01f);
        }
    }
}
