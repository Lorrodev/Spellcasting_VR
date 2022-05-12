using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "newSpellObject", menuName = "New Spell Object")]
public class SpellObject : ScriptableObject
{
    public string spellName;
    public List<GameObject> runes;
    public GameObject spellScriptObject;
}
