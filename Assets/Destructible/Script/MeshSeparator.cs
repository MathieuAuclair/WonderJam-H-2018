using MeshSplitting.Splitters;
using UnityEngine;

public class MeshSeparator : MonoBehaviour
{
	public float requiredMagnitudeToBreak = 8.5f;

	void OnCollisionEnter (Collision other)
	{

		if (other.relativeVelocity.magnitude > requiredMagnitudeToBreak) {
			IdentifyPropGameObject (other.gameObject);
		}
	}

	static void IdentifyPropGameObject (GameObject other)
	{
		if (other.gameObject.CompareTag ("prop") || other.gameObject.CompareTag ("broken-prop")) {
			CreateSlicerBasedOnGameObject (other.gameObject);
		}
	}

	void wakeSleepingRigidBody (GameObject prop)
	{
		Rigidbody propRb = prop.GetComponent<Rigidbody> ();

		if (propRb == null) {
			propRb.WakeUp ();
		}
	}

	static void CreateSlicerBasedOnGameObject (GameObject prop)
	{
		CreateBladeWithRotation (prop, Quaternion.Euler (90, 0, 0));
		CreateBladeWithRotation (prop, Quaternion.Euler (0, 90, 0));
		CreateBladeWithRotation (prop, Quaternion.Euler (0, 0, 90));
	}

	static void CreateBladeWithRotation (GameObject prop, Quaternion rotation)
	{
		GameObject blade = GameObject.CreatePrimitive (PrimitiveType.Plane);
		blade.GetComponent<Renderer> ().enabled = true;
		blade.transform.SetPositionAndRotation (GetPropDimensions (prop), rotation);
		blade.transform.position = prop.transform.position;
		blade.AddComponent (typeof(SplitterSingleCut));
		blade.AddComponent (typeof(MeshCollider));
		blade.GetComponent<MeshCollider> ().convex = true;
		blade.GetComponent<MeshCollider> ().isTrigger = true;
	}

	static Vector3 GetPropDimensions (GameObject prop)
	{
		return prop.GetComponent<Renderer> ().bounds.size / 10;
	}
}