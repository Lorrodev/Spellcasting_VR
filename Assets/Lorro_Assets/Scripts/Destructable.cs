using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destructable : MonoBehaviour
{
    [SerializeField]
    private bool byFire = true;
    [SerializeField]
    private bool byLightning = true;
    [SerializeField]
    private bool byEarth = true;
    [SerializeField]
    private float respawnTime = 20f;

    private float deadTime = 0f;

    private float targetScale = 1f;

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
        if (transform.localScale.x <= 0.01f && targetScale == 0)
        {
            deadTime += Time.deltaTime;

            if(deadTime >= respawnTime)
            {
                targetScale = 1f;
            }
        }
        else if(Mathf.Abs(transform.localScale.x - targetScale) > 0.01f)
        {
            float scale = transform.localScale.x + (Mathf.Sign(targetScale - transform.localScale.x) * Time.deltaTime * 5f);
            transform.localScale = new Vector3(scale, scale, scale);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if ((other.CompareTag("Fire") && byFire) || (other.CompareTag("Lightning") && byLightning) || (other.CompareTag("Earth") && byEarth))
        {
            deadTime = 0f;
            targetScale = 0f;
        }
    }

    public void ResetObj()
    {
        targetScale = 1f;
        deadTime = respawnTime;
        transform.localScale = Vector3.one;
    }

}
