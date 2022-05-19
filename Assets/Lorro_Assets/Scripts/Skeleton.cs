using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skeleton : MonoBehaviour
{
    [SerializeField]
    private GameObject skull;

    private GameObject skullObj = null;

    private bool earthHit = false;

    // Start is called before the first frame update
    void Start()
    {
        transform.position = transform.position - new Vector3(0, 1, 0);
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
        if (earthHit && transform.position.y < 0)
        {
            transform.position += Vector3.up * Time.deltaTime;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Earth"))
        {
            earthHit = true;
        }

        Debug.Log("erathHit: "+earthHit+" / other tag: "+other.tag);
        if (earthHit && other.CompareTag("Lightning") && skullObj == null)
        {
            skullObj = Instantiate(skull, GameObject.Find("Rotation").transform);
            skullObj.transform.position = transform.position;
        }
    }

    public void ResetObj()
    {
        earthHit = false;
        transform.position = new Vector3(transform.position.x, -1, transform.position.z);
    }
}
