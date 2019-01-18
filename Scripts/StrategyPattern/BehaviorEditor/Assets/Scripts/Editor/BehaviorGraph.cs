using System.Collections.Generic;
using UnityEngine;


namespace StateMachine.BehaviorEditor
{
    [CreateAssetMenu]
    public class BehaviorGraph : ScriptableObject
    {
	// [SerializeField]
        public List<BaseNode> Windows = new List<BaseNode>();
	// [SerializeField]
        public int IdCount;
        private List<int> _IndexToDelete = new List<int>();

        #region Checkers
        public BaseNode GetNodeWithIndex(int index)
        {
            for (int i = 0; i < Windows.Count; i++)
            {
                if (Windows[i].Id == index)
                    return Windows[i];
            }

            return null;
        }

        public void DeleteWindowsThatNeedTo()
        {
            for (int i = 0; i < _IndexToDelete.Count; i++)
            {
                BaseNode b = GetNodeWithIndex(_IndexToDelete[i]);
                if(b != null)
                    Windows.Remove(b);
            }

            _IndexToDelete.Clear();
        }

        public void DeleteNode(int index)
        {
			if(!_IndexToDelete.Contains(index))
				_IndexToDelete.Add(index);
        }

        public bool IsStateDuplicate(BaseNode b)
        {
            for (int i = 0; i < Windows.Count; i++)
            {
                if (Windows[i].Id == b.Id)
                    continue;

                if (Windows[i].StateRef.CurrentState == b.StateRef.CurrentState &&
                    !Windows[i].IsDuplicate)
                    return true;
            }
             
            return false;
        }

        public bool IsTransitionDuplicate(BaseNode b)
        {
            BaseNode enter = GetNodeWithIndex(b.EnterNode);
            if (enter == null)
            {
                Debug.Log("false");
                return false;
            }
            for (int i = 0; i < enter.StateRef.CurrentState.Transitions.Count; i++)
            {
                Transition t = enter.StateRef.CurrentState.Transitions[i];
                if (t.Condition == b.TransRef.PreviousCondition && b.TransRef.TransitionId != t.Id)
                {
                    return true;
                }
            }

            return false;
        }

        #endregion

    }
}
