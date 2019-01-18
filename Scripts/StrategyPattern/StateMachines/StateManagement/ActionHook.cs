using UnityEngine;

namespace StateMachine
{
    public class ActionHook : MonoBehaviour
    {
		public Action[] FixedUpdateActions;
		public Action[] UpdateActions;

		void FixedUpdate()
		{
			if (FixedUpdateActions == null)
				return;

			for (int i = 0; i < FixedUpdateActions.Length; i++)
			{
				FixedUpdateActions[i].Execute();
			}
		}

		void Update()
        {
			if (UpdateActions == null)
				return;

            for (int i = 0; i < UpdateActions.Length; i++)
            {
                UpdateActions[i].Execute();
            }
        }
    }
}
