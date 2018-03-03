using UnityEngine;

/// <summary>
/// Used to jump. Also manages going higher if jumping input is maintained.
/// </summary>
[System.Serializable]
public class JumpModule : CharacterModule
{
    [Tooltip("Vertical velocity boost applied during a jump.")]
    [SerializeField]
    float upwardSpeed;

    [Tooltip("Time during which vertical velocity boost can be maintained.")]
    [SerializeField] float maxTime;

    float jumpTimeLeft;

    public bool IsJumping { get; private set; }

    public bool CanJump { get; set; }

    public override void FixedUpdate()
    {
        if (IsJumping && jumpTimeLeft > 0)
        {
            ApplyJump();
            jumpTimeLeft -= Time.fixedDeltaTime;
            if (CanJump)
            {
                jumpTimeLeft = maxTime;
            }
        }
    }

    /// <summary>
    /// Begin jumping.
    /// </summary>
    public void Begin()
    {
        if (CanJump)
        {
            IsJumping = true;
            EnableState();
            jumpTimeLeft = maxTime;
        }
    }

    /// <summary>
    /// End jumping.
    /// </summary>
    public void End()
    {
        IsJumping = false;
        DisableState();
    }

    void ApplyJump()
    {
        var velocity = OwnBody.velocity;
        velocity.y = upwardSpeed;
        OwnBody.velocity = velocity;
    }

}
