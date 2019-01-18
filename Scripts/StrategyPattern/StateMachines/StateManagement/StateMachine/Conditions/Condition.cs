using UnityEngine;

namespace StateMachine
{
    public abstract class Condition : ScriptableObject
    {
		public string Description;

        public abstract bool CheckCondition(StateManager state);

    }
}
