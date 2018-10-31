using System;
using SA.Variables;
using UnityEngine;

namespace SA
{
    [CreateAssetMenu(menuName = "SA/Actions/InputManager")]
    public class InputManager : Action
    {
        public InputAxis Horizontal;
        public InputAxis Vertical;

        public RefFloat MoveAmount;

        public override void Execute()
        {
            Horizontal.Execute();
            Vertical.Execute();

            MoveAmount.Value = Mathf.Clamp01(Mathf.Abs(Horizontal.value) + Math.Abs(Vertical.value));
        }
    }
}
