using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trainer : MonoBehaviour
{
    [SerializeField]
    private GameObject ghostSphere;

    private bool trainingComplete = false;

    private SpellManager spellManager;

    private Camera cam;

    private int spellNr = 0;
    private bool nextSpellFlag = true;
    private bool awaitingUserRune = false;

    private SpellObject currentSpellObj;
    private GameObject currentRuneObj;
    private Rune currentRune;

    private CastPoint castPoint;

    // Start is called before the first frame update
    void Start()
    {
        spellManager = GetComponent<SpellManager>();
        castPoint = spellManager.GetCastPoint().GetComponent<CastPoint>();

        ghostSphere = Instantiate(ghostSphere, transform);

        cam = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        if (spellManager.IsTrainingMode())
        {
            if (nextSpellFlag)
            {
                currentSpellObj = spellManager.GetCastableSpells()[spellNr];
                currentRuneObj = Instantiate(currentSpellObj.GetRunes()[0], transform);
                currentRune = currentRuneObj.GetComponent<Rune>();

                nextSpellFlag = false;
            }

            if (!awaitingUserRune)
            {
                if ((ghostSphere.transform.position - castPoint.transform.position).magnitude < 0.1f)
                {
                    StartCoroutine(currentRune.Animate());

                    awaitingUserRune = true;
                }
                else if (currentRuneObj != null)
                {
                    Vector3 projectedCamForward = Vector3.ProjectOnPlane(Camera.main.transform.forward, Vector3.up);

                    currentRuneObj.transform.position += (cam.transform.position + projectedCamForward.normalized * 0.6f - currentRuneObj.transform.position) * Time.deltaTime * 2.5f;

                    currentRuneObj.transform.rotation = Quaternion.LookRotation(projectedCamForward, Vector3.up);

                    currentRuneObj.transform.localScale = new Vector3(0.7f, 0.7f, 0.7f);

                    //Place ghost sphere at first point of rune path
                    ghostSphere.transform.position = currentRune.getWorldPoints()[0];
                }
            }
            else
            {
                //Wait for a suspected rune to be picked up from cast point
                if (castPoint.isPossibleRuneDetected() && !castPoint.wasCurrentPossibleRunePickedUp())
                {
                    List<Vector3> suspectPoints = castPoint.getPossibleRunePoints();

                    Rune suspectRune = Instantiate(spellManager.GetEmptyRune());
                    suspectRune.gameObject.name = "TrainingRune";
                    suspectRune.SetRunePoints(suspectPoints);

                    float delta = currentRune.GetDeltaToRune(suspectRune);
                    delta *= 100;

                    Debug.Log("TRAINING: Did " + currentRune.name + " with delta of " + delta);

                    if (delta < currentRune.GetDeltaThreshold())
                    {
                        //Instantiate spell and execute
                        GameObject spellObject = Instantiate(currentSpellObj.GetSpellScriptObject(), transform);
                        Spell spell = spellObject.GetComponent<Spell>();
                        spell.Execute();

                        //if spell was cast successfully either continue to next spell or..
                        if (spellNr < spellManager.GetCastableSpells().Count - 1)
                        {
                            spellNr++;
                            nextSpellFlag = true;
                            Destroy(currentRune.gameObject);
                            Destroy(suspectRune.gameObject);
                        }
                        //or end training if it was the last spell
                        else
                        {
                            trainingComplete = true;
                            Destroy(currentRune.gameObject);
                            Destroy(suspectRune.gameObject);
                            Destroy(ghostSphere.gameObject);
                            Debug.Log("TRAINING: Training complete!");
                        }
                    }
                    else
                    {
                        awaitingUserRune = false;
                        Destroy(suspectRune.gameObject);
                        Debug.Log("TRAINING: Rune "+currentRune.name+" not recognized, fallback.");
                    }
                }
            }
        }
    }

    public bool IsTrainingComplete()
    {
        return trainingComplete;
    }
}
