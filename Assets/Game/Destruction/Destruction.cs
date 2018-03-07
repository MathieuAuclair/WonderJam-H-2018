using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destruction : MonoBehaviour
{
    [Space(7)]
    [Header("State")]
    [Space(2)]
    [Tooltip("Whether the object starts broken")]
    public bool startBroken;

    [Space(7)]
    [Header("Explode on Collision")]
    [Space(2)]
    [Tooltip("Whether the object Explodes when it collides with something")]
    public bool explodeOnCollision = true;
    [Tooltip("The minimum relative velocity to Explode the object")]
    public float velocityToExplode = 1;

    [Space(7)]
    [Header("Explode when nothing underneath")]
    [Space(2)]
    [Tooltip("Whether the object Explodes when there's nothing underneath supporting it")]
    public bool explodeOnNoSupports = false;
    [Tooltip("The length of the raycast used to check for supports underneath")]
    public float raycastLength = 1f;

    [Space(7)]
    [Header("Sound On Explode")]
    [Space(2)]
    [Tooltip("Whether the object makes a sound when it Explodes")]
    public bool soundOnExplode = false;
    [Tooltip("An array of sounds for the object to make when it Explodes (A random one will be selected)")]
    public AudioClip[] clips;

    [Space(7)]
    [Header("Particles On Explode")]
    [Space(2)]
    [Tooltip("Whether the object makes particles when it Explodes")]
    public bool particlesOnExplode = false;

    [Space(7)]
    [Header("Prop Life Span")]
    [Space(2)]
    [Tooltip("How much time do you want to keep the destroyed prop")]
    public float propLifeSpan = 10;

    [Space(7)]
    [Header("Is Prop Destroyed After LifeSpan")]
    [Space(2)]
    [Tooltip("Whether the object is destroyed or get it's rigidbody disabled")]
    public bool isPropDestroyedAfterLifeSpan = false;

    [Space(7)]
    [Header("Prefab powerup")]
    public List<GameObject> powerUps;

    [SerializeField] const string TAG_ALLOWED_TO_EXPLODE_THINGS = "Player";
    [SerializeField] public string particlePoolName = "HitPool";

    AudioSource src;

    Collider coll;
    Rigidbody[] rigids;
    MeshRenderer[] renderers;
    List<Transform> currentlyPlayingParticles;
    GameObjectPool _pool;

    void Start()
    {
        _pool = GameObjectPool.GetPool(particlePoolName);
        renderers = gameObject.GetComponentsInChildren<MeshRenderer>();
        rigids = gameObject.GetComponentsInChildren<Rigidbody>();
        SetRenderersEnabled(false);
        coll = GetComponent<Collider>();
        SetPiecesKinematic(!startBroken);
    }

    void Awake()
    {
        currentlyPlayingParticles = new List<Transform>();
    }

    void SetRenderersEnabled(bool value)
    {
        for (int i = 1; i < renderers.Length; i++)
        {
            renderers[i].enabled = value;
        }
    }

    void Update()
    {
        List<Transform> toDelete = new List<Transform>();
        foreach (var effect in currentlyPlayingParticles)
        {
            if (!effect.GetComponentInChildren<ParticleSystem>().IsAlive(true))
            {
                toDelete.Add(effect);
                _pool.ReleaseInstance(effect);
            }
        }
        foreach (var effectToDelete in toDelete)
        {
            currentlyPlayingParticles.Remove(effectToDelete);
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.CompareTag(TAG_ALLOWED_TO_EXPLODE_THINGS) && explodeOnCollision)
        {
            if (collision.relativeVelocity.sqrMagnitude * collision.rigidbody.mass > velocityToExplode * velocityToExplode)
            {
                ExplodeEverything();
                CrackleAudio.SoundController.PlaySound("destruction");
                var a = _pool.GetInstance(collision.transform.position);
                a.GetComponentInChildren<ParticleSystem>().Play();
                currentlyPlayingParticles.Add(a);
                int scream = Random.Range(0, 4);
                if (scream == 1)
                {
                    CrackleAudio.SoundController.PlaySound("scream");
                }
                UpdateScore(collision, UnityEngine.Random.Range(1f, GetComponent<Destruction>().velocityToExplode));
            }
        }
    }

    static void UpdateScore(Collision collision, float amount)
    {
        ScoreBoard.IncreaseScore(collision.gameObject.GetComponentInParent<PlayerController>().PlayerId, amount);
    }

    public void ExplodeEverything()
    {
        SetPiecesKinematic(false);
        SetRenderersEnabled(true);
        renderers[0].gameObject.SetActive(false);
        if (soundOnExplode)
        {
            src.Play();
        }

        if (gameObject.tag == "dome")
        {
            int index = Random.Range(0, (powerUps.Count));
            GameObject usedPowerUp = powerUps[index];
            Instantiate(usedPowerUp, (transform.position + new Vector3(0, 2, 0)), Quaternion.identity);
        }
    }

    void SetPiecesKinematic(bool valueIn)
    {
        foreach (Rigidbody rigid in rigids)
        {
            rigid.isKinematic = valueIn;
            rigid.GetComponent<Collider>().enabled = !valueIn;
            if (valueIn == false)
            {
                StartCoroutine(WaitToMakeDisabledAfterPropLifeSpan(rigid.gameObject));
            }
        }
        coll.enabled = valueIn;
        GetComponent<Rigidbody>().isKinematic = !valueIn;
    }


    private IEnumerator WaitToMakeDisabledAfterPropLifeSpan(GameObject prop)
    {
        yield return new WaitForSeconds(propLifeSpan);
        if (isPropDestroyedAfterLifeSpan)
        {
            Destroy(prop);
        }
        else
        {
            prop.GetComponent<Rigidbody>().detectCollisions = false;
            Destroy(prop, 2.5f);
        }
    }

    public void ExplodeWithForce(float force, float radius = 3)
    {
        ExplodeEverything();
        foreach (Rigidbody rigid in rigids)
        {
            rigid.AddExplosionForce(force, transform.position, radius);
        }
    }
}
