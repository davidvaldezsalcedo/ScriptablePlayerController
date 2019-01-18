using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

namespace StateMachine.BehaviorEditor
{
    [System.Serializable]
    public class BaseNode 
    {
        public int Id;
        public DrawNode DrawNode;
        public Rect WindowRect;
        public string WindowTitle;
        public int EnterNode;
        public int TargetNode;
        public bool IsDuplicate;
        public string Comment;
        public bool IsAssigned;
		public bool ShowDescription;
		public bool IsOnCurrent;

        public bool Collapse;
		public bool ShowActions = true;
		public bool ShowEnterExit = false;
        [HideInInspector]
        public bool PreviousCollapse;

        [SerializeField]
        public StateNodeReferences StateRef;
        [SerializeField]
        public TransitionNodeReferences TransRef;

        public void DrawWindow()
        {
            if(DrawNode != null)
            {
                DrawNode.DrawWindow(this);
            }
        }

        public void DrawCurve()
        {
            if (DrawNode != null)
            {
                DrawNode.DrawCurve(this);
            }
        }

    }

    [System.Serializable]
    public class StateNodeReferences
    { 
    //    [HideInInspector]
        public State CurrentState;
        [HideInInspector]
        public State PreviousState;
		public SerializedObject SerializedState;
	    public ReorderableList OnFixedList;
		public ReorderableList OnUpdateList;
		public ReorderableList OnEnterList;
		public ReorderableList OnExitList;
	}

	[System.Serializable]
    public class TransitionNodeReferences
    {
        [HideInInspector]
        public Condition PreviousCondition;
        public int TransitionId;
    }
}
