using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "newSpellObject", menuName = "Spell Object (RuneMagic)")]
public class SpellContainer : ScriptableObject
{
    [SerializeField]
    private string spellName;
    [SerializeField]
    private List<Rune> runes;
    [SerializeField]
    private Spell spell;

    public List<Rune> GetRunes()
    {
        return new List<Rune>(runes);
    }

    public Spell GetSpell()
    {
        return spell;
    }
}
