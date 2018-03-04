using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class BigArm : CharacterModule
{
    [SerializeField]
    Transform rightArm;
    [SerializeField]
    Transform leftArm;

    [SerializeField]
    float sizeRatio = 4;

    public override bool IsEnabled {
        get { return base.IsEnabled;}
        set {
            if(value)
                BuffArms();
            base.IsEnabled = value;
        }
    }

    void BuffArms()
    {
        rightArm.localScale = new Vector3(rightArm.localScale.x * sizeRatio, rightArm.localScale.y * sizeRatio, rightArm.localScale.z * sizeRatio);
        leftArm.localScale = new Vector3(leftArm.localScale.x * sizeRatio, leftArm.localScale.y * sizeRatio, leftArm.localScale.z * sizeRatio);
    }
}
