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
    private bool trainingMode = false;
    private Trainer trainer;

    [SerializeField]
    private bool debugRunes;

    private Transform cam;

    // Start is called before the first frame update
    void Start()
    {
        castPoint = castPointObject.GetComponent<CastPoint>();

        cam = Camera.main.transform;

        trainer = GetComponent<Trainer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!trainingMode)
        {

            if (castPoint.isPossibleRuneDetected() && !castPoint.wasCurrentPossibleRunePickedUp())
            {
                CheckRune();
            }
        }

        if (trainer.IsTrainingComplete())
        {
            trainingMode = false;
        }
    }

    private void CheckRune()
    {
        List<Vector3> suspectPoints = castPoint.getPossibleRunePoints();

        //The spell that the user probably wanted to cast
        SpellObject mostLikelyToCastSpell = null;

        //The gap to the delta threshold that is needed to cast the spell sucessfully (= certainty that user means this spell)
        //float mostLikelyToCastSpellGapToDeltaThreshold = 0f;
        float mostLikelyToCastSpellDelta = Mathf.Infinity;

        Rune suspectRune = Instantiate(emptyRune);
        suspectRune.gameObject.name = "RecordedRune";
        suspectRune.SetRunePoints(suspectPoints);

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

                float scaleFactor = suspectRune.GetSize() / castableRune.GetSize();

                castableRune.FitToRune(suspectRune);
                float delta = castableRune.GetDeltaToRune(suspectRune);

                float deltaMultiplier = scaleFactor > 1 ? scaleFactor : 1 / scaleFactor;
                delta *= 100 * deltaMultiplier;

                if (delta < castableRune.GetDeltaThreshold())
                {
                    if (delta < mostLikelyToCastSpellDelta)
                    {
                        mostLikelyToCastSpellDelta = delta;
                        mostLikelyToCastSpell = castableSpell;
                        Debug.Log("Might be " + castableSpell.name + " from " + castableRune.gameObject.name + " | Delta: " + mostLikelyToCastSpellDelta);
                    }
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

        if (!recordRunes)
        {
            Destroy(suspectRune.gameObject);
        }

        if (!debugRunes)
        {
            Destroy(runeContainer);
        }
    }

    public bool IsTrainingMode()
    {
        return trainingMode;
    }

    public GameObject GetCastPoint()
    {
        return castPoint.gameObject;
    }

    public Rune GetEmptyRune()
    {
        return emptyRune;
    }

    public List<SpellObject> GetCastableSpells()
    {
        //By Referrence!
        return castableSpells;
    }
}
