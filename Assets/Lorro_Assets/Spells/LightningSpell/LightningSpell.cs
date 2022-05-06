using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightningSpell : Spell
{
    public float speed = 5f;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.position.y > 0)
        {
            transform.position += Vector3.down * Time.deltaTime * speed;
        }
        else
        {
            //Destroy(gameObject);
        }
    }

    public override void Execute()
    {
        SpellManager spellManager = GameObject.Find("SpellManager").GetComponent<SpellManager>();

        GameObject castPoint = spellManager.GetCastPoint();

        //Init
        RaycastHit hit;
        
        if(Physics.Raycast(castPoint.transform.position, castPoint.transform.forward, out hit))
        {
            transform.position = hit.point + Vector3.up * 30;
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
