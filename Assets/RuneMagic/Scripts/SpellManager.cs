using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellManager : MonoBehaviour
{
    [SerializeField]
    private GameObject castPointObject;
    private CastPoint castPoint;

    [SerializeField]
    private bool recordRunes = false;
    [SerializeField]
    private Rune emptyRune;
    [SerializeField]
    private List<SpellObject> castableSpells;

    [SerializeField]
    private bool debugRunes;

    private Transform cam;

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
        //float mostLikelyToCastSpellGapToDeltaThreshold = 0f;
        float mostLikelyToCastSpellDelta = Mathf.Infinity;

        if (recordRunes)
        {
            Rune recordedRune = Instantiate(emptyRune);
            recordedRune.gameObject.name = "RecordedRune";
            recordedRune.SetRunePoints(suspectPoints);
        }

        Dictionary<string, Vector3> suspectCorners = Helpers.Helpers.GetBoundingBox(suspectPoints);
        float suspectSize = (suspectCorners["bottomCornerA"] - suspectCorners["topCornerC"]).magnitude;
        Vector3 suspectCenter = suspectCorners["center"];

        //Possible to link runes with spell object without deleting them every time?
        GameObject runeContainer = new GameObject("RuneContainer");

        for (int c = 0; c < castableSpells.Count; c++)
        {
            SpellObject castableSpell = castableSpells[c];
            Debug.Log("=================== " + castableSpell.name + " ========================");

            List<GameObject> castableSpellRunes = castableSpell.GetRunes();
            for (int r = 0; r < castableSpellRunes.Count; r++)
            {
                GameObject rune = Instantiate(castableSpellRunes[r], runeContainer.transform);
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

                if (!castableRune.IsUseCameraForwardToAlign())
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

                Debug.Log("Delta for "+castableRune.gameObject.name+" is " + delta + " / threshold is : "+castableRune.GetDeltaThreshold());

                if (delta < castableRune.GetDeltaThreshold())
                {
                    //float gapTopDeltaThreshold = delta - castableRune.GetDeltaThreshold();

                    if (delta < mostLikelyToCastSpellDelta)
                    {
                        mostLikelyToCastSpellDelta = delta;
                        mostLikelyToCastSpell = castableSpell;
                        Debug.Log("Might be " + castableSpell.name + " from " + castableRune.gameObject.name + " | Delta: " + mostLikelyToCastSpellDelta);
                    }

                    /*float gapTopDeltaThreshold = delta - castableRune.GetDeltaThreshold();

                    if (gapTopDeltaThreshold < mostLikelyToCastSpellGapToDeltaThreshold)
                    {
                        mostLikelyToCastSpellGapToDeltaThreshold = gapTopDeltaThreshold;
                        mostLikelyToCastSpell = castableSpell;
                        Debug.Log("Might be " + castableSpell.name + " from " + castableRune.gameObject.name + " | Gap to delta: " + mostLikelyToCastSpellGapToDeltaThreshold);
                    }*/
                }
            }
        }

        if (mostLikelyToCastSpell != null)
        {
            Debug.Log("Executing "+mostLikelyToCastSpell.name);

            GameObject activeSpells = GameObject.Find("ActiveSpells");
            if (activeSpells == null)
            {
                activeSpells = new GameObject("ActiveSpells");
            }

            //Instantiate spell and execute
            GameObject spellObject = Instantiate(mostLikelyToCastSpell.GetSpellScriptObject(), activeSpells.transform);
            Spell spell = spellObject.GetComponent<Spell>();

            spell.Execute();
        }
        else
        {
            Debug.Log("Not likely a spell");
        }

        if (!debugRunes)
        {
            Destroy(runeContainer);
        }
    }

    public GameObject GetCastPoint()
    {
        return castPoint.gameObject;
    }
}
