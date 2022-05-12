using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkullEnemy : MonoBehaviour
{
    void Awake()
    {
    }

    // Update is called once per frame
    void Update()
    {
        transform.LookAt(Camera.main.transform.position);

        if (transform.position.y < 2f)
        {
            transform.position += Vector3.up * Time.deltaTime;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Fire") || other.CompareTag("Lightning"))
        {
            Destroy(gameObject);
        }
    }
}
