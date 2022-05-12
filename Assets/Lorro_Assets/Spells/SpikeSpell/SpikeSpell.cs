using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikeSpell : Spell
{
    public List<AudioClip> impactSounds;
    public AudioClip rumblingSound;

    private float TTL = 7f;
    private AudioSource ac;
    private CapsuleCollider cc;

    private bool impactSoundPlayed = false;

    // Start is called before the first frame update
    void Awake()
    {
        ac = GetComponent<AudioSource>();
        cc = GetComponent<CapsuleCollider>();
    }

    // Update is called once per frame
    void Update()
    {
        TTL -= Time.deltaTime;

        if (7f - TTL >= 0.18f && !impactSoundPlayed)
        {
            cc.enabled = false;
            ac.Stop();
            ac.PlayOneShot(impactSounds[Random.Range(0, impactSounds.Count)]);
            impactSoundPlayed = true;
        }

        if (TTL <= 0)
        {
            Destroy(gameObject);
        }
    }

    public override void Execute()
    {
        Camera cam = Camera.main;
        transform.position = new Vector3(cam.transform.position.x, 0, cam.transform.position.z);
        transform.LookAt(transform.position + new Vector3(cam.transform.forward.x, 0, cam.transform.forward.z), Vector3.up);

        ac.PlayOneShot(rumblingSound);
    }
}
