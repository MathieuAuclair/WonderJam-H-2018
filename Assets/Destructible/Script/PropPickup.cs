using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PropPickup : MonoBehaviour
{

    void FixedUpdate()
    {
        RaycastHit hit;
        Vector3 forward = transform.TransformDirection(Vector3.forward) * 10;
        Debug.DrawRay(transform.position, forward, Color.cyan);

        if (Physics.Raycast(transform.position, -Vector3.up, out hit, 100.0f))
            print("Found an object - distance: " + hit.distance);
    }
}
