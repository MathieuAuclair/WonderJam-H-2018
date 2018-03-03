using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class Sensor
{
    readonly HashSet<Transform> sensedObjects = new HashSet<Transform>();
    readonly RaycastHit noHit = new RaycastHit();

    [Tooltip("Sensor's radius inside which sensing occurs.")]
    [SerializeField] float radius;
    [Tooltip("Layers to sense.")]
    [SerializeField] LayerMask layers;
    [Tooltip("Wheter or not to detect trigger colliders")]
    [SerializeField] bool senseTriggers = true;
    [Tooltip("Tags to sense, separated by a ','. Do not add spaces between tags.")]
    [SerializeField] string tags;
    [Tooltip("Check this box to orient offset is local space.")]
    [SerializeField] bool useLocalOffset = true;
    [Tooltip("Transform from which offset is applied and rotated.")]
    [SerializeField] Transform source;
    [Tooltip("Offset from transform local space where will be sensor's origin.")]
    [SerializeField] Vector3 offset;
    [Tooltip("Vector to move the sphere cas (and thus sense a capsule).")]
    [SerializeField] Vector3 ray;
    [Tooltip(
        "Whether or not we want to raycast all colliders. Useful when needing to " +
        "identify a specific target from multiple instances.")]
    [SerializeField] bool all = true;

    IList<string> parsedTags;
    float distance;
    bool noTagMask;

    Vector3 Origin
    {
        get
        {
            return source.position +
            (useLocalOffset ? source.TransformVector(offset) : offset);
        }
    }

    /// <summary>
    /// Transform of the last object sensed by the sensor.
    /// </summary>
    /// <value>The sensed Transform.</value>
    public Transform SensedObject { get; private set; }

    /// <summary>
    /// Rigidbody of the last object sensed by the sensor. Be careful as this value can
    /// be null.
    /// </summary>
    /// <value>The sensed Rigidbody.</value>
    public Rigidbody SensedBody { get; private set; }

    public void DrawGizmo()
    {
        if (source != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(Origin, radius);
        }
    }

    public void Initialize()
    {
        distance = ray.magnitude;
        if (ray == Vector3.zero)
        {
            ray = Vector3.one;
        }
        noTagMask = string.IsNullOrEmpty(tags);
        parsedTags = new List<string>(tags.Split(new char[]{ ',' })); 
    }

    /// <summary>
    /// Checks if any targeted collider has been sensed.
    /// </summary>
    /// <returns><c>true</c>, if any was sensed, <c>false</c> otherwise.</returns>
    public bool SenseAny()
    {
        Vector3 usedRay = (distance > 0 && useLocalOffset) ? 
            source.TransformVector(this.ray) : this.ray;

        sensedObjects.Clear();
        bool sensedSomething = false;
        RaycastHit lastHit = noHit;
        if (all)
        {
            RaycastHit[] hits = Physics.SphereCastAll(Origin, radius, usedRay, distance, layers);
            foreach (RaycastHit hit in hits)
            {
                if ((senseTriggers || !hit.collider.isTrigger) &&
                    (noTagMask || parsedTags.Contains(hit.collider.tag)))
                {
                    lastHit = hit;
                    sensedObjects.Add(lastHit.transform);
                }
            }
            sensedSomething = lastHit.collider != null;
        }
        else
        {
            Physics.SphereCast(Origin, radius, usedRay, out lastHit, distance, layers);
            sensedSomething = (lastHit.collider != null && (senseTriggers ||
            !lastHit.collider.isTrigger) && (noTagMask ||
            parsedTags.Contains(lastHit.collider.tag)));
        }

        if (sensedSomething)
        {
            SensedObject = lastHit.transform;
            SensedBody = lastHit.rigidbody;
            sensedSomething &= OnSense(lastHit);
        }
        else
        {
            OnNotSense();
        }
        
        return sensedSomething;
    }

    /// <summary>
    /// Checks if a specific object is targeted and sensed by the sensor.
    /// </summary>
    /// <param name="target">Target.</param>
    public bool Sense(Transform target)
    {
        SenseAny();
        return sensedObjects.Contains(target);
    }

    protected virtual bool OnSense(RaycastHit hit)
    {
        return true;
    }

    protected virtual void OnNotSense()
    {
    }
}
