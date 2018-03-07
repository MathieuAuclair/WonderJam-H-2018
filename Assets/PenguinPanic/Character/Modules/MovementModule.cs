using UnityEngine;

/// <summary>
/// Movement manages wuick and responsive character movements.
/// </summary>
[System.Serializable]
public class MovementModule : CharacterModule
{
    struct Move
    {
        public float h;
        public float v;
        public float speedRatio;
    }

    const int FLOATING_OBJECT_LAYER = 9;

    [Tooltip(
        "Maximum magnitude of penguin's velocity on the ground.")]
    [SerializeField]
    float maximumSpeed;

    Move input;
    Vector3 facing;

    public bool CanWalk { private get; set; }

    public GroundSensor Ground { private get; set; }

    public bool IsWalking { get; private set; }

    public override bool IsEnabled
    {
        get { return base.IsEnabled; }
        set
        {
            base.IsEnabled = value;
            if (!value)
            {
                MoveTowardsLocal(0, 0, 0);
                ProcessInput();
            }
        }
    }

    /// <summary>
    /// Sets object horizontal velocity. Final movement will be influenced by speed.
    /// Input data are considered to be in the object's local coordinates.
    /// </summary>
    /// <param name="h">X axis direction component.</param>
    /// <param name="v">Z axis direction component.</param>
    /// <param name="speedRatio">Ratio of maximum speed to use to affect velocity's horizontal component.</param>
    public void MoveTowardsLocal(float h, float v, float speedRatio = 1)
    {
        input.h = h;
        input.v = v;
        input.speedRatio = speedRatio;
    }

    void ProcessInput()
    {
        SetAnimFloat("Speed", input.speedRatio);

        Vector3 movement;

        if (CanWalk && Ground.IsGrounded)
        {
            EnableState();
        }
        else
        {
            DisableState();
        }

        movement = new Vector3(
            input.h, 0, input.v).normalized * maximumSpeed * input.speedRatio;
        facing = movement;
        movement.y = OwnBody.velocity.y;

        OwnBody.velocity = movement;
    }

    Vector3 GetGroundMovement()
    {
        Vector3 movement = Vector3.ProjectOnPlane(
                               new Vector3(input.h, 0, input.v), Ground.Normal);
        movement.Normalize();
        movement *= maximumSpeed * Mathf.Min(input.speedRatio, 1);
        facing = movement;
        IsWalking = !movement.Equals(Vector3.zero);

        Vector3 groundVelocity = Ground.Velocity;
        movement.x += groundVelocity.x;
        movement.z += groundVelocity.z;
        return movement;
    }

    public override void FixedUpdate()
    {
        ProcessInput();
        AdjustFacing();
    }

    /// <summary>
    /// Smoothly ajusts rotation on the Y axis according to velocity.
    /// </summary>
    void AdjustFacing()
    {
        var horizontalDirection = facing;
        if (horizontalDirection != Vector3.zero)
        {
            Quaternion baseRotation = Parent.rotation;
            Quaternion targetRotation = Quaternion.LookRotation(horizontalDirection, Vector3.up);
            Parent.rotation = Quaternion.Lerp(
                baseRotation, targetRotation, 10 * Time.fixedDeltaTime);
        }
    }
}
