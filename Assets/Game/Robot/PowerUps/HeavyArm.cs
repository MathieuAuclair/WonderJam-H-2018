using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class HeavyArm : CharacterModule
{
    [SerializeField]
    float upgradedWeight = 15.0f;

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
        OwnBody.mass = upgradedWeight;
    }
}
