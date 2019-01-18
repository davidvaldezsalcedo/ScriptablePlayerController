using UnityEngine;

namespace StateMachine
{
    [CreateAssetMenu(menuName = "Inputs/Axis")]
    public class InputAxis : Action
    {
        public string TargetString;
        public float Value;

        public override void Execute()
        {
            Value = Input.GetAxis(TargetString);
        }
    }
}
