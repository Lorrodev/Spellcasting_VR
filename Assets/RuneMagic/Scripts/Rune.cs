using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Helpers;

public class Rune : MonoBehaviour
{
    [SerializeField]
    private List<Vector3> runePoints = new List<Vector3>();
    [SerializeField]
    private float deltaThreshold;
    [SerializeField]
    private bool useCameraForwardToAlign = false;

    private Dictionary<string, Vector3> corners;

    /*
    [SerializeField]
    private Vector3 forward;
    [SerializeField]
    private Vector3 up;
    private float size = 0;
    private Vector3 center;*/

    // Start is called before the first frame update
    void Start()
    {
        UpdatePoints();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Center()
    {
        UpdatePoints();

        Vector3 offset = getWorldBoundingBox()["center"] - transform.position;

        for (int p = 0; p < runePoints.Count; p++)
        {
            runePoints[p] -= offset;
        }
    }

    public void Scale(float factor)
    {
        for (int p = 0; p < runePoints.Count; p++)
        {
            runePoints[p] *= factor;
        }

        UpdatePoints();
    }
    public void Translate(Vector3 translation)
    {
        for (int p = 0; p < runePoints.Count; p++)
        {
            runePoints[p] += translation;
        }

        UpdatePoints();
    }

    public void Rotate(Vector3 rotation)
    {
        for (int p = 0; p < runePoints.Count; p++)
        {
            runePoints[p] = Quaternion.Euler(rotation) * runePoints[p];
        }

        UpdatePoints();
    }

    public void Rotate(Quaternion rotation)
    {
        for (int p = 0; p < runePoints.Count; p++)
        {
            runePoints[p] = rotation * runePoints[p];
        }

        UpdatePoints();
    }

    public void Split()
    {
        for (int p = 0; p < runePoints.Count - 1; p++)
        {
            Vector3 newPoint = runePoints[p] + (runePoints[p + 1] - runePoints[p]) / 2;
            runePoints.Insert(p+1, newPoint);
            p++;
        }

        UpdatePoints();
    }

    public void Combine()
    {
        for (int p = 1; p < runePoints.Count; p++)
        {
            runePoints.RemoveAt(p);
        }

        UpdatePoints();
    }

    public void UpdatePoints()
    {
        corners = Helpers.Helpers.GetBoundingBox(runePoints);
        /*size = (corners["bottomCornerA"] - corners["topCornerC"]).magnitude;
        center = corners["center"];*/
    }

    public List<Vector3> getWorldPoints()
    {
        UpdatePoints();

        List<Vector3> worldPoints = new List<Vector3>();

        for (int p = 0; p < runePoints.Count; p++)
        {
            worldPoints.Add(Vector3.Scale(transform.rotation * (runePoints[p] - corners["center"]), transform.localScale) + corners["center"] + transform.position);
        }

        return worldPoints;
    }

    public Dictionary<string, Vector3> getWorldBoundingBox()
    {
        UpdatePoints();

        Dictionary<string, Vector3> worldBoundingBox = new Dictionary<string, Vector3>();

        worldBoundingBox.Add("bottomCornerA", Vector3.Scale(transform.rotation * (corners["bottomCornerA"] - corners["center"]), transform.localScale) + corners["center"] + transform.position);
        worldBoundingBox.Add("bottomCornerB", Vector3.Scale(transform.rotation * (corners["bottomCornerB"] - corners["center"]), transform.localScale) + corners["center"] + transform.position);
        worldBoundingBox.Add("bottomCornerC", Vector3.Scale(transform.rotation * (corners["bottomCornerC"] - corners["center"]), transform.localScale) + corners["center"] + transform.position);
        worldBoundingBox.Add("bottomCornerD", Vector3.Scale(transform.rotation * (corners["bottomCornerD"] - corners["center"]), transform.localScale) + corners["center"] + transform.position);
        worldBoundingBox.Add("topCornerA", Vector3.Scale(transform.rotation * (corners["topCornerA"] - corners["center"]), transform.localScale) + corners["center"] + transform.position);
        worldBoundingBox.Add("topCornerB", Vector3.Scale(transform.rotation * (corners["topCornerB"] - corners["center"]), transform.localScale) + corners["center"] + transform.position);
        worldBoundingBox.Add("topCornerC", Vector3.Scale(transform.rotation * (corners["topCornerC"] - corners["center"]), transform.localScale) + corners["center"] + transform.position);
        worldBoundingBox.Add("topCornerD", Vector3.Scale(transform.rotation * (corners["topCornerD"] - corners["center"]), transform.localScale) + corners["center"] + transform.position);
        worldBoundingBox.Add("center", corners["center"] + transform.position);
        /*worldBoundingBox.Add("up", transform.rotation * up);
        worldBoundingBox.Add("forward", transform.rotation * forward);*/

        return worldBoundingBox;
    }

    public List<Vector3> GetRunePoints()
    {
        return new List<Vector3>(runePoints);
    }

    public void SetRunePoints(List<Vector3> points)
    {
        runePoints = points;
    }

    public bool IsUseCameraForwardToAlign()
    {
        return useCameraForwardToAlign;
    }

    public float GetDeltaThreshold()
    {
        return deltaThreshold;
    }

    private void OnDrawGizmos()
    {
        UpdatePoints();

        List<Vector3> worldPoints = getWorldPoints();
        Dictionary<string, Vector3> worldBoundingBox = getWorldBoundingBox();

        for (int p = 0; p < worldPoints.Count-1; p++)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawLine(worldPoints[p], worldPoints[p+1]);

            Gizmos.color = Color.red;
            Gizmos.DrawSphere(worldPoints[p], 0.01f);

            if (p == worldPoints.Count - 2)
            {
                Gizmos.DrawSphere(worldPoints[p + 1], 0.01f);

            }
        }
        Gizmos.color = Color.blue;
        Gizmos.DrawSphere(worldBoundingBox["center"], 0.015f);

        /*Gizmos.color = Color.magenta;
        Gizmos.DrawLine(worldBoundingBox["center"], worldBoundingBox["center"] + worldBoundingBox["up"] * 0.3f);
        Gizmos.DrawLine(worldBoundingBox["center"], worldBoundingBox["center"] + worldBoundingBox["forward"] * 0.3f);*/

        Gizmos.color = Color.blue;
        Gizmos.DrawSphere(worldBoundingBox["bottomCornerA"], 0.02f);
        Gizmos.DrawSphere(worldBoundingBox["bottomCornerB"], 0.02f);
        Gizmos.DrawSphere(worldBoundingBox["bottomCornerC"], 0.02f);
        Gizmos.DrawSphere(worldBoundingBox["bottomCornerD"], 0.02f);
        Gizmos.DrawSphere(worldBoundingBox["topCornerA"], 0.02f);
        Gizmos.DrawSphere(worldBoundingBox["topCornerB"], 0.02f);
        Gizmos.DrawSphere(worldBoundingBox["topCornerC"], 0.02f);
        Gizmos.DrawSphere(worldBoundingBox["topCornerD"], 0.02f);
    }
}
