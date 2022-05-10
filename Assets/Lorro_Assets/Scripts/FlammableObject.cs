using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlammableObject : MonoBehaviour
{
    Collider coll;

    public GameObject fireObj;

    private GameObject fire;
    private Light lt;

    // Start is called before the first frame update
    void Start()
    {
        coll = GetComponent<Collider>();
        coll.isTrigger = true;

        lt = GetComponent<Light>();

        fire = Instantiate(fireObj, transform);
        fire.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (false && lt.enabled)
        {
            lt.range += Random.Range(-0.1f, 0.1f);

            if (lt.range < 0.3f)
            {
                lt.range = 0.3f;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log(other.tag);
        if (other.CompareTag("Fire"))
        {
            fire.SetActive(true);
            lt.enabled = true;
        }
    }
}
