using UnityEngine;

namespace SA
{
    [CreateAssetMenu(menuName = "SA/StateActions/RotateBasedOnPointer")]
    public class RotateBasedOnPointer : StateAction
    {
        [SerializeField]
        private InputManager InpManager;
        [SerializeField]
        public float Speed = 3;

        public Vector3 MousePosition;

        public override void Execute(StateManager states)
        {
            MousePosition.y = states.transform.position.y;
            if (MousePosition == Vector3.zero)
                MousePosition = states.transform.forward;

            Quaternion posToLookAt = Quaternion.LookRotation(MousePosition - states.transform.position, Vector3.up);
            Quaternion targetRotation = Quaternion.Slerp(states.transform.rotation, posToLookAt, states.Delta * InpManager.MoveAmount.Value * Speed);
          
            states.Rb.MoveRotation(targetRotation);
        }
    }
}
