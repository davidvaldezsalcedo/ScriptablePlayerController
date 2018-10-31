using UnityEngine;

namespace SA
{
	[System.Serializable]
	public class Transition
	{
		public Condition Condition;
		public State TargetState;
		public bool CheckIf = true;
		public bool Disable;
	}
}
