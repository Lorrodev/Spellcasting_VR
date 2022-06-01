using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DistinguishingFire : MonoBehaviour
{
    public float TTL = 7f;

    private AudioSource ac;

    ParticleSystem ps;
    ParticleSystem sps;
    private void OnEnable()
    {
        FindObjectOfType<DemoManager>().onResetDemo += ResetObj;
    }
    private void OnDisable()
    {
        FindObjectOfType<DemoManager>().onResetDemo -= ResetObj;
    }

    private void Awake()
    {
        ps = transform.Find("FX_Fire_Big_03").GetComponent<ParticleSystem>();
        sps = transform.Find("FX_Fire_Big_03").transform.Find("FX_Fire_Big_Embers_01").GetComponent<ParticleSystem>();
        var e = ps.emission;
        e.enabled = true;

        e = sps.emission;
        e.enabled = true;

        ac = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        TTL -= Time.deltaTime;

        if (TTL <= 0)
        {
            if (ps.isEmitting)
            {
                var e = ps.emission;
                e.enabled = false;

                e = sps.emission;
                e.enabled = false;
            }

            ac.volume -= Time.deltaTime;
        }
        else if(TTL <= -5f)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Destructable"))
        {
            TTL = 0f;
        }
    }

    public void ResetObj()
    {
        Destroy(gameObject);
    }
}
