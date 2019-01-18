using UnityEngine;

namespace StateMachine
{
	[CreateAssetMenu(menuName = "Inputs/Button")]
	public class InputButton : Action
	{
		public string TargetInput;
		public bool IsPressed;
		public KeyState KeyPressedState;
		public bool UpdateBoolVar = true;
		//You need to import the SO library from my github to use a BoolVariable asset
	//	public SO.BoolVariable targetBoolVariable;


		public override void Execute()
		{
			switch (KeyPressedState)
			{
				case KeyState.onDown:
					IsPressed = Input.GetButtonDown(TargetInput);
					break;
				case KeyState.onCurrent:
					IsPressed = Input.GetButton(TargetInput);
					break;
				case KeyState.onUp:
					IsPressed = Input.GetButtonUp(TargetInput);
					break;
				default:
					break;
			}

			if (UpdateBoolVar)
			{
				//if (targetBoolVariable != null)
				//{
				//	targetBoolVariable.value = isPressed;
				//}
			}
		}

		public enum KeyState
		{
			onDown,onCurrent,onUp
		}
	}
}
