using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireSpell : Spell
{
    public float speed = 5f;
    public float TTL = 10;

    public GameObject explosionEffect;

    public List<AudioClip> castSounds;
    public AudioClip flightSound;
    public List<AudioClip> impactSounds;

    private Vector3 direction;
    private SphereCollider sc;
    private AudioSource ac;
    private ParticleSystem ps;
    private Light lt;

    private bool collided = false;

    void Awake()
    {
        ac = GetComponent<AudioSource>();
        sc = GetComponent<SphereCollider>();
        ps = GetComponent<ParticleSystem>();
        lt = GetComponent<Light>();

        sc.isTrigger = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (!collided)
        {
            transform.position += direction * Time.deltaTime * speed;
        }
        else
        {
            lt.intensity -= Time.deltaTime;
        }

        TTL -= Time.deltaTime;

        if (TTL <= 0)
        {
            Destroy(gameObject);
        }
    }

    public override void Execute()
    {
        SpellManager spellManager = GameObject.Find("SpellManager").GetComponent<SpellManager>();

        GameObject castPoint = spellManager.GetCastPoint();

        //Init
        transform.position = castPoint.transform.position;
        direction = castPoint.transform.forward;

        ac.PlayOneShot(castSounds[Random.Range(0, castSounds.Count)]);
        ac.PlayOneShot(flightSound);
    }

    private void OnTriggerEnter(Collider other)
    {
        ps.Stop();
        Instantiate(explosionEffect, transform);

        AudioClip sound = impactSounds[Random.Range(0, castSounds.Count)];

        ac.Stop();

        ac.spatialBlend = 0.5f;
        ac.PlayOneShot(sound);

        TTL = sound.length;

        collided = true;
    }
}
