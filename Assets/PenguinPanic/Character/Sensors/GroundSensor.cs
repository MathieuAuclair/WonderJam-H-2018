using UnityEngine;

[System.Serializable]
public class GroundSensor : Sensor
{
    [Tooltip(
        "Anything under this value is considered to be ground. Anything over is " +
        "considered to be a wall.")]
    [SerializeField][Range(0, 90)] float maximumSlopeAngle;

    Rigidbody detectedBody;

    /// <summary>
    /// Gets a value indicating whether last ground check was positive or not.
    /// </summary>
    /// <value><c>true</c> if ground check was positive; otherwise, <c>false</c>.</value>
    public bool IsGrounded { get; private set; }

    /// <summary>
    /// Normal of the last ground check.
    /// </summary>
    /// <value>The normal.</value>
    public Vector3 Normal { get; private set; }

    /// <summary>
    /// Gets the touched ground's velocity.
    /// </summary>
    /// <value>Velocity of the ground.</value>
    public Vector3 Velocity
    {
        get { return detectedBody != null ? detectedBody.velocity : Vector3.zero; }
    }

    protected override bool OnSense(RaycastHit hit)
    {        
        Normal = hit.normal;
        IsGrounded = (hit.collider != null) &&
        Vector3.Angle(Vector3.up, Normal) <= maximumSlopeAngle;
        detectedBody = IsGrounded ? hit.rigidbody : null;
        return IsGrounded;
    }

    protected override void OnNotSense()
    {
        IsGrounded = false;
        detectedBody = null;
    }
}
