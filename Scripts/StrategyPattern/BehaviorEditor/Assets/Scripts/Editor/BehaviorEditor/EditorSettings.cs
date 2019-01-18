using UnityEngine;

namespace StateMachine.BehaviorEditor
{
    [CreateAssetMenu(menuName ="Editor/Settings")]
    public class EditorSettings : ScriptableObject
    {
        public BehaviorGraph CurrentGraph;
        public StateNode StateNode;
		public PortalNode PortalNode;
        public TransitionNode TransitionNode;
        public CommentNode CommentNode;
        public bool MakeTransition;
        public GUISkin Skin;
		public GUISkin ActiveSkin;
        
        public BaseNode AddNodeOnGraph(DrawNode type, float width,float height, string title, Vector3 pos)
        {
            BaseNode baseNode = new BaseNode();
            baseNode.DrawNode = type;
            baseNode.WindowRect.width = width;
            baseNode.WindowRect.height = height;
            baseNode.WindowTitle = title;
            baseNode.WindowRect.x = pos.x;
            baseNode.WindowRect.y = pos.y;
            CurrentGraph.Windows.Add(baseNode);
            baseNode.TransRef = new TransitionNodeReferences();
            baseNode.StateRef = new StateNodeReferences();
            baseNode.Id = CurrentGraph.IdCount;
            CurrentGraph.IdCount++;
            return baseNode;
        }
    }
}
