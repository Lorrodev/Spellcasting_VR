using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trainer : MonoBehaviour
{
    [SerializeField]
    private GameObject ghostSphere;

    private bool trainingComplete = false;

    private SpellManager spellManager;

    // Start is called before the first frame update
    void Start()
    {
        spellManager = GetComponent<SpellManager>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public bool IsTrainingComplete()
    {
        return trainingComplete;
    }
}
