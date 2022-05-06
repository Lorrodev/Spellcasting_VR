using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikeSpell : Spell
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.position.y < 0)
        {
            transform.position += Vector3.up * Time.deltaTime * 2f;
        }
    }

    public override void Execute()
    {
        transform.position = Camera.main.transform.position + Camera.main.transform.forward * 1.5f - Vector3.up * 2f;
    }
}
