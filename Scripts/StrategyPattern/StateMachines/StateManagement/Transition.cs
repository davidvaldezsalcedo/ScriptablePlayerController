using UnityEngine;

namespace StateMachine
{
    [System.Serializable]
    public class Transition
    {
        public int Id;
        public Condition Condition;
        public State TargetState;
        public bool Disable;
    }
}
