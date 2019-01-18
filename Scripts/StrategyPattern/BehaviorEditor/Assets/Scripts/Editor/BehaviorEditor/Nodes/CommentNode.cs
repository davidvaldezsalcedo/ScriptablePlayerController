using System;
using UnityEngine;

namespace StateMachine.BehaviorEditor
{
    [CreateAssetMenu(menuName = "Editor/Comment Node")]
    public class CommentNode :DrawNode
    {
        
        public override void DrawWindow(BaseNode b)
        {
            b.Comment = GUILayout.TextArea(b.Comment, 200);
        }

        public override void DrawCurve(BaseNode b)
        {
        }
    }
}
