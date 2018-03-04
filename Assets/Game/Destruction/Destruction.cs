using UnityEngine;

public class Destruction : MonoBehaviour
{
	[Space (7)]
	[Header ("State")]
	[Space (2)]
	[Tooltip ("Whether the object starts broken")]
	public bool startBroken;

	[Space (7)]
	[Header ("General Settings")]
	[Space (2)]
	[Tooltip ("The higher this is, the more will break off in an impact")]
	public float breakageMultiplier = 0.3f;
	[Tooltip ("How resistant the object initially is to breakage.")]
	public float strength = 0.3f;

	[Space (7)]
	[Header ("Breaking on Collision")]
	[Space (2)]
	[Tooltip ("Whether the object breaks when it collides with something")]
	public bool breakOnCollision = true;
	[Tooltip ("The minimum relative velocity to break the object")]
	public float velocityToBreak = 1;

	[Space (7)]
	[Header ("Breaking when nothing underneath")]
	[Space (2)]
	[Tooltip ("Whether the object breaks when there's nothing underneath supporting it")]
	public bool breakOnNoSupports = false;
	[Tooltip ("The length of the raycast used to check for supports underneath")]
	public float raycastLength = 1f;

	[Space (7)]
	[Header ("Sound On Break")]
	[Space (2)]
	[Tooltip ("Whether the object makes a sound when it breaks")]
	public bool soundOnBreak = false;
	[Tooltip ("An array of sounds for the object to make when it breaks (A random one will be selected)")]
	public AudioClip[] clips;

	[Space (7)]
	[Header ("Particles On Break")]
	[Space (2)]
	[Tooltip ("Whether the object makes particles when it breaks")]
	public bool particlesOnBreak = false;

	[SerializeField] const string TAG_ALLOWED_TO_EXPLODE_THINGS = "Player";

	AudioSource src;
	ParticleSystem particles;

	Collider coll;
	Rigidbody[] rigids;
	MeshRenderer[] renderers;

	void Start ()
	{
		renderers = gameObject.GetComponentsInChildren<MeshRenderer> ();
		rigids = gameObject.GetComponentsInChildren<Rigidbody> ();
		SetRenderersEnabled (false);
		coll = GetComponent<Collider> ();
		SetPiecesKinematic (!startBroken);

		if (soundOnBreak) {
			SetupSound ();
		}
		if (particlesOnBreak) {
			SetupParticles ();
		}
	}

	void SetRenderersEnabled (bool value)
	{
		for (int i = 1; i < renderers.Length; i++) {
			renderers [i].enabled = value;
		}
	}

	void SetupSound ()
	{
		src = GetComponent<AudioSource> ();
		if (src == null) {
			src = gameObject.AddComponent<AudioSource> ();
		}
		src.clip = clips [Random.Range (0, clips.Length - 1)];
	}

	void SetupParticles ()
	{
		particles = GetComponent<ParticleSystem> ();
		if (particles == null) {
			particles = gameObject.AddComponent<ParticleSystem> ();
		}
		particles.Stop ();
	}

	void Update ()
	{
		ExplodeEverything ();
		if (breakOnNoSupports) {
			CheckDownwardsForSupports ();
		}
	}

	void CheckDownwardsForSupports ()
	{
		if (!Physics.Raycast (transform.position, Vector3.down, raycastLength)) {
			ExplodeEverything ();
		}
	}

	void OnCollisionEnter (Collision collision)
	{
		if (collision.transform.CompareTag (TAG_ALLOWED_TO_EXPLODE_THINGS) && breakOnCollision) {
			if (collision.relativeVelocity.magnitude > velocityToBreak) {
				ExplodeEverything ();
			}
		}
	}

	public void ExplodeEverything ()
	{
		SetPiecesKinematic (false);
		SetRenderersEnabled (true);
		renderers [0].gameObject.SetActive (false);
		if (soundOnBreak) {
			src.Play ();
		}
		if (particlesOnBreak) {
			particles.Play ();
		}
	}

	void SetPiecesKinematic (bool valueIn)
	{
		foreach (Rigidbody rigid in rigids) {
			rigid.isKinematic = valueIn;
			rigid.GetComponent<Collider> ().enabled = !valueIn;
		}
		coll.enabled = valueIn;
		GetComponent<Rigidbody> ().isKinematic = !valueIn;
	}

	public void BreakWithExplosiveForce (float force, float radius = 3)
	{
		ExplodeEverything ();
		foreach (Rigidbody rigid in rigids) {
			rigid.AddExplosionForce (force, transform.position, radius);
		}
	}
}
