using UnityEngine;

public class PlayerView : MonoBehaviour
{
    [SerializeField] Transform holder;
    [SerializeField] Transform view;

    [Header("Debug variables")]
    [Tooltip("Use this to set a target for the camera without having to call SetTarget.")]
    [SerializeField] Transform target;

    [Header("Automatic Translation")]
    [Tooltip(
        "Camera will look at vertical offset from target position.")]
    [SerializeField] float verticalOffset;
    [Tooltip(
        "This value is multiplied by the targets velocity to add to the camera's offset" +
        " from its target.")]
    [SerializeField] float previewLength;
    [Tooltip(
        "Percentage of the distance between the camera and its target that will be " +
        "covered each second.")]
    [SerializeField] float moveRatioPerSecond;

    [Header("Input Based Transformations")]
    [Tooltip(
        "How quick inputs will make the camera holder rotate on the Y axis.")]
    [SerializeField] float horizontalSensitivity;
    [Tooltip(
        "How quick inputs will make the camera move up and down on the Y axis.")]
    [SerializeField][Range(0, 1)] float verticalSensitivity;
    [Tooltip(
        "Minimum height of the camera relative to its target.")]
    [SerializeField] float minHeight;
    [Tooltip(
        "Maximum height of the camera relative to its target.")]
    [SerializeField] float maxHeight;

    public float Angle { get { return transform.eulerAngles.y; } }

    public Camera View { get { return view.GetComponent<Camera>(); } }

    Rigidbody targetBody;
    float targetHeight;

    void Start()
    {
#if UNITY_EDITOR
        if (target != null)
        {
            SetTarget(target);
            target.GetComponent<PlayerController>().View = this;
        }
#endif
    }

    void FixedUpdate()
    {
        Vector3 targetPosition = GetTargetPosition();
        float closure = moveRatioPerSecond * Time.deltaTime;
        holder.position = Vector3.Lerp(holder.position, targetPosition, closure);
        view.LookAt(target.position + Vector3.up * verticalOffset);
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

    public void Rotate(float h, float v)
    {
        // Horizontal rotation
        Vector3 localUp = holder.InverseTransformDirection(Vector3.up);
        holder.Rotate(localUp, h * horizontalSensitivity * Time.fixedDeltaTime);

        // Vertical positionning
        if (v > verticalSensitivity)
        {
            targetHeight = maxHeight;
        }
        else if (-v > verticalSensitivity)
        {
            targetHeight = minHeight;
        }
    }

    Vector3 GetTargetPosition()
    {
        Vector3 preview = Vector3.zero;
        if (targetBody != null)
        {
            preview = targetBody.velocity * previewLength;
        }
        return target.position + preview + Vector3.up * targetHeight;
    }
}
