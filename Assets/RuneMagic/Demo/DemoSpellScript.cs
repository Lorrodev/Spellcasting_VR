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

    public override void Execute(CastInfo castInfo)
    {
        Debug.Log("Demo Spell was executed!");
        Debug.Log("Cast Point is at: " + castInfo.GetCastPoint().transform.position);
        Debug.Log("Scale Factor is " + castInfo.GetScaleFactor());
        Debug.Log("Speed Factor is " + castInfo.GetSpeedFactor());
        Debug.Log("Delta is " + castInfo.GetDelta());
        Debug.Log("Drawn path contains " + castInfo.GetRune().GetRunePoints().Count + " points");
    }
}
