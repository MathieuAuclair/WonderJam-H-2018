using UnityEngine;

public class EscapeToExit : MonoBehaviour
{
	void Update ()
	{
		if (Input.GetKey ("escape"))
			Application.Quit ();

	}
}
