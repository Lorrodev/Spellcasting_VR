using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikeSpell : Spell
{
    public GameObject visual;

    private float TTL = 7f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        TTL -= Time.deltaTime;

        if (TTL <= 0)
        {
            Destroy(gameObject);
        }
    }

    public override void Execute()
    {
        Camera cam = Camera.main;
        transform.position = new Vector3(cam.transform.position.x, 0, cam.transform.position.z);

        GameObject visualFX = Instantiate(visual, transform);
        visualFX.transform.LookAt(transform.position + new Vector3(cam.transform.forward.x, 0, cam.transform.forward.z), Vector3.up);
    }
}
