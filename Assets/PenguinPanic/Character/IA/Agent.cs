using UnityEngine;
using System.Collections.Generic;

public class Agent : InputController
{
    [Tooltip(
        "Maximum steering force to apply before which we cease to consider any " +
        "subsequent behaviour.")]
    [SerializeField] float maxForce = 7.0f;

    Vector3 steeringForce;
    IList<SteeringBehaviour> prioritizedBehaviours;

    /// <summary>
    /// Initializes the prioritized steering behaviours. This must be called in a
    /// specialized class'initialization logic to manage steering behaviours properly.
    /// </summary>
    /// <param name="behaviours">Steering behaviours to manage.</param>
    protected void InitializePrioritizedBehaviours(IList<SteeringBehaviour> behaviours)
    {
        this.prioritizedBehaviours = behaviours;
        foreach (SteeringBehaviour behaviour in prioritizedBehaviours)
        {
            behaviour.Initialize(transform);
        }
    }

    void FixedUpdate()
    {
        steeringForce = ComputeSteeringForce();
        BiaxialAction(CharacterAction.MOVE, steeringForce.x, steeringForce.z);
    }

    Vector3 ComputeSteeringForce()
    {
        Vector3 force;
        Vector3 steeringForceAverage = Vector3.zero;

        foreach (SteeringBehaviour behaviour in prioritizedBehaviours)
        {
            if (behaviour.IsActive)
            {
                force = behaviour.GetSteeringForce();
                if (force.magnitude < maxForce - steeringForceAverage.magnitude)
                {
                    steeringForceAverage += force;
                }
                else
                {
                    steeringForceAverage += (force.normalized * (maxForce - steeringForceAverage.magnitude));
                    return steeringForceAverage;
                }
            }
        }

        return steeringForceAverage;
    }
}
