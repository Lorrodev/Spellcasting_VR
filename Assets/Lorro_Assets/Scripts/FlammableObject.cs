using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlammableObject : MonoBehaviour
{
    Collider coll;

    [SerializeField]
    private GameObject fireObj;

    private GameObject fire;

    // Start is called before the first frame update
    void Start()
    {
        coll = GetComponent<Collider>();
        coll.isTrigger = true;

        fire = Instantiate(fireObj, transform);
        fire.SetActive(false);
    }

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

    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log(other.tag);
        if (other.CompareTag("Fire") || other.CompareTag("Lightning"))
        {
            fire.SetActive(true);
        }
    }

    public void ResetObj()
    {
        fire.SetActive(false);
    }
}
