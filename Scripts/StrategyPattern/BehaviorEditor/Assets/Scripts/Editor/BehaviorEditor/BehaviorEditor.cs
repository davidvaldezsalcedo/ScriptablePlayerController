using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace StateMachine.BehaviorEditor
{
    public class BehaviorEditor : EditorWindow
    {

        #region Variables
        private Vector3 _MousePosition;
        private bool _ClickedOnWindow;
        private BaseNode _SelectedNode;

        public static EditorSettings Settings;
        private int _TransitFromId;
        private Rect _MouseRect = new Rect(0, 0, 1, 1);
        private Rect _All = new Rect(-5, -5, 10000, 10000);
        private GUIStyle _Style;
		private GUIStyle _ActiveStyle;
		private Vector2 _ScrollPos;
		private Vector2 _ScrollStartPos;
		private static BehaviorEditor _Editor;
		public static StateManager CurrentStateManager;
		public static bool ForceSetDirty;
		private static StateManager _PrevStateManager;
		private static State _PreviousState;
		private int _NodesToDelete;


		public enum UserActions
        {
            AddState,
            AddTransitionNode,
            DeleteNode,
            CommentNode,
            MakeTransition,
            MakePortal,
            ResetPan
        }
        #endregion

        #region Init
        [MenuItem("Behavior Editor/Editor")]
        static void ShowEditor()
        {
            _Editor = EditorWindow.GetWindow<BehaviorEditor>();
            _Editor.minSize = new Vector2(800, 600);
        
        }

        private void OnEnable()
        {
            Settings = Resources.Load("EditorSettings") as EditorSettings;
            _Style = Settings.Skin.GetStyle("window");
			_ActiveStyle = Settings.ActiveSkin.GetStyle("window");

		}
		#endregion

		private void Update()
		{
			if (CurrentStateManager != null)
			{
				if (_PreviousState != CurrentStateManager.CurrentState)
				{
					Repaint();
					_PreviousState = CurrentStateManager.CurrentState;
				}
			}

			if (_NodesToDelete > 0)
			{
				if (Settings.CurrentGraph != null)
				{		
					Settings.CurrentGraph.DeleteWindowsThatNeedTo();
					Repaint();
				}
				_NodesToDelete = 0;
			}
		}

		#region GUI Methods
		private void OnGUI()
        {
			if (Selection.activeTransform != null)
			{
				CurrentStateManager = Selection.activeTransform.GetComponentInChildren<StateManager>();
				if (_PrevStateManager != CurrentStateManager)
				{
					_PrevStateManager = CurrentStateManager;
					Repaint();
				}
			}

		

			Event e = Event.current;
            _MousePosition = e.mousePosition;
            UserInput(e);

            DrawWindows();

			if (e.type == EventType.MouseDrag)
			{
				if (Settings.CurrentGraph != null)
				{
					//settings.currentGraph.DeleteWindowsThatNeedTo();
					Repaint();
				}
			}

			if (GUI.changed)
			{
				Settings.CurrentGraph.DeleteWindowsThatNeedTo();
				Repaint();
			}

            if(Settings.MakeTransition)
            {
                _MouseRect.x = _MousePosition.x;
                _MouseRect.y = _MousePosition.y;
                Rect from = Settings.CurrentGraph.GetNodeWithIndex(_TransitFromId).WindowRect;
                DrawNodeCurve(from, _MouseRect, true, Color.blue);
                Repaint();
            }

			if (ForceSetDirty)
			{
				ForceSetDirty = false;
				EditorUtility.SetDirty(Settings);
				EditorUtility.SetDirty(Settings.CurrentGraph);

				for (int i = 0; i < Settings.CurrentGraph.Windows.Count; i++)
				{
					BaseNode n = Settings.CurrentGraph.Windows[i];
					if(n.StateRef.CurrentState != null)
						EditorUtility.SetDirty(n.StateRef.CurrentState);
			
				}

			}
			
		}

		void DrawWindows()
        {
			GUILayout.BeginArea(_All, _Style);
		
			BeginWindows();
            EditorGUILayout.LabelField(" ", GUILayout.Width(100));
            EditorGUILayout.LabelField("Assign Graph:", GUILayout.Width(100));
            Settings.CurrentGraph = (BehaviorGraph)EditorGUILayout.ObjectField(Settings.CurrentGraph, typeof(BehaviorGraph), false, GUILayout.Width(200));

			if (Settings.CurrentGraph != null)
            {
                foreach (BaseNode n in Settings.CurrentGraph.Windows)
                {
                    n.DrawCurve();
                }

                for (int i = 0; i < Settings.CurrentGraph.Windows.Count; i++)
                {
					BaseNode b = Settings.CurrentGraph.Windows[i];

					if (b.DrawNode is StateNode)
					{
						if (CurrentStateManager != null && b.StateRef.CurrentState == CurrentStateManager.CurrentState)
						{
							b.WindowRect = GUI.Window(i, b.WindowRect,
								DrawNodeWindow, b.WindowTitle,_ActiveStyle);
						}
						else
						{
							b.WindowRect = GUI.Window(i, b.WindowRect,
								DrawNodeWindow, b.WindowTitle);
						}
					}
					else
					{
						b.WindowRect = GUI.Window(i, b.WindowRect,
							DrawNodeWindow, b.WindowTitle);
					}
                }
            }
			EndWindows();

			GUILayout.EndArea();
			

		}

		void DrawNodeWindow(int id)
        {
            Settings.CurrentGraph.Windows[id].DrawWindow();
            GUI.DragWindow();
        }

        void UserInput(Event e)
        {
            if (Settings.CurrentGraph == null)
                return;

            if(e.button == 1 && !Settings.MakeTransition)
            {
                if(e.type == EventType.MouseDown)
                {
                    RightClick(e);
					
                }
            }

            if (e.button == 0 && !Settings.MakeTransition)
            {
                if (e.type == EventType.MouseDown)
                {

                }
            }

            if(e.button == 0 && Settings.MakeTransition)
            {
                if(e.type == EventType.MouseDown)
                {
                    MakeTransition();
                }
            }

			if (e.button == 2)
			{
				if (e.type == EventType.MouseDown)
				{
					_ScrollStartPos = e.mousePosition;
				}
				else if (e.type == EventType.MouseDrag)
				{
					HandlePanning(e);
				}
				else if (e.type == EventType.MouseUp)
				{

				}
			}
        }

		void HandlePanning(Event e)
		{
			Vector2 diff = e.mousePosition - _ScrollStartPos;
			diff *= .6f;
			_ScrollStartPos = e.mousePosition;
			_ScrollPos += diff;

			for (int i = 0; i < Settings.CurrentGraph.Windows.Count; i++)
			{
				BaseNode b = Settings.CurrentGraph.Windows[i];
				b.WindowRect.x += diff.x;
				b.WindowRect.y += diff.y;
			}
		}

		void ResetScroll()
		{
			for (int i = 0; i < Settings.CurrentGraph.Windows.Count; i++)
			{
				BaseNode b = Settings.CurrentGraph.Windows[i];
				b.WindowRect.x -= _ScrollPos.x;
				b.WindowRect.y -= _ScrollPos.y;
			}

			_ScrollPos = Vector2.zero;
		}

        void RightClick(Event e)
        {
            _ClickedOnWindow = false;
            for (int i = 0; i < Settings.CurrentGraph.Windows.Count; i++)
            {
                if (Settings.CurrentGraph.Windows[i].WindowRect.Contains(e.mousePosition))
                {
                    _ClickedOnWindow = true;
                    _SelectedNode = Settings.CurrentGraph.Windows[i];
                    break;
                }
            }

            if(!_ClickedOnWindow)
            {
                AddNewNode(e);
            }
            else
            {
                ModifyNode(e);
            }
        }
       
        void MakeTransition()
        {
            Settings.MakeTransition = false;
            _ClickedOnWindow = false;
            for (int i = 0; i < Settings.CurrentGraph.Windows.Count; i++)
            {
                if (Settings.CurrentGraph.Windows[i].WindowRect.Contains(_MousePosition))
                {
                    _ClickedOnWindow = true;
                    _SelectedNode = Settings.CurrentGraph.Windows[i];
                    break;
                }
            }

            if(_ClickedOnWindow)
            {
                if(_SelectedNode.DrawNode is StateNode || _SelectedNode.DrawNode is PortalNode)
                {
                    if(_SelectedNode.Id != _TransitFromId)
                    {
                        BaseNode transNode = Settings.CurrentGraph.GetNodeWithIndex(_TransitFromId);
                        transNode.TargetNode = _SelectedNode.Id;

                        BaseNode enterNode = BehaviorEditor.Settings.CurrentGraph.GetNodeWithIndex(transNode.EnterNode);
                        Transition transition = enterNode.StateRef.CurrentState.GetTransition(transNode.TransRef.TransitionId);

						transition.TargetState = _SelectedNode.StateRef.CurrentState;
                    }
                }
            }
        }
        #endregion

        #region Context Menus
        void AddNewNode(Event e)
        {
            GenericMenu menu = new GenericMenu();
            menu.AddSeparator("");
            if (Settings.CurrentGraph != null)
            {
                menu.AddItem(new GUIContent("Add State"), false, ContextCallback, UserActions.AddState);
				menu.AddItem(new GUIContent("Add Portal"), false, ContextCallback, UserActions.MakePortal);
				menu.AddItem(new GUIContent("Add Comment"), false, ContextCallback, UserActions.CommentNode);
				menu.AddSeparator("");
				menu.AddItem(new GUIContent("Reset Panning"), false, ContextCallback, UserActions.ResetPan);
			}
            else
            {
                menu.AddDisabledItem(new GUIContent("Add State"));
                menu.AddDisabledItem(new GUIContent("Add Comment"));
            }
            menu.ShowAsContext();
            e.Use();
        }

        void ModifyNode(Event e)
        {
            GenericMenu menu = new GenericMenu();
            if (_SelectedNode.DrawNode is StateNode)
            {
                if (_SelectedNode.StateRef.CurrentState != null)
                {
                    menu.AddSeparator("");
                    menu.AddItem(new GUIContent("Add Condition"), false, ContextCallback, UserActions.AddTransitionNode);
                }
                else
                {
                    menu.AddSeparator("");
                    menu.AddDisabledItem(new GUIContent("Add Condition"));
                }
                menu.AddSeparator("");
                menu.AddItem(new GUIContent("Delete"), false, ContextCallback, UserActions.DeleteNode);
            }

			if (_SelectedNode.DrawNode is PortalNode)
			{
				menu.AddSeparator("");
				menu.AddItem(new GUIContent("Delete"), false, ContextCallback, UserActions.DeleteNode);
			}

			if (_SelectedNode.DrawNode is TransitionNode)
            {
                if (_SelectedNode.IsDuplicate || !_SelectedNode.IsAssigned)
                {
                    menu.AddSeparator("");
                    menu.AddDisabledItem(new GUIContent("Make Transition"));
                }
                else
                {
                    menu.AddSeparator("");
                    menu.AddItem(new GUIContent("Make Transition"), false, ContextCallback, UserActions.MakeTransition);
                }
                menu.AddSeparator("");
                menu.AddItem(new GUIContent("Delete"), false, ContextCallback, UserActions.DeleteNode);
            }

            if (_SelectedNode.DrawNode is CommentNode)
            {
                menu.AddSeparator("");
                menu.AddItem(new GUIContent("Delete"), false, ContextCallback, UserActions.DeleteNode);
            }
            menu.ShowAsContext();
            e.Use();
        }
        
        void ContextCallback(object o)
        {
            UserActions a = (UserActions)o;
            switch (a)
            {
                case UserActions.AddState:
                    Settings.AddNodeOnGraph(Settings.StateNode, 200, 100, "State", _MousePosition);                
                    break;
				case UserActions.MakePortal:
					Settings.AddNodeOnGraph(Settings.PortalNode, 100, 80, "Portal", _MousePosition);
					break;
                case UserActions.AddTransitionNode:
					AddTransitionNode(_SelectedNode, _MousePosition);

					break;           
                case UserActions.CommentNode:
                    BaseNode commentNode = Settings.AddNodeOnGraph(Settings.CommentNode, 200, 100, "Comment", _MousePosition);
                    commentNode.Comment = "This is a comment";           
                    break;
                default:
                    break;
                case UserActions.DeleteNode:
					if (_SelectedNode.DrawNode is TransitionNode)
					{
						BaseNode enterNode = Settings.CurrentGraph.GetNodeWithIndex(_SelectedNode.EnterNode);
						if (enterNode != null)
							enterNode.StateRef.CurrentState.RemoveTransition(_SelectedNode.TransRef.TransitionId);
					}

					_NodesToDelete++;
                    Settings.CurrentGraph.DeleteNode(_SelectedNode.Id);
                    break;
                case UserActions.MakeTransition:
                    _TransitFromId = _SelectedNode.Id;
                    Settings.MakeTransition = true;
                    break;
				case UserActions.ResetPan:
					ResetScroll();
					break;
            }

			ForceSetDirty = true;
        
		}

		public static BaseNode AddTransitionNode(BaseNode enterNode, Vector3 pos)
		{
			BaseNode transNode = Settings.AddNodeOnGraph(Settings.TransitionNode, 200, 100, "Condition", pos);
			transNode.EnterNode = enterNode.Id;
			Transition t = Settings.StateNode.AddTransition(enterNode);
			transNode.TransRef.TransitionId = t.Id;
			return transNode;
		}

		public static BaseNode AddTransitionNodeFromTransition(Transition transition, BaseNode enterNode, Vector3 pos)
		{
			BaseNode transNode = Settings.AddNodeOnGraph(Settings.TransitionNode, 200, 100, "Condition", pos);
			transNode.EnterNode = enterNode.Id;
			transNode.TransRef.TransitionId = transition.Id;
			return transNode;

		}

		#endregion

		#region Helper Methods
		public static void DrawNodeCurve(Rect start, Rect end, bool left, Color curveColor)
        {
            Vector3 startPos = new Vector3(
                (left) ? start.x + start.width : start.x,
                start.y + (start.height *.5f),
                0);

            Vector3 endPos = new Vector3(end.x + (end.width * .5f), end.y + (end.height * .5f), 0);
            Vector3 startTan = startPos + Vector3.right * 50;
            Vector3 endTan = endPos + Vector3.left * 50;

            Color shadow = new Color(0, 0, 0, 1);
            for (int i = 0; i < 1; i++)
            {
                Handles.DrawBezier(startPos, endPos, startTan, endTan, shadow, null, 4);
            }

            Handles.DrawBezier(startPos, endPos, startTan, endTan, curveColor, null, 3);
        }

        public static void ClearWindowsFromList(List<BaseNode>l)
        {
            for (int i = 0; i < l.Count; i++)
            {
          //      if (windows.Contains(l[i]))
            //        windows.Remove(l[i]);
            }
        }
        
        #endregion

    }
}
