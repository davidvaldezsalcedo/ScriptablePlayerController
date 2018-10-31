using System.Collections.Generic;
using UnityEngine;

namespace SA
{
    [CreateAssetMenu(menuName = "SA/States/State")]
    public class State : ScriptableObject
    {
        public List<StateAction> OnStateActions = new List<StateAction>();
        public List<StateAction> OnEnterAction = new List<StateAction>();
        public List<StateAction> OnExitAction = new List<StateAction>();
        
        public List<Transition> Transitions = new List<Transition>();

        #region LifeCycle
            public void OnEnter(StateManager states)
            {   
                ExecuteActions(states, OnEnterAction);
            }

            public void OnExit(StateManager states)
            {
                ExecuteActions(states, OnExitAction);
            }

            public void Tick(StateManager states)
            {
                ExecuteActions(states, OnStateActions);
                CheckTransitions(states);
            }
        #endregion LifeCycle

        #region Logic
            private void ExecuteActions(StateManager states, List<StateAction> actionList)
            {
                for (int i = actionList.Count - 1; i >= 0; --i)
                {
                    if(actionList[i] != null)
                    {
                        actionList[i].Execute(states);
                    }
                }
            }

            private void CheckTransitions(StateManager states)
            {
                for (int i = 0; i < Transitions.Count; ++i)
                {
                    if(Transitions[i].Disable)
                    {
                        continue;
                    }

                    if(Transitions[i].Condition.CheckCondition(states) == Transitions[i].CheckIf)
                    {
                        if(Transitions[i].TargetState != null)
                        {
                            states.CurrentState = Transitions[i].TargetState;
                            OnExit(states);
                            states.CurrentState.OnEnter(states);
                        }

                        return;
                    }
                }
            }

            public Transition AddTransition()
            {
                Transition returnVal = new Transition();
                Transitions.Add(returnVal);
                return returnVal;
            }
        #endregion Logic
    }
}
