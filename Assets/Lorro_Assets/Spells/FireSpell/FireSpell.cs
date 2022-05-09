using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireSpell : Spell
{
    public float speed = 5f;
    public float TTL = 10;

    public List<AudioClip> castSounds;
    public AudioClip flightSound;
    public List<AudioClip> impactSounds;

    private Vector3 direction;
    private SphereCollider sc;
    private AudioSource ac;

    private Vector3 origin;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        transform.position += direction * Time.deltaTime * speed;
        //transform.position += new Vector3((Random.value - 0.5f) * Time.deltaTime * (speed/5), (Random.value - 0.5f) * Time.deltaTime * (speed / 5), (Random.value - 0.5f) * Time.deltaTime * (speed / 5));

        TTL -= Time.deltaTime;

        if (TTL <= 0)
        {
            Destroy(gameObject);
        }
    }

    public override void Execute()
    {
        ac = GetComponent<AudioSource>();
        sc = GetComponent<SphereCollider>();

        SpellManager spellManager = GameObject.Find("SpellManager").GetComponent<SpellManager>();

        GameObject castPoint = spellManager.GetCastPoint();

        //Init
        transform.position = castPoint.transform.position;
        direction = castPoint.transform.forward;
        origin = transform.position;

        ac.PlayOneShot(castSounds[Random.Range(0, castSounds.Count)]);
        ac.PlayOneShot(flightSound);
    }
}
