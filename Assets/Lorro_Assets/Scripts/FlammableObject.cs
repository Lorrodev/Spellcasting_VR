using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlammableObject : MonoBehaviour
{
    Collider coll;

    public GameObject fireObj;

    private GameObject fire;
    private ParticleSystem ps;
    private Light lt;

    // Start is called before the first frame update
    void Start()
    {
        coll = GetComponent<Collider>();
        coll.isTrigger = true;

        lt = GetComponent<Light>();
        ps = GetComponent<ParticleSystem>();
        ps.Stop();

        fire = Instantiate(fireObj, transform);
        fire.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log(other.tag);
        if (other.CompareTag("Fire"))
        {
            fire.SetActive(true);
            lt.enabled = true;
            ps.Play();
        }
    }
}
