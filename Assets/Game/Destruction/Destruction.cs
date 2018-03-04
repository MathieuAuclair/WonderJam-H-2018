using System.Collections;
using UnityEngine;

public class Destruction : MonoBehaviour
{
	[Space (7)]
	[Header ("State")]
	[Space (2)]
	[Tooltip ("Whether the object starts broken")]
	public bool startBroken;

	[Space (7)]
	[Header ("Explode on Collision")]
	[Space (2)]
	[Tooltip ("Whether the object Explodes when it collides with something")]
	public bool explodeOnCollision = true;
	[Tooltip ("The minimum relative velocity to Explode the object")]
	public float velocityToExplode = 1;

	[Space (7)]
	[Header ("Explode when nothing underneath")]
	[Space (2)]
	[Tooltip ("Whether the object Explodes when there's nothing underneath supporting it")]
	public bool explodeOnNoSupports = false;
	[Tooltip ("The length of the raycast used to check for supports underneath")]
	public float raycastLength = 1f;

	[Space (7)]
	[Header ("Sound On Explode")]
	[Space (2)]
	[Tooltip ("Whether the object makes a sound when it Explodes")]
	public bool soundOnExplode = false;
	[Tooltip ("An array of sounds for the object to make when it Explodes (A random one will be selected)")]
	public AudioClip[] clips;

	[Space (7)]
	[Header ("Particles On Explode")]
	[Space (2)]
	[Tooltip ("Whether the object makes particles when it Explodes")]
	public bool particlesOnExplode = false;

	[Space (7)]
	[Header ("Prop Life Span")]
	[Space (2)]
	[Tooltip ("How much time do you want to keep the destroyed prop")]
	public float propLifeSpan = 10;

	[Space (7)]
	[Header ("Is Prop Destroyed After LifeSpan")]
	[Space (2)]
	[Tooltip ("Whether the object is destroyed or get it's rigidbody disabled")]
	public bool isPropDestroyedAfterLifeSpan = false;


	[SerializeField] const string TAG_ALLOWED_TO_EXPLODE_THINGS = "Player";
    [SerializeField] GameObject hitParticle;

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

		if (soundOnExplode) {
			SetupSound ();
		}
		if (particlesOnExplode) {
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
		//src = GetComponent<AudioSource> ();
		//if (src == null) {
		//	src = gameObject.AddComponent<AudioSource> ();
		//}
		//src.clip = clips [Random.Range (0, clips.Length - 1)];
	}

	void SetupParticles ()
	{
		particles = GetComponent<ParticleSystem> ();
		if (particles == null) {
			particles = gameObject.AddComponent<ParticleSystem> ();
		}
		particles.Stop ();
	}

	void CheckDownwardsForSupports ()
	{
		if (explodeOnNoSupports) {
			if (!Physics.Raycast (transform.position, Vector3.down, raycastLength)) {
				ExplodeEverything ();
			}
		}
	}

	void OnCollisionEnter (Collision collision)
	{
		if (collision.transform.CompareTag (TAG_ALLOWED_TO_EXPLODE_THINGS) && explodeOnCollision) {
			if (collision.relativeVelocity.magnitude > velocityToExplode) {
				ExplodeEverything ();
                CrackleAudio.SoundController.PlaySound("destruction");
                Instantiate(hitParticle, collision.transform.position, Quaternion.identity);
                int scream = Random.Range(0, 4);
                if (scream == 1)
                {
                    CrackleAudio.SoundController.PlaySound("scream");
                }
				ScoreBoard.IncreaseScore (collision.gameObject.tag, 1);
			}
		}
	}

	public void ExplodeEverything ()
	{
		SetPiecesKinematic (false);
		SetRenderersEnabled (true);
		renderers [0].gameObject.SetActive (false);
		if (soundOnExplode) {
			src.Play ();
		}
		if (particlesOnExplode) {
			particles.Play ();
		}
	}

	void SetPiecesKinematic (bool valueIn)
	{
		foreach (Rigidbody rigid in rigids) {
			rigid.isKinematic = valueIn;
			rigid.GetComponent<Collider> ().enabled = !valueIn;
			if (valueIn == false) {
				StartCoroutine (WaitToMakeDisabledAfterPropLifeSpan (rigid.gameObject));
			}
		}
		coll.enabled = valueIn;
		GetComponent<Rigidbody> ().isKinematic = !valueIn;
	}


	private IEnumerator WaitToMakeDisabledAfterPropLifeSpan (GameObject prop)
	{
		yield return new WaitForSeconds (propLifeSpan);
		if (isPropDestroyedAfterLifeSpan) {
			Destroy (prop);
		} else {
			prop.GetComponent<Rigidbody> ().detectCollisions = false;
			Destroy (prop, 2.5f);
		}
	}

	public void ExplodeWithForce (float force, float radius = 3)
	{
		ExplodeEverything ();
		foreach (Rigidbody rigid in rigids) {
			rigid.AddExplosionForce (force, transform.position, radius);
		}
	}
}
