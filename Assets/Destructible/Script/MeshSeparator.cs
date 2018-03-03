using System;
using System.Collections;
using System.Collections.Generic;
using MeshSplitting.Splitables;
using MeshSplitting.Splitters;
using ProBuilder2.Common;
using UnityEngine;

public class MeshSeparator : MonoBehaviour
{


    private void OnCollisionEnter(Collision other)
    {
        IdentifyPropGameObject(other.gameObject);
    }

    private void IdentifyPropGameObject(GameObject other)
    {
        if (other.gameObject.CompareTag("prop") || other.gameObject.CompareTag("broken-prop"))
        {
            CreateSlicerBasedOnGameObject(other.gameObject);
        }
    }

    private void wakeSleepingRigidBody(GameObject prop)
    {
        Rigidbody propRb = prop.GetComponent<Rigidbody>();

        if (propRb == null)
        {
            propRb.WakeUp();
        }
    }

    private void CreateSlicerBasedOnGameObject(GameObject prop)
    {
        CreateBladeWithRotation(prop, Quaternion.Euler(90, 0, 0));
        CreateBladeWithRotation(prop, Quaternion.Euler(0, 90, 0));
        CreateBladeWithRotation(prop, Quaternion.Euler(0, 0, 90));
    }

    private void CreateBladeWithRotation(GameObject prop, Quaternion rotation)
    {
        GameObject blade = GameObject.CreatePrimitive(PrimitiveType.Plane);
        blade.GetComponent<Renderer>().enabled = false;
        blade.transform.SetPositionAndRotation(GetPropDimensions(prop), rotation);
        blade.transform.position = prop.transform.position;
        blade.AddComponent(typeof(SplitterSingleCut));
        blade.AddComponent(typeof(MeshCollider));
        blade.GetComponent<MeshCollider>().convex = true;
        blade.GetComponent<MeshCollider>().isTrigger = true;
    }

    private Vector3 GetPropDimensions(GameObject prop)
    {
        return prop.GetComponent<Renderer>().bounds.size / 10;
    }
}