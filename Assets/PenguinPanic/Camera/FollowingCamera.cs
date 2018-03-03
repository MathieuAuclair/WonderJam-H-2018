using UnityEngine;

[RequireComponent(typeof(SpringJoint))]
public class FollowingCamera : MonoBehaviour
{
    [Tooltip("Base offset between the camera and its target.")]
    [SerializeField] Vector3 baseOffset;
    [Tooltip(
        "This value is multiplied by the targets velocity to add to the camera's offset" +
        " from its target.")]
    [SerializeField] float previewLength;

    [Header("Debug variables")]
    [Tooltip("Use this to set a target for the camera without having to call SetTarget.")]
    [SerializeField] Transform target;

    Rigidbody targetBody;
    SpringJoint spring;

    void Start()
    {
        #if UNITY_EDITOR
        if (target != null)
            SetTarget(target);
        #endif

        spring = GetComponent<SpringJoint>();

        // Next line ensures that the rigidbody will never sleep, avoiding anchor lock bug
        spring.GetComponent<Rigidbody>().sleepThreshold = 0;
    }

    void Update()
    {
        if (target != null)
        {
            spring.connectedAnchor =
                target.position + baseOffset + GetPreviewFromTargetVelocity();
        }
    }

    /// <summary>
    /// Sets the target of the camera. Camera's distance from the target is affected by
    /// baseOffset, previewLenght and target's velocity.
    /// </summary>
    /// <param name="newTarget">The target the camera should follow.</param>
    public void SetTarget(Transform newTarget)
    {
        target = newTarget;
        targetBody = target.GetComponent<Rigidbody>();
    }

    Vector3 GetPreviewFromTargetVelocity()
    {
        Vector3 preview = Vector3.zero;
        if (targetBody != null)
        {
            preview = targetBody.velocity * previewLength;
        }
        return preview;
    }
}
