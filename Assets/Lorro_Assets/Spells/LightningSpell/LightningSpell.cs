using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightningSpell : Spell
{
    public GameObject effect;
    public GameObject aimEffectWand;
    public GameObject aimEffectTarget;

    public List<AudioClip> castSounds;
    public List<AudioClip> aimSounds;
    public List<AudioClip> failSounds;

    private AudioSource ac;

    private SpellManager sm;
    private GameObject cp;

    private GameObject aEW;
    private GameObject aET;

    private bool wasFired = false;

    // Start is called before the first frame update
    void Awake()
    {
        ac = GetComponent<AudioSource>();
        sm = GameObject.Find("SpellManager").GetComponent<SpellManager>();
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
        if (!ac.isPlaying && wasFired)
        {
            Destroy(gameObject);
        }else if (wasFired)
        {
            Destroy(aEW.gameObject);
            Destroy(aET.gameObject);
        }
        else
        {
            aEW.transform.position = cp.transform.position;

            RaycastHit hit;
            if (Physics.Raycast(cp.transform.position, cp.transform.forward, out hit))
            {
                //aET.transform.position += (hit.point + Vector3.up * 0.2f - aET.transform.position) * Time.deltaTime * 25f;
                aET.transform.position += (hit.point - aET.transform.position) * Time.deltaTime * 25f;
            }
            else
            {
                aET.transform.position += (cp.transform.position + cp.transform.forward * 25f - aET.transform.position) * Time.deltaTime * 25f;
            }
        }
    }

    private void Fire()
    {
        //Init
        RaycastHit hit;

        if (Physics.Raycast(cp.transform.position, cp.transform.forward, out hit))
        {
            transform.position = hit.point;
            Instantiate(effect, transform);

            ac.PlayOneShot(castSounds[Random.Range(0, castSounds.Count)]);
        }
        else
        {
            ac.PlayOneShot(failSounds[Random.Range(0, castSounds.Count)]);
            Invoke("Kill", 1.2f);
        }

        wasFired = true;
    }

    private void Kill()
    {
        Destroy(gameObject);
    }

    public override void Execute(CastInfo castInfo)
    {
        cp = castInfo.GetCastPoint().gameObject;

        aEW = Instantiate(aimEffectWand, transform);
        aEW.transform.position = cp.transform.position;
        aEW.transform.localScale = new Vector3(0.3f, 0.3f, 0.3f);

        aET = Instantiate(aimEffectTarget, transform);
        aET.transform.position = Vector3.down * 100;
        aEW.transform.localScale = new Vector3(3f, 3f, 3f);

        ac.PlayOneShot(aimSounds[Random.Range(0, castSounds.Count)]);

        Invoke("Fire", 1.8f);
    }
    public void ResetObj()
    {
        Destroy(gameObject);
    }
}
