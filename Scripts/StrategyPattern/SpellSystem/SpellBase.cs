using UnityEngine;

public abstract class SpellBase : ScriptableObject 
{
	public abstract void CastSpell(GameObject spellToUse, Transform playerTransform);
}
