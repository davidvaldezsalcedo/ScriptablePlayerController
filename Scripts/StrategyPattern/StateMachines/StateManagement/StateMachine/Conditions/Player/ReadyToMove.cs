using UnityEngine;
using StateMachine;

[CreateAssetMenu(menuName = "StateMachine/Conditions/Player/ReadyToMove", fileName = "ReadyToMove")]
public class ReadyToMove : Condition
{
    public override bool CheckCondition(StateManager state)
    {
		float i = state.Delta;
		var owner = state.GetComponent<PlayerController>();
        return owner.ReadyToMove;
    }
}
