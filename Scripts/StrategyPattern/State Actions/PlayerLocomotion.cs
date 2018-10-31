using UnityEngine;

namespace SA
{
    [CreateAssetMenu(menuName = "SA/StateActions/PlayerLocomotion")]
    public class PlayerLocomotion : StateAction
    {
        [SerializeField]
        private InputManager InpManager;
        [SerializeField]
        private float movementSpeed = 2;

        public override void Execute(StateManager states)
        {
            Vector3 velocity = new Vector3(InpManager.Horizontal.value, 0, InpManager.Vertical.value) * movementSpeed;
            velocity.y = states.Rb.velocity.y;
            states.Rb.velocity = velocity;
        }
    }
}
