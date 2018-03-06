using UnityEngine;

[System.Serializable]
public class FastBoost : CharacterModule
{
    [SerializeField] float speedModifier = 1.6f;
    [SerializeField] TrailRenderer motionTrail;

    public override bool IsEnabled
    {
        get { return base.IsEnabled; }
        set
        {
            base.IsEnabled = value;
            motionTrail.enabled |= value;
        }
    }

    public override void FixedUpdate()
    {
        OwnBody.velocity = new Vector3(
            OwnBody.velocity.x * speedModifier, 
            OwnBody.velocity.y, 
            OwnBody.velocity.z * speedModifier);
    }
}
