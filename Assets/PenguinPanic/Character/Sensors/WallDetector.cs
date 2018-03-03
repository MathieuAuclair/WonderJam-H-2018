using UnityEngine;

/// <summary>
/// Wall Detector is used to detect if entering and leaving collisions are with walls or
/// not and stores data about its results. This class could be further expended to use
/// raycasts and such for AI purposes.
/// </summary>
public class WallDetector : MonoBehaviour
{
    const int FLOATING_OBJECT_LAYER = 9;
    const int FOOD_LAYER = 12;

    [SerializeField] LayerMask wallLayers;
    [SerializeField] Transform triggerCollider;
    [SerializeField] float criticalSlopeAngle;

    Collider touchedWall;

    /// <summary>
    /// Gets a value indicating whether this instance is touching a wall.
    /// </summary>
    /// <value><c>true</c> if last DetectWall detected a wall; otherwise, <c>false</c>. Also becomes fall if wall collider is passed to ExitCollider.</value>
    public bool IsColliding { get; private set; }

    /// <summary>
    /// Horizontal component of the last touched wall's normal.
    /// </summary>
    /// <value>An horizontal vector pointing towards the wall's outer face.</value>
    public Vector3 Facing { get; private set; }

    Vector3 _sensedDirection;

    public Vector3 SensedDirection
    {
        get { return _sensedDirection; }
        set
        {
            _sensedDirection = value.normalized;
            triggerCollider.position = triggerCollider.parent.position + _sensedDirection * 0.1f;
        }
    }

    void Start()
    {
        _sensedDirection = Vector3.forward;
    }

    void OnTriggerEnter(Collider other)
    {
        DetectWall(other);
    }

    /// <summary>
    /// Detects collision for wall.
    /// </summary>
    /// <param name="col">Entering collision to inspect.</param>
    public void DetectWall(Collider col)
    {
        // Floating objects and food can be pushed and do not count as walls.
        if (col.isTrigger || wallLayers != (wallLayers | (1 << col.gameObject.layer)))
        {
            return;
        }
        
        // We check for first available contact and remove velocity component that is
        // going towards the wall.
        RaycastHit hit;
        col.Raycast(new Ray(triggerCollider.position, SensedDirection), out hit, 5);
        Vector3 normal = hit.normal;
        IsColliding = Vector3.Angle(Vector3.up, normal) > criticalSlopeAngle;
        if (IsColliding)
        {
            touchedWall = col;
            normal.y = 0;
            Facing = normal;
        }
        
    }

    void OnTriggerExit(Collider other)
    {
        if (other == touchedWall)
        {
            touchedWall = null;
            IsColliding = false;
        }
    }
}
