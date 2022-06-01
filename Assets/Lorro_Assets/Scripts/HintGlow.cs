using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HintGlow : MonoBehaviour
{
    ParticleSystem ps;

    [SerializeField]
    private float glowDuration = 1.5f;

    private float glowTime = 0f;

    // Start is called before the first frame update
    void Start()
    {
        ps = transform.Find("FX_GlowSpot_03").GetComponent<ParticleSystem>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!ps.emission.enabled && Random.Range(0f,1f) > 0.997f)
        {
            glowTime = 0f;

            var e = ps.emission;
            e.enabled = true;
        }

        if (ps.emission.enabled && glowTime >= glowDuration)
        {
            var e = ps.emission;
            e.enabled = false;
        }
        else if(ps.emission.enabled)
        {
            glowTime += Time.deltaTime;
        }
    }
}
