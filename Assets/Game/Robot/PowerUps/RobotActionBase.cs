using UnityEngine;

namespace AssemblyCSharp
{
	public abstract class RobotActionBase : MonoBehaviour
	{
		public abstract void ActionBegins ();

		public abstract void ActionEnds ();
	}
}
