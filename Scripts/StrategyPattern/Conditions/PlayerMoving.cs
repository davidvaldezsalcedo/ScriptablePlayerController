using SA;
using SA.Variables;
using UnityEngine;

[CreateAssetMenu(menuName = "SA/Conditions/PlayerMoving")]
public class PlayerMoving : Condition
{
	[SerializeField]
    private RefFloat _PlayerMoveAmount;

    public override bool CheckCondition(StateManager state)
    {
        if(_PlayerMoveAmount.Value != 0)
        {
            return true;
        }
        
        return false;
    }
}
