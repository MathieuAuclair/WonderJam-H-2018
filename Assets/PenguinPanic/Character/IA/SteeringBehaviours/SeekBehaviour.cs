using UnityEngine;

/// <summary>
/// Adds a steering force towards set target.
/// </summary>
[System.Serializable]
public class SeekBehaviour : SteeringBehaviour
{
    /// <summary>
    /// Target towards which the agent will move.
    /// </summary>
    /// <value>The target.</value>
    public Transform Target { get; set; }

    protected override Vector3 ComputeSteeringForce()
    {
        if (Target == null)
        {
            return Vector3.zero;
        }

        Vector3 directionSeek = Target.position - Parent.position;

        if (directionSeek.magnitude <= 3f)
        {
            directionSeek = directionSeek * (directionSeek.magnitude / 3f);
        }

        return directionSeek;
    }
}
