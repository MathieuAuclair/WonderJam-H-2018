using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PushToStart : MonoBehaviour {

    [SerializeField]
    CountDown countdown;

	// Use this for initialization
	void Start ()
    {
		
	}
	
	// Update is called once per frame
	void FixedUpdate ()
    {
		if (Input.GetKeyDown("space"))
        {
            countdown.Initiate(3, "Destroy UnicornCity!!!");
        }
	}
}
