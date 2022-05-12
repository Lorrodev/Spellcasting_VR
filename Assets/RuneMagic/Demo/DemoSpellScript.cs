using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DemoSpellScript : Spell
{
    private float TTL = 2f;

    private void Update()
    {
        TTL -= Time.deltaTime;

        if (TTL <= 0)
        {
            Destroy(gameObject);
        }
    }

    public override void Execute()
    {
        Debug.Log("Demo Spell was executed");
    }
}
