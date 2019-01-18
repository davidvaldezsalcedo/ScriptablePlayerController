using UnityEngine;
using StateMachine;

[CreateAssetMenu(menuName = "StateMachine/StateActions/Player/PlayerMove", fileName = "PlayerMove")]
public class PlayerMove : StateActions
{
    public override void Execute(StateManager states)
    {
        states.Rb.AddForce(states.transform.forward * 50f);
    }
}
