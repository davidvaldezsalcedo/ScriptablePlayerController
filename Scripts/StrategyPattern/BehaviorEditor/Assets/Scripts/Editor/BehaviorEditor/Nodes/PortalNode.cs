using UnityEngine;
using UnityEditor;


namespace StateMachine.BehaviorEditor
{
	[CreateAssetMenu(menuName = "Editor/Nodes/Portal Node")]
	public class PortalNode : DrawNode
	{

		public override void DrawCurve(BaseNode b)
		{

		}

		public override void DrawWindow(BaseNode b)
		{
			b.StateRef.CurrentState = (State)EditorGUILayout.ObjectField(b.StateRef.CurrentState, typeof(State), false);
			b.IsAssigned = b.StateRef.CurrentState != null;

			if (b.StateRef.PreviousState != b.StateRef.CurrentState)
			{
				b.StateRef.PreviousState = b.StateRef.CurrentState;
				BehaviorEditor.ForceSetDirty = true;
			}
		}
	}
}
