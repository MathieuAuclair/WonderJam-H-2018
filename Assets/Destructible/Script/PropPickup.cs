using UnityEngine;

public class PropPickup : MonoBehaviour
{
	public float PropPickupDistance = 5.0f;

	void FixedUpdate ()
	{
		RaycastHit hit;
		//Vector3 forward = transform.TransformDirection(Vector3.forward) * 10;

		if (Physics.Raycast (transform.position, Vector3.down, out hit, PropPickupDistance)) {

		}
	}

	void OnDrawGizmos ()
	{
		Debug.DrawRay (Camera.main.transform.position, Camera.main.transform.forward * PropPickupDistance, Color.green, 0.5f);
	}
}
