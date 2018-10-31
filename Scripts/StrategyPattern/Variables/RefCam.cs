using UnityEngine;

namespace SA.Variables
{
	[CreateAssetMenu(menuName = "SA/Variables/Camera")]
	public class RefCam : ScriptableObject 
	{
		public Camera Value;
	}
}
