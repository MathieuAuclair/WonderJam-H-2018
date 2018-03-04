using UnityEngine;
using System.Collections;

[RequireComponent(typeof(ParticleSystem))]
public class PooledParticleSystem : MonoBehaviour
{
	private GameObjectPool _pool;

	void OnPoolCreate(GameObjectPool pool)
	{
		_pool = pool;

		GetComponent<Renderer>().enabled = true;
		GetComponent<ParticleSystem>().time = 0;
		GetComponent<ParticleSystem>().Clear(true);
		GetComponent<ParticleSystem>().Play(true);
	}

	void OnPoolRelease()
	{
		GetComponent<ParticleSystem>().Stop();
		GetComponent<ParticleSystem>().time = 0;
		GetComponent<ParticleSystem>().Clear(true);
		GetComponent<Renderer>().enabled = false;
	}

	void Update()
	{
		if (!GetComponent<ParticleSystem>().IsAlive(true) && GetComponent<Renderer>().enabled)
		{
			_pool.ReleaseInstance(transform);
		}
	}
}
