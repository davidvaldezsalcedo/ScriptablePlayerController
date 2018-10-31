using UnityEngine;

namespace SA
{
    [CreateAssetMenu(menuName = "SA/Actions/Inputs/Axis")]
    public class InputAxis : Action
    {
        public string targetString;
        public float value;

        public override void Execute()
        {
            value = Input.GetAxis(targetString);
        }
    }
}
