using UnityEngine;
using System.Collections.Generic;
using SA;
using SA.Variables;

[CreateAssetMenu(menuName = "SA/Conditions/UseSpell")]
public class UseSpell : Condition
{
    [SerializeField]
    private RefGameObject _SpellToUse;
    [SerializeField]
    private RefSpell _SpellTypeToCast;
    [SerializeField]
    private CustomSpells[] _MappedSpells;

    private Dictionary<KeyCode, CustomSpells> _SpellsToUseDictionary = new Dictionary<KeyCode, CustomSpells>();

    private void OnEnable()
    {
        for (int i = 0; i < _MappedSpells.Length; ++i)
        {
            _SpellsToUseDictionary.Add(_MappedSpells[i].KeyToPress, _MappedSpells[i]);   
        }
    }

    public override bool CheckCondition(StateManager state)
    {
        for (int i = 0; i < _MappedSpells.Length; ++i)
        {
            if(Input.GetKeyDown(_MappedSpells[i].KeyToPress))
            {
                _SpellTypeToCast.Value = _SpellsToUseDictionary[_MappedSpells[i].KeyToPress].SpellCastType;
                _SpellToUse.Value = _SpellsToUseDictionary[_MappedSpells[i].KeyToPress].SpellObject;
                return true;
            }
        }
        return false;
    }

    [System.Serializable]
    public class CustomSpells
    {
        public KeyCode KeyToPress;
        public GameObject SpellObject;
        public SpellBase SpellCastType;
    }
}
