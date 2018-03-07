using UnityEngine;

[System.Serializable]
public class HeavyArm : CharacterModule
{
    [SerializeField]
    float weightMultiplier = 3;

    public override bool IsEnabled
    {
        get { return base.IsEnabled; }
        set
        {
            if (value)
            {
                OwnBody.mass *= weightMultiplier;
            }
            else
            {
                OwnBody.mass /= weightMultiplier;
            }
            base.IsEnabled = value;
        }
    }
}
