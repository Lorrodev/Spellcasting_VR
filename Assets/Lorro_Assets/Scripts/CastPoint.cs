using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CastPoint : MonoBehaviour
{
    public float cacheSeconds = 3f; //chacheSeconds * samplesPerSeconds needs to be an int
    public int samplesPerSecond = 10;

    private int noChangeInPointsOfInterestForNumUpdates = 0;

    private float timeBetweenSamples;

    private float timeSinceLastSample = Mathf.Infinity;

    private int cacheSize;

    private List<Vector3> pointCache = new List<Vector3>();
    private List<Vector3> pointsOfInterest = new List<Vector3>();
    private List<Vector3> lastPointsOfInterest = new List<Vector3>();
    private List<Vector3> consistentPointsOfInterest = new List<Vector3>();

    private bool possibleRuneDetected = false;

    private bool currentPossibleRunePickedUp = false;

    // Start is called before the first frame update
    void Start()
    {
        timeBetweenSamples = 1 / samplesPerSecond;
        cacheSize = (int)(cacheSeconds * samplesPerSecond);
    }

    // Update is called once per frame
    void Update()
    {
        if (timeSinceLastSample > timeBetweenSamples)
        {
            timeSinceLastSample = 0;

            pointCache.Add(transform.position);

            if (pointCache.Count > cacheSize)
            {
                pointCache.RemoveAt(0);
            }
        }
        else
        {
            timeSinceLastSample += Time.deltaTime;
        }

        analyzePointCache();
    }

    void analyzePointCache()
    {
        //Get avg. mag between points
        /*float totalMag = 0f;
        for (int p = 0; p < pointCache.Count - 1; p++)
        {
            totalMag += (pointCache[p + 1] - pointCache[p]).magnitude;
        }
        float avgMag = totalMag / (pointCache.Count - 1);

        //Set minimum for avgMag -> ignore too small movementes
        avgMag = avgMag > 0.01 ? avgMag : 0.01f;*/

        //Setting fixed avgMag for now
        float avgMag = 0.02f;

        //Find points with mag above avg (= points of interest)
        //List<Vector3> pointsOfInterest = new List<Vector3>();
        int pointsSinceLastPOI = int.MaxValue;

        float totalPOIMag = 0f;

        for (int p = 0; p < pointCache.Count - 1; p++)
        {
            float currentMag = (pointCache[p + 1] - pointCache[p]).magnitude;

            if (currentMag > avgMag * 2f)
            {
                if (pointsSinceLastPOI > 10)
                {
                    pointsOfInterest.Clear();
                }

                pointsSinceLastPOI = 0;
                totalPOIMag += currentMag;
                pointsOfInterest.Add(pointCache[p]);
            }
            else
            {
                if (pointsSinceLastPOI < int.MaxValue)
                {
                    pointsSinceLastPOI++;
                }

                //If point is not actually interesting but close to POIs
                if (pointsOfInterest.Count > 5 && pointsSinceLastPOI < 5)
                {
                    pointsOfInterest.Add(pointCache[p]);
                }
            }
        }

        //If there are some spaced out POIs try to identifiy stable points
        //Stable means POIs that are consistent over multiple updates (for now 15)
        if (pointsOfInterest.Count > 15 && totalPOIMag > 1f)
        {
            if (lastPointsOfInterest.Count == pointsOfInterest.Count)
            {
                noChangeInPointsOfInterestForNumUpdates++;

                if (noChangeInPointsOfInterestForNumUpdates > 8 && possibleRuneDetected == false)
                {
                    consistentPointsOfInterest = new List<Vector3>(pointsOfInterest);

                    possibleRuneDetected = true;
                    currentPossibleRunePickedUp = false;
                }
            }
            else
            {
                noChangeInPointsOfInterestForNumUpdates = 0;

                if (possibleRuneDetected)
                {
                    possibleRuneDetected = false;
                }
            }

            lastPointsOfInterest = new List<Vector3>(pointsOfInterest);
        }
    }

    public bool isPossibleRuneDetected()
    {
        return possibleRuneDetected;
    }

    public bool wasCurrentPossibleRunePickedUp()
    {
        return currentPossibleRunePickedUp;
    }

    public List<Vector3> getPossibleRunePoints()
    {
        currentPossibleRunePickedUp = true;
        return new List<Vector3>(consistentPointsOfInterest);
    }

    void OnDrawGizmos()
    {
        //Draw points in cache
        //Vector3 offsetV1 = new Vector3(0, 0, 1.5f);
        for (int p = 0; p < pointCache.Count - 1; p++)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawLine(pointCache[p], pointCache[p + 1]);
            Gizmos.color = Color.yellow;
            Gizmos.DrawSphere(pointCache[p], 0.007f);
        }

        //Draw points of interest
        /*Vector3 offsetV2 = new Vector3(0, 0, 1f);
        for (int p = 0; p < pointsOfInterest.Count - 1; p++)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawLine(pointsOfInterest[p] + offsetV2, pointsOfInterest[p + 1] + offsetV2);
            Gizmos.color = Color.red;
            Gizmos.DrawSphere(pointsOfInterest[p] + offsetV2, 0.007f);
        }

        //Draw last points of interest
        Vector3 offsetV3 = new Vector3(0, 0, 0.5f);
        for (int p = 0; p < lastPointsOfInterest.Count - 1; p++)
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawLine(lastPointsOfInterest[p] + offsetV3, lastPointsOfInterest[p + 1] + offsetV3);
            Gizmos.color = Color.blue;
            Gizmos.DrawSphere(lastPointsOfInterest[p] + offsetV3, 0.007f);
        }*/

        //Draw consistent points of interest
        for (int p = 0; p < consistentPointsOfInterest.Count - 1; p++)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawLine(consistentPointsOfInterest[p], consistentPointsOfInterest[p + 1]);
            Gizmos.color = Color.green;
            Gizmos.DrawSphere(consistentPointsOfInterest[p], 0.007f);
        }

        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(transform.position, 0.01f);

        Gizmos.color = Color.green;
        Gizmos.DrawLine(transform.position, transform.position + transform.up);
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, transform.position + transform.right);
        Gizmos.color = Color.blue;
        Gizmos.DrawLine(transform.position, transform.position + transform.forward);
    }
}