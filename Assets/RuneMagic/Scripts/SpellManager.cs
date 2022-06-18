using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellManager : MonoBehaviour
{
    [SerializeField]
    private CastPoint castPoint;

    [SerializeField]
    private bool recordRunes = false;
    [SerializeField]
    private Rune emptyRune;
    [SerializeField]
    private List<SpellContainer> castableSpells;

    [SerializeField]
    private bool debugRunes;

    // Update is called once per frame
    void Update()
    {
        if (castPoint.isPossibleRuneDetected() && !castPoint.wasCurrentPossibleRunePickedUp())
        {
            CheckRune();
        }
    }

    private void CheckRune()
    {
        CastInfo castInfo = new CastInfo();

        List<Vector3> suspectPoints = castPoint.getPossibleRunePoints();

        //The spell that the user probably wanted to cast
        SpellContainer mostLikelyToCastSpell = null;

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
            SpellContainer castableSpell = castableSpells[c];
            //Debug.Log("=================== " + castableSpell.name + " ========================");

            List<Rune> castableSpellRunes = castableSpell.GetRunes();
            for (int r = 0; r < castableSpellRunes.Count; r++)
            {
                GameObject rune = Instantiate(castableSpellRunes[r].gameObject, runeContainer.transform);
                Rune castableRune = rune.GetComponent<Rune>();

                float scaleFactor = suspectRune.GetSize() / castableRune.GetSize();
                float speedFactor = (float)castableRune.GetRunePoints().Count / (float)suspectPoints.Count;

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
                        Debug.Log("Might be " + castableSpell.GetSpellName() + " from " + castableRune.gameObject.name + " | Delta: " + mostLikelyToCastSpellDelta);

                        castInfo.SetCastPoint(castPoint);
                        castInfo.SetRune(suspectRune);
                        castInfo.SetScaleFactor(scaleFactor);
                        castInfo.SetSpeedFactor(speedFactor);
                        castInfo.SetDelta(delta);
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
            GameObject spellGameObject = Instantiate(mostLikelyToCastSpell.GetSpell().gameObject, activeSpells.transform);
            Spell spell = spellGameObject.GetComponent<Spell>();
            spell.Execute(castInfo);
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

    public CastPoint GetCastPoint()
    {
        return castPoint;
    }

    public Rune GetEmptyRune()
    {
        return emptyRune;
    }
}
