using UnityEngine;
using UnityEditor;
using StateMachine;

namespace StateMachine.BehaviorEditor
{
    [CreateAssetMenu(menuName ="Editor/Transition Node")]
    public class TransitionNode : DrawNode
    {
      
        public void Init(StateNode enterState, Transition transition)
        {
      //      this.enterState = enterState;
        }

        public override void DrawWindow(BaseNode b)
        {
            EditorGUILayout.LabelField("");
            BaseNode enterNode = BehaviorEditor.Settings.CurrentGraph.GetNodeWithIndex(b.EnterNode);
			if (enterNode == null)
			{
				return;
			}
			
			if (enterNode.StateRef.CurrentState == null)
			{
				BehaviorEditor.Settings.CurrentGraph.DeleteNode(b.Id);
				return;
			}

            Transition transition = enterNode.StateRef.CurrentState.GetTransition(b.TransRef.TransitionId);

			if (transition == null)
				return;


            transition.Condition = 
                (Condition)EditorGUILayout.ObjectField(transition.Condition
                , typeof(Condition), false);

            if(transition.Condition == null)
            {            
                EditorGUILayout.LabelField("No Condition!");
                b.IsAssigned = false;
            }
            else
            {

                b.IsAssigned = true;
				if (b.IsDuplicate)
				{
					EditorGUILayout.LabelField("Duplicate Condition!");
				}
				else
				{
					GUILayout.Label(transition.Condition.Description);

					BaseNode targetNode = BehaviorEditor.Settings.CurrentGraph.GetNodeWithIndex(b.TargetNode);
					if (targetNode != null)
					{
						if (!targetNode.IsDuplicate)
							transition.TargetState = targetNode.StateRef.CurrentState;
						else
							transition.TargetState = null;
					}
					else
					{
						transition.TargetState = null;
					}
				}
			}
            
            if (b.TransRef.PreviousCondition != transition.Condition)
            {
                b.TransRef.PreviousCondition = transition.Condition;
                b.IsDuplicate = BehaviorEditor.Settings.CurrentGraph.IsTransitionDuplicate(b);
				
				if (!b.IsDuplicate)
                {
					BehaviorEditor.ForceSetDirty = true;
					// BehaviorEditor.settings.currentGraph.SetNode(this);   
				}
            }
            
        }

        public override void DrawCurve(BaseNode b)
        {
            Rect rect = b.WindowRect;
            rect.y += b.WindowRect.height * .5f;
            rect.width = 1;
            rect.height = 1;

            BaseNode e = BehaviorEditor.Settings.CurrentGraph.GetNodeWithIndex(b.EnterNode);
            if (e == null)
            {
                BehaviorEditor.Settings.CurrentGraph.DeleteNode(b.Id);
            }
            else
            {
                Color targetColor = Color.green;
                if (!b.IsAssigned || b.IsDuplicate)
                    targetColor = Color.red;

                Rect r = e.WindowRect;
                BehaviorEditor.DrawNodeCurve(r, rect, true, targetColor);
            }

            if (b.IsDuplicate)
                return;

            if(b.TargetNode > 0)
            {
                BaseNode t = BehaviorEditor.Settings.CurrentGraph.GetNodeWithIndex(b.TargetNode);
                if (t == null)
                {
                    b.TargetNode = -1;
                }
                else
                {
					

                    rect = b.WindowRect;
                    rect.x += rect.width;
                    Rect endRect = t.WindowRect;
                    endRect.x -= endRect.width * .5f;

                    Color targetColor = Color.green;

					if (t.DrawNode is StateNode)
					{
						if (!t.IsAssigned || t.IsDuplicate)
							targetColor = Color.red;
					}
					else
					{
						if (!t.IsAssigned)
							targetColor = Color.red;
						else
							targetColor = Color.yellow;
					}
                    
                    BehaviorEditor.DrawNodeCurve(rect,endRect,false, targetColor);
                }

            }
        }
    }
}
