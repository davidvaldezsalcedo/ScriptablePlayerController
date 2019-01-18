using UnityEngine;
using StateMachine;

[CreateAssetMenu(menuName = "StateMachine/Conditions/Player/ArrivedToDestination", fileName = "ArrivedToDestination")]
public class ArrivedToDestination : Condition
{
    public override bool CheckCondition(StateManager state)
    {
        return false;
    }
}
