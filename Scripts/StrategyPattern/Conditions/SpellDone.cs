using UnityEngine;
using SA;
using SA.Variables;

[CreateAssetMenu(menuName = "SA/Conditions/SpellDone")]
public class SpellDone : Condition
{
	[SerializeField]
	private RefBool _SpellDone;

    public override bool CheckCondition(StateManager state)
    {
        return _SpellDone.Value;
    }
}
