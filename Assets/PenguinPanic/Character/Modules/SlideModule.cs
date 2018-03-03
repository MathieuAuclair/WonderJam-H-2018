using UnityEngine;

/// <summary>
/// Manages force based horizontal movement.
/// </summary>
[System.Serializable]
public class SlideModule : CharacterModule
{
    struct Slide
    {
        public float h;
        public float v;
        public float speedRatio;
    }

    [Tooltip(
        "Overrides rigidbody's drag when it touches the ground, reverting it to its default " +
        "value once it gets up.")]
    [SerializeField] float dragOverride;

    [Tooltip(
        "Magnitude of the force applied to the character when it slides.")]
    [SerializeField] float power;

    Slide input;
    RigidbodyConstraints defaultConstraints;
    float defaultDrag;

    public GroundSensor Ground { private get; set; }

    public bool CanSlide { get; set; }

    public bool IsSliding { get; private set; }

    public override void Initialize(Transform parent, Animator animator)
    {
        base.Initialize(parent, animator);
        defaultConstraints = OwnBody.constraints;
        defaultDrag = OwnBody.drag;
    }

    public override void FixedUpdate()
    {
        if (IsSliding && CanSlide)
        {
            ProcessInput();
        }
    }

    public void EnableSliding()
    {
        EnableState();
        IsEnabled = true;
        IsSliding = true;
        OwnBody.constraints = RigidbodyConstraints.FreezeRotationZ;
        OwnBody.drag = dragOverride;
    }

    public void DisableSliding()
    {
        DisableState();
        IsEnabled = false;
        IsSliding = false;
        OwnBody.constraints = defaultConstraints;
        OwnBody.drag = defaultDrag;
    }

    void ProcessInput()
    {
        Vector3 force = Vector3.ProjectOnPlane(new Vector3(input.h, 0, input.v), Ground.Normal);
        force.Normalize();
        force *= power * input.speedRatio;

        Vector3 position = Parent.position + Parent.forward * 2;

        OwnBody.AddForceAtPosition(force, position);
    }

    /// <summary>
    /// Sets object sliding force direction. Final movement will be influenced by power.
    /// Input data are considered to be in the object's local coordinates.
    /// </summary>
    /// <param name="h">X axis direction component.</param>
    /// <param name="v">Z axis direction component.</param>
    /// <param name="speedRatio">Ratio of maximum speed to use to affect velocity's horizontal component.</param>
    public void SlideTowardsLocal(float h, float v, float speedRatio = 1)
    {
        input.h = h;
        input.v = v;
        input.speedRatio = speedRatio;
    }
}
