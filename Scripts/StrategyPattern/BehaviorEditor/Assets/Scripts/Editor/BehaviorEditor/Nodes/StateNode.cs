using UnityEngine;
using UnityEditor;
using UnityEditorInternal;
using StateMachine;
using System;
using System.IO;

namespace StateMachine.BehaviorEditor
{
    [CreateAssetMenu(menuName = "Editor/Nodes/State Node")]
    public class StateNode : DrawNode
    {
        public override void DrawWindow(BaseNode b)
        {
            if(b.StateRef.CurrentState == null)
            {
                EditorGUILayout.LabelField("Add state to modify:");
            }
            else
            {
                if(!b.Collapse)
                {
                 
                }
                else
                {
                    b.WindowRect.height = 100;
                }

                b.Collapse = EditorGUILayout.Toggle(" ", b.Collapse);
            }

            b.StateRef.CurrentState = (State)EditorGUILayout.ObjectField(b.StateRef.CurrentState, typeof(State), false);

            if(b.PreviousCollapse != b.Collapse)
            {
                b.PreviousCollapse = b.Collapse;
            }

            if(b.StateRef.PreviousState != b.StateRef.CurrentState)
            {
                //b.serializedState = null;
                b.IsDuplicate = BehaviorEditor.Settings.CurrentGraph.IsStateDuplicate(b);
				b.StateRef.PreviousState = b.StateRef.CurrentState;

				if (!b.IsDuplicate)
				{
					Vector3 pos = new Vector3(b.WindowRect.x,b.WindowRect.y,0);
					pos.x += b.WindowRect.width * 2;

					SetupReordableLists(b);

					//Load transtions
					for (int i = 0; i < b.StateRef.CurrentState.Transitions.Count; i++)
					{
						pos.y += i * 100;
						BehaviorEditor.AddTransitionNodeFromTransition(b.StateRef.CurrentState.Transitions[i], b, pos);
					}

					BehaviorEditor.ForceSetDirty = true;
				}
				
			}

			if (b.IsDuplicate)
            {
                EditorGUILayout.LabelField("State is a duplicate!");
                b.WindowRect.height = 100;
				return;
            }

            if (b.StateRef.CurrentState != null)
            {
                b.IsAssigned = true;
                
                if (!b.Collapse)
                {
					if (b.StateRef.SerializedState == null)
					{
						SetupReordableLists(b);

					//	SerializedObject serializedState = new SerializedObject(b.stateRef.currentState);
					}

					float standard = 150;
					b.StateRef.SerializedState.Update();
					b.ShowActions = EditorGUILayout.Toggle("Show Actions ", b.ShowActions);
					if (b.ShowActions)
					{
						EditorGUILayout.LabelField("");
						b.StateRef.OnFixedList.DoLayoutList();
						EditorGUILayout.LabelField("");
						b.StateRef.OnUpdateList.DoLayoutList();
						standard += 100 + 40 + (b.StateRef.OnUpdateList.count + b.StateRef.OnFixedList.count) * 20;
					}
					b.ShowEnterExit = EditorGUILayout.Toggle("Show Enter/Exit ", b.ShowEnterExit);
					if (b.ShowEnterExit)
					{
						EditorGUILayout.LabelField("");
						b.StateRef.OnEnterList.DoLayoutList();
						EditorGUILayout.LabelField("");
						b.StateRef.OnExitList.DoLayoutList();
						standard += 100 + 40 + (b.StateRef.OnEnterList.count + b.StateRef.OnExitList.count) * 20;
					}

					b.StateRef.SerializedState.ApplyModifiedProperties();
                    b.WindowRect.height = standard;
                }   
            }
            else
            {
                b.IsAssigned = false;
            }
		}

		void SetupReordableLists(BaseNode b)
		{
			b.StateRef.SerializedState = new SerializedObject(b.StateRef.CurrentState);
			b.StateRef.OnFixedList = new ReorderableList(b.StateRef.SerializedState, b.StateRef.SerializedState.FindProperty("OnFixedUpdate"), true, true, true, true);
			b.StateRef.OnUpdateList = new ReorderableList(b.StateRef.SerializedState, b.StateRef.SerializedState.FindProperty("OnUpdate"), true, true, true, true);
			b.StateRef.OnEnterList = new ReorderableList(b.StateRef.SerializedState, b.StateRef.SerializedState.FindProperty("OnStateEnter"), true, true, true, true);
			b.StateRef.OnExitList = new ReorderableList(b.StateRef.SerializedState, b.StateRef.SerializedState.FindProperty("OnStateExit"), true, true, true, true);

			HandleReordableList(b.StateRef.OnFixedList, "On Fixed");
			HandleReordableList(b.StateRef.OnUpdateList, "On Update");
			HandleReordableList(b.StateRef.OnEnterList, "On Enter");
			HandleReordableList(b.StateRef.OnExitList, "On Exit");
		}

        void HandleReordableList(ReorderableList list, string targetName)
        {
            list.drawHeaderCallback = (Rect rect) =>
            {
                EditorGUI.LabelField(rect, targetName);
            };

            list.drawElementCallback = (Rect rect, int index,bool isActive, bool isFocused) =>
             {
                 var element = list.serializedProperty.GetArrayElementAtIndex(index);
                 EditorGUI.ObjectField(new Rect(rect.x, rect.y, rect.width, EditorGUIUtility.singleLineHeight), element, GUIContent.none);
             };
        }

        public override void DrawCurve(BaseNode b)
        {

        }

        public Transition AddTransition(BaseNode b)
        {
            return b.StateRef.CurrentState.AddTransition();
        }

        public void ClearReferences()
        {
      //      BehaviorEditor.ClearWindowsFromList(dependencies);
        //    dependencies.Clear();
        }

    }
}
