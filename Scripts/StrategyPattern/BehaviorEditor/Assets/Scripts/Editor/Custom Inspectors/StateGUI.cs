using UnityEngine;
using StateMachine;
using UnityEditor;
using UnityEditorInternal;

namespace StateMachine.CustomUI
{
	[CustomEditor(typeof(State))]
	public class StateGUI : Editor
	{
		private SerializedObject _SerializedState;
		private ReorderableList _OnFixedList;
		private ReorderableList _OnUpdateList;
		private ReorderableList _OnEnterList;
		private ReorderableList _OnExitList;
		private ReorderableList _Transitions;

		private bool _ShowDefaultGUI = false;
		private bool _ShowActions = true;
		private bool _ShowTransitions = true;

		private void OnEnable()
		{
			_SerializedState = null;
		}

		public override void OnInspectorGUI()
		{
			_ShowDefaultGUI = EditorGUILayout.Toggle("DefaultGUI", _ShowDefaultGUI);
			if (_ShowDefaultGUI)
			{
				base.OnInspectorGUI();
				return;
			}

			_ShowActions = EditorGUILayout.Toggle("Show Actions", _ShowActions);

			if(_SerializedState == null)
				SetupReordableLists();

			_SerializedState.Update();

			if (_ShowActions)
			{	
				EditorGUILayout.LabelField("Actions that execute on FixedUpdate()");
				_OnFixedList.DoLayoutList();
				EditorGUILayout.LabelField("Actions that execute on Update()");
				_OnUpdateList.DoLayoutList();
				EditorGUILayout.LabelField("Actions that execute when entering this State");
				_OnEnterList.DoLayoutList();
				EditorGUILayout.LabelField("Actions that execute when exiting this State");
				_OnExitList.DoLayoutList();	
			}

			_ShowTransitions = EditorGUILayout.Toggle("Show Transitions", _ShowTransitions);

			if (_ShowTransitions)
			{
				EditorGUILayout.LabelField("Conditions to exit this State");
				_Transitions.DoLayoutList();
			}

			_SerializedState.ApplyModifiedProperties();
		}

		void SetupReordableLists()
		{
			State curState = (State)target;
			_SerializedState = new SerializedObject(curState);
			_OnFixedList = new ReorderableList(_SerializedState,_SerializedState.FindProperty("OnFixedUpdate"), true, true, true, true);
			_OnUpdateList = new ReorderableList(_SerializedState,_SerializedState.FindProperty("OnUpdate"), true, true, true, true);
			_OnEnterList = new ReorderableList(_SerializedState,_SerializedState.FindProperty("OnStateEnter"), true, true, true, true);
			_OnExitList = new ReorderableList(_SerializedState,_SerializedState.FindProperty("OnStateExit"), true, true, true, true);
			_Transitions = new ReorderableList(_SerializedState, _SerializedState.FindProperty("Transitions"), true, true, true, true);

			HandleReordableList(_OnFixedList, "On Fixed");
			HandleReordableList(_OnUpdateList, "On Update");
			HandleReordableList(_OnEnterList, "On Enter");
			HandleReordableList(_OnExitList, "On Exit");
			HandleTransitionReordable(_Transitions, "Condition --> New State");
		}

		void HandleReordableList(ReorderableList list, string targetName)
		{
			list.drawHeaderCallback = (Rect rect) =>
			{
				EditorGUI.LabelField(rect, targetName);
			};

			list.drawElementCallback = (Rect rect, int index, bool isActive, bool isFocused) =>
			{
				var element = list.serializedProperty.GetArrayElementAtIndex(index);
				EditorGUI.ObjectField(new Rect(rect.x, rect.y, rect.width, EditorGUIUtility.singleLineHeight), element, GUIContent.none);
			};
		}

		void HandleTransitionReordable(ReorderableList list, string targetName)
		{
			list.drawHeaderCallback = (Rect rect) =>
			{
				EditorGUI.LabelField(rect, targetName);
			};

			list.drawElementCallback = (Rect rect, int index, bool isActive, bool isFocused) =>
			{
				var element = list.serializedProperty.GetArrayElementAtIndex(index);
				EditorGUI.ObjectField(new Rect(rect.x, rect.y, rect.width * .3f, EditorGUIUtility.singleLineHeight), element.FindPropertyRelative("Condition"), GUIContent.none);
				EditorGUI.ObjectField(new Rect(rect.x + + (rect.width *.35f), rect.y, rect.width * .3f, EditorGUIUtility.singleLineHeight), element.FindPropertyRelative("TargetState"), GUIContent.none);
				EditorGUI.LabelField(new Rect(rect.x + +(rect.width * .75f), rect.y, rect.width * .2f, EditorGUIUtility.singleLineHeight), "Disable");
				EditorGUI.PropertyField(new Rect(rect.x + +(rect.width * .90f), rect.y, rect.width * .2f, EditorGUIUtility.singleLineHeight), element.FindPropertyRelative("Disable"), GUIContent.none);

			};
		}

	}
}
