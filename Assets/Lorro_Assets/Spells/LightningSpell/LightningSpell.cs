using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightningSpell : Spell
{
    public GameObject effect;
    public List<AudioClip> castSounds;

    private AudioSource ac;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (!ac.isPlaying)
        {
            Destroy(gameObject);
        }
    }

    public override void Execute()
    {
        SpellManager spellManager = GameObject.Find("SpellManager").GetComponent<SpellManager>();

        GameObject castPoint = spellManager.GetCastPoint();

        //Init
        RaycastHit hit;
        
        if(Physics.Raycast(castPoint.transform.position, castPoint.transform.forward, out hit))
        {
            transform.position = hit.point;
            Instantiate(effect, transform);

            ac = GetComponent<AudioSource>();
            ac.PlayOneShot(castSounds[Random.Range(0, castSounds.Count)]);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
