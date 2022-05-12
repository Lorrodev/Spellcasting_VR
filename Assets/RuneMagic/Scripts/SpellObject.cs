using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "newSpellObject", menuName = "Spell Object (RuneMagic)")]
public class SpellObject : ScriptableObject
{
    [SerializeField]
    private string spellName;
    [SerializeField]
    private List<GameObject> runes;
    [SerializeField]
    private GameObject spellScriptObject;

    public List<GameObject> GetRunes()
    {
        return new List<GameObject>(runes);
    }

    public GameObject GetSpellScriptObject()
    {
        return spellScriptObject;
    }
}
