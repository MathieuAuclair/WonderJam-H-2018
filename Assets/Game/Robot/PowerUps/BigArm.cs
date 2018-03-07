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

    public override bool IsEnabled
    {
        get { return base.IsEnabled; }
        set
        {
            if (value)
                BuffArms();
            base.IsEnabled = value;
        }
    }

    void BuffArms()
    {
        rightArm.localScale *= sizeRatio;
        rightArm.GetComponent<Rigidbody>().mass *= sizeRatio * sizeRatio;
        leftArm.localScale *= sizeRatio;
        leftArm.GetComponent<Rigidbody>().mass *= sizeRatio * sizeRatio;
    }

}
