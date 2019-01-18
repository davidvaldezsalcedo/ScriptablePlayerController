using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace StateMachine
{
    [CreateAssetMenu(menuName = "StateMachine/States/State", fileName = "State_")]
    public class State : ScriptableObject
    {
     	public StateActions[] OnFixedUpdate;
        public StateActions[] OnUpdate;
        public StateActions[] OnStateEnter;
        public StateActions[] OnStateExit;

        public int IdCount;
		[SerializeField]
        public List<Transition> Transitions = new List<Transition>();

        public void OnEnter(StateManager states)
        {
            ExecuteActions(states, OnStateEnter);
        }
	
		public void FixedTick(StateManager states)
		{
			ExecuteActions(states,OnFixedUpdate);
		}

        public void Tick(StateManager states)
        {
            ExecuteActions(states, OnUpdate);
            CheckTransitions(states);
        }

        public void OnExit(StateManager states)
        {
            ExecuteActions(states, OnStateExit);
        }

        public void CheckTransitions(StateManager states)
        {
            for (int i = 0; i < Transitions.Count; i++)
            {
                if (Transitions[i].Disable)
                    continue;

                if(Transitions[i].Condition.CheckCondition(states))
                {
                    if (Transitions[i].TargetState != null)
                    {
                        states.CurrentState = Transitions[i].TargetState;
                        OnExit(states);
                        states.CurrentState.OnEnter(states);
                    }
                    return;
                }
            }
        }
        
        public void ExecuteActions(StateManager states, StateActions[] l)
        {
            for (int i = 0; i < l.Length; i++)
            {
                if (l[i] != null)
                    l[i].Execute(states);
            }
        }

        public Transition AddTransition()
        {
            Transition retVal = new Transition();
            Transitions.Add(retVal);
            retVal.Id = IdCount;
            IdCount++;
            return retVal;
        }

        public Transition GetTransition(int id)
        {
            for (int i = 0; i < Transitions.Count; i++)
            {
                if (Transitions[i].Id == id)
                    return Transitions[i];
            }

            return null;
        }

		public void RemoveTransition(int id)
		{
			Transition t = GetTransition(id);
			if (t != null)
				Transitions.Remove(t);
		}
    }
}
