using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellManager : MonoBehaviour
{
    public GameObject castPointObject;
    private CastPoint castPoint;

    public bool recordRunes = false;
    public Rune emptyRune;
    public List<SpellObject> castableSpells;

    public bool debugRunes;

    private Transform cam;

    private bool DEBUG_isInvestigating = false;
    public List<Vector3> DEBUG_suspectsPoints;
    private Dictionary<string, Vector3> DEBUG_suspectsBounds;

    // Start is called before the first frame update
    void Start()
    {
        castPoint = castPointObject.GetComponent<CastPoint>();

        cam = Camera.main.transform;
    }

    // Update is called once per frame
    void Update()
    {
        if (castPoint.isPossibleRuneDetected() && !castPoint.wasCurrentPossibleRunePickedUp())
        {
            DEBUG_isInvestigating = true;
            CheckRune();
        }
    }

    public void SetCastPoint()
    {

    }

    private void CheckRune()
    {
        List<Vector3> suspectPoints = castPoint.getPossibleRunePoints();

        //The spell that the user probably wanted to cast
        SpellObject mostLikelyToCastSpell = null;

        //The gap to the delta threshold that is needed to cast the spell sucessfully (= certainty that user means this spell)
        float mostLikelyToCastSpellGapToDeltaThreshold = 0f;

        if (recordRunes)
        {
            Rune recordedRune = Instantiate(emptyRune);
            recordedRune.gameObject.name = "RecordedRune";
            recordedRune.runePoints = suspectPoints;
        }

        Dictionary<string, Vector3> suspectCorners = Helpers.Helpers.GetBoundingBox(suspectPoints);
        float suspectSize = (suspectCorners["bottomCornerA"] - suspectCorners["topCornerC"]).magnitude;
        Vector3 suspectCenter = suspectCorners["center"];

        //Possible to link runes with spell object without deleting them every time?
        GameObject runeContainer = new GameObject("RuneContainer");

        Debug.ClearDeveloperConsole();

        for (int c = 0; c < castableSpells.Count; c++)
        {
            SpellObject castableSpell = castableSpells[c];
            Debug.Log("=================== " + castableSpell.name + " ========================");

            for (int r = 0; r < castableSpell.runes.Count; r++)
            {
                GameObject rune = Instantiate(castableSpell.runes[r], runeContainer.transform);
                Rune castableRune = rune.GetComponent<Rune>();

                Dictionary<string, Vector3> castableRuneWorldBoundingBox = castableRune.getWorldBoundingBox();

                float castableRuneSize = (castableRuneWorldBoundingBox["bottomCornerA"] - castableRuneWorldBoundingBox["topCornerC"]).magnitude;

                //Start of Dynamic Rune Adjustment

                //Scale
                //For now all axis are scaled the same
                float scaleFactor = suspectSize / castableRuneSize;
                castableRune.transform.localScale *= scaleFactor;

                //Translate
                Vector3 offset = suspectCenter - castableRuneWorldBoundingBox["center"];
                castableRune.transform.position += offset;

                //Rotate
                Quaternion rotation;

                if (!castableRune.useCameraForwardToAlign)
                {
                    //Project the up vector of the HMD to the plane defined by right and up of the wand (-> cast point)
                    //Since only the normal of the plane is needed the plane does not need to be built and the castPoint.normal vector is enough
                    Vector3 projectedCamUp = Vector3.ProjectOnPlane(cam.up, castPoint.transform.forward);
                    rotation = Quaternion.LookRotation(castPoint.transform.forward, projectedCamUp);
                }
                else
                {
                    rotation = Quaternion.LookRotation(cam.forward, cam.up);
                }

                castableRune.transform.rotation = rotation;

                //End of Dynamic Rune Adjustment

                //Compare Runes
                List<Vector3> castableRuneWorldPoints = castableRune.getWorldPoints();
                DEBUG_suspectsPoints = suspectPoints;
                DEBUG_suspectsBounds = suspectCorners;

                float delta = 0;

                float pointCountFactor;

                List<Vector3> biggerList;
                List<Vector3> smallerList;

                if (castableRuneWorldPoints.Count > suspectPoints.Count)
                {
                    biggerList = new List<Vector3>(castableRuneWorldPoints);
                    smallerList = new List<Vector3>(suspectPoints);
                }
                else
                {
                    biggerList = new List<Vector3>(suspectPoints);
                    smallerList = new List<Vector3>(castableRuneWorldPoints);
                }

                pointCountFactor = (float)biggerList.Count / (float)smallerList.Count;

                for (int p = 0; p < biggerList.Count; p++)
                {
                    delta += (biggerList[p] - smallerList[Mathf.FloorToInt(p / pointCountFactor)]).magnitude;
                }

                delta /= biggerList.Count;

                float deltaMultiplier = scaleFactor > 1 ? scaleFactor : 1 / scaleFactor;

                delta *= 100 * deltaMultiplier;

                Debug.Log("Delta for "+castableRune.gameObject.name+" is " + delta + " / threshold is : "+castableRune.deltaThreshold);

                if (delta < castableRune.deltaThreshold)
                {
                    float gapTopDeltaThreshold = delta - castableRune.deltaThreshold;

                    if (gapTopDeltaThreshold < mostLikelyToCastSpellGapToDeltaThreshold)
                    {
                        mostLikelyToCastSpellGapToDeltaThreshold = gapTopDeltaThreshold;
                        mostLikelyToCastSpell = castableSpell;
                        Debug.Log("Expecting " + castableSpell.name + " from " + castableRune.gameObject.name + " to be meant | Gap to delta: "+mostLikelyToCastSpellGapToDeltaThreshold);
                    }
                }
            }
        }

        if (mostLikelyToCastSpell != null)
        {
            Debug.Log("Executing "+mostLikelyToCastSpell.name);

            //Instantiate spell and execute
            GameObject spellObject = Instantiate(mostLikelyToCastSpell.spellScriptObject, GameObject.Find("ActiveSpells").transform);
            Spell spell = spellObject.GetComponent<Spell>();

            spell.Execute();
        }

        if (!debugRunes)
        {
            Destroy(runeContainer);
        }
        Debug.Log("Ready for next Rune");
    }

    public GameObject GetCastPoint()
    {
        return castPoint.gameObject;
    }

    private void OnDrawGizmos()
    {
        /*if (DEBUG_isInvestigating)
        {
            Gizmos.color = Color.magenta;
            Gizmos.DrawLine(DEBUG_suspectsPoints[0], DEBUG_suspectsBounds["center"]);
            Gizmos.DrawLine(DEBUG_suspectsPoints[0], DEBUG_suspectsPoints[DEBUG_suspectsPoints.Count - 1]);
        }*/
        
        /*if (DEBUG_isInvestigating)
        {
            for (int p = 0; p < DEBUG_suspectsPoints.Count - 1; p++)
            {
                Gizmos.color = Color.red;
                Gizmos.DrawLine(transform.position + DEBUG_suspectsPoints[p], transform.position + DEBUG_suspectsPoints[p + 1]);

                Gizmos.color = Color.red;
                Gizmos.DrawSphere(transform.position + DEBUG_suspectsPoints[p], 0.01f);

                if (p == DEBUG_suspectsPoints.Count - 2)
                {
                    Gizmos.DrawSphere(transform.position + DEBUG_suspectsPoints[p + 1], 0.01f);

                }
            }
            
            Gizmos.color = Color.yellow;
            Gizmos.DrawSphere(DEBUG_suspectCorners["center"], 0.015f);

            Gizmos.color = Color.yellow;
            Gizmos.DrawLine(DEBUG_suspectCorners["center"], DEBUG_suspectCorners["center"] + cam.forward);

            Gizmos.color = Color.yellow;
            Gizmos.DrawSphere(DEBUG_suspectCorners["bottomCornerA"], 0.02f);
            Gizmos.DrawSphere(DEBUG_suspectCorners["bottomCornerB"], 0.02f);
            Gizmos.DrawSphere(DEBUG_suspectCorners["bottomCornerC"], 0.02f);
            Gizmos.DrawSphere(DEBUG_suspectCorners["bottomCornerD"], 0.02f);
            Gizmos.DrawSphere(DEBUG_suspectCorners["topCornerA"], 0.02f);
            Gizmos.DrawSphere(DEBUG_suspectCorners["topCornerB"], 0.02f);
            Gizmos.DrawSphere(DEBUG_suspectCorners["topCornerC"], 0.02f);
            Gizmos.DrawSphere(DEBUG_suspectCorners["topCornerD"], 0.02f);
        }*/
    }
}
