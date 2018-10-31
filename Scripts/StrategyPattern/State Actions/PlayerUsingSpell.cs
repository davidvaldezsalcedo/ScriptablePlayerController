using UnityEngine;
using SA;
using SA.Variables;

[CreateAssetMenu(menuName = "SA/StateActions/PlayerCastingSpell")]
public class PlayerUsingSpell : StateAction
{
    [SerializeField]
    private RefGameObject _SpellToUse;
    [SerializeField]
    private RefTransform _PlayerTransform;
    [SerializeField]
    private RefSpell _SpellCast;
    [SerializeField]
    private RefBool _SpellDoneExecuting;

    public override void Execute(StateManager states)
    {
        _SpellDoneExecuting.Value = false;

        _SpellCast.Value.CastSpell(_SpellToUse.Value, _PlayerTransform.Value);
        
        _SpellDoneExecuting.Value = true;
    }
}
