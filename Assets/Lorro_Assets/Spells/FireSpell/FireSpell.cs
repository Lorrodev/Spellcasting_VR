using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireSpell : Spell
{
    public float speed = 5f;
    public float TTL = 10;

    private Vector3 direction;
    private SphereCollider sc;

    // Start is called before the first frame update
    void Start()
    {
        sc = GetComponent<SphereCollider>();
        sc.isTrigger = true;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += direction * Time.deltaTime * speed;

        TTL -= Time.deltaTime;

        if (TTL <= 0)
        {
            Destroy(gameObject);
        }
    }

    public override void Execute()
    {
        SpellManager spellManager = GameObject.Find("SpellManager").GetComponent<SpellManager>();

        GameObject castPoint = spellManager.GetCastPoint();

        //Init
        transform.position = castPoint.transform.position;
        direction = castPoint.transform.forward;
    }
}
