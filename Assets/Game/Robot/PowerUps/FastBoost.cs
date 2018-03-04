using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class FastBoost : CharacterModule
{
    [SerializeField]
    float speedModifier = 1.6f;
    public override void FixedUpdate()
    {
        OwnBody.velocity = new Vector3(OwnBody.velocity.x * speedModifier, 0, OwnBody.velocity.z * speedModifier);
    }
}
