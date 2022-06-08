using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireSpell : Spell
{
    public float speed = 5f;
    public float TTL = 10;

    public GameObject explosionEffect;
    public GameObject fireEffect;

    public List<AudioClip> castSounds;
    public AudioClip flightSound;
    public List<AudioClip> impactSounds;


    private Vector3 direction;
    private SphereCollider sc;
    private AudioSource ac;
    private ParticleSystem ps;
    private Light lt;

    private bool isFlying = false;
    private bool collided = false;

    private SpellManager sm;
    private GameObject cp;

    void Awake()
    {
        ac = GetComponent<AudioSource>();
        sc = GetComponent<SphereCollider>();
        ps = GetComponent<ParticleSystem>();
        lt = GetComponent<Light>();

        sc.isTrigger = true;

        sm = GameObject.Find("SpellManager").GetComponent<SpellManager>();
    }

    private void OnEnable()
    {
        FindObjectOfType<DemoManager>().onResetDemo += ResetObj;
    }
    private void OnDisable()
    {
        FindObjectOfType<DemoManager>().onResetDemo -= ResetObj;
    }

    // Update is called once per frame
    void Update()
    {
        if (isFlying)
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
        else
        {
            transform.position = cp.transform.position;
        }
    }

    public override void Execute(CastInfo castInfo)
    {
        cp = castInfo.GetCastPoint().gameObject;

        transform.position = cp.transform.position;

        ac.PlayOneShot(flightSound);
        ac.PlayOneShot(castSounds[Random.Range(0, castSounds.Count)]);
        Invoke("Fire", 0.8f);
    }

    private void Fire()
    {
        direction = cp.transform.forward;

        ac.PlayOneShot(castSounds[Random.Range(0, castSounds.Count)]);
        ps.Play();

        isFlying = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (isFlying && !collided)
        {
            ps.Stop();
            Instantiate(explosionEffect, transform);

            AudioClip sound = impactSounds[Random.Range(0, castSounds.Count)];

            ac.Stop();

            ac.spatialBlend = 0.5f;
            ac.PlayOneShot(sound);

            TTL = sound.length;

            collided = true;

            if (other.CompareTag("Player"))
            {
                GameObject.Find("DemoManager").GetComponent<DemoManager>().onResetDemo?.Invoke();
            }else if (!other.CompareTag("NoFire"))
            {
                GameObject obj = Instantiate(fireEffect, GameObject.Find("ActiveEffects").transform);
                obj.transform.position = transform.position;
            }
        }
    }
    public void ResetObj()
    {
        Destroy(gameObject);
    }
}
