using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Helpers
{
    public class Helpers : MonoBehaviour
    {
        /*
         * Returns the corners of the smallest box containing all the points in the list
         * From bottom to top on y axis
        */
        public static Dictionary<string, Vector3> GetBoundingBox(List<Vector3> points)
        {
            Dictionary<string, Vector3> corners = new Dictionary<string, Vector3>();

            float smallestX = Mathf.Infinity;
            float smallestY = Mathf.Infinity;
            float smallestZ = Mathf.Infinity;
            float biggestX = -Mathf.Infinity;
            float biggestY = -Mathf.Infinity;
            float biggestZ = -Mathf.Infinity;

            for (int p = 0; p < points.Count; p++)
            {
                smallestX = points[p].x < smallestX ? points[p].x : smallestX;
                smallestY = points[p].y < smallestY ? points[p].y : smallestY;
                smallestZ = points[p].z < smallestZ ? points[p].z : smallestZ;
                biggestX = points[p].x > biggestX ? points[p].x : biggestX;
                biggestY = points[p].y > biggestY ? points[p].y : biggestY;
                biggestZ = points[p].z > biggestZ ? points[p].z : biggestZ;
            }

            corners.Add("bottomCornerA", new Vector3(smallestX, smallestY, smallestZ));
            corners.Add("bottomCornerB", new Vector3(biggestX, smallestY, smallestZ));
            corners.Add("bottomCornerC", new Vector3(biggestX, smallestY, biggestZ));
            corners.Add("bottomCornerD", new Vector3(smallestX, smallestY, biggestZ));
            corners.Add("topCornerA", new Vector3(smallestX, biggestY, smallestZ));
            corners.Add("topCornerB", new Vector3(biggestX, biggestY, smallestZ));
            corners.Add("topCornerC", new Vector3(biggestX, biggestY, biggestZ));
            corners.Add("topCornerD", new Vector3(smallestX, biggestY, biggestZ));

            corners.Add("center", corners["bottomCornerA"] + (corners["topCornerC"] - corners["bottomCornerA"]) / 2);

            return corners;
        }
    }
}