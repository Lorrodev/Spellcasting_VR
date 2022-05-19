using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkullEnemy : MonoBehaviour
{
    private float iFrameSecs = 0.3f;

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
        if (iFrameSecs > 0)
        {
            iFrameSecs -= Time.deltaTime;
        }

        transform.LookAt(Camera.main.transform.position);

        if (transform.position.y < 2f)
        {
            transform.position += Vector3.up * Time.deltaTime;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (iFrameSecs <= 0 && (other.CompareTag("Fire") || other.CompareTag("Lightning")))
        {
            Destroy(gameObject);
        }
    }

    public void ResetObj()
    {
        Destroy(gameObject);
    }
}
