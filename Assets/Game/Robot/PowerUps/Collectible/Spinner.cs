using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spinner : MonoBehaviour {

	public float speed = 5;

	void Update () {
		gameObject.transform.Rotate(new Vector3(0,speed,0));
	}
}
