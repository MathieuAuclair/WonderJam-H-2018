using UnityEngine;

/// <summary>
/// Used to jump. Also manages going higher if jumping input is maintained.
/// </summary>
[System.Serializable]
public class JetPackModule : CharacterModule
{
    [Tooltip("JetPack power that propels upward.")]
    [SerializeField]
    float power;

    [Tooltip("Time during which jetpack can be used before touching ground again.")]
    [SerializeField] float fullCharge;

    float chargeLeft;

    public bool IsPropeling { get; private set; }

    public bool CanJump { get; set; }

    public override void FixedUpdate()
    {
        if (IsPropeling && chargeLeft > 0)
        {
            OwnBody.AddForce(Vector3.up * power);
            chargeLeft -= Time.fixedDeltaTime;
        }
    }

    public void Begin()
    {
        IsPropeling = true;
        EnableState();
    }

    public void End()
    {
        IsPropeling = false;
        DisableState();
    }

    public void Recharge()
    {
        chargeLeft = fullCharge;
    }
}
