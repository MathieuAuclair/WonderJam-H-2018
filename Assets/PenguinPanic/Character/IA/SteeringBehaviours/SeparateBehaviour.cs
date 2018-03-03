using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// Adds a steering force that makes the agent separate itself from its set neighbours.
/// </summary>
[System.Serializable]
public class SeparationBehaviour : SteeringBehaviour
{
    [SerializeField] float maxSeparation;

    HashSet<Transform> neighbours = new HashSet<Transform>();

    protected override Vector3 ComputeSteeringForce()
    {
        Vector3 desiredVelocity = Vector3.zero;
        //On cherche à itérer sur tous les voisins proche et à séloigner de tous.
        if (neighbours.Count > 0)
        {
            foreach (Transform t in neighbours)
            {
                if (t != null)
                {
                    Vector3 directionSeparation = Parent.position - t.position;
                    if (!directionSeparation.Equals(Vector3.zero))
                    {
                        Vector3 separationForce = directionSeparation.normalized / directionSeparation.magnitude;
                        desiredVelocity += Vector3.ClampMagnitude(separationForce, maxSeparation);
                    }
                    else
                    {
                        desiredVelocity += GetRandomSign() * Vector3.forward * maxSeparation;
                    }
                }
            }
        }

        return desiredVelocity;
    }

    int GetRandomSign()
    {
        return (Random.Range(0, 1) * 2) - 1;
    }

    /// <summary>
    /// Adds a neighbour to separate from.
    /// </summary>
    /// <param name="neighbour">Neighbour.</param>
    public void AddNeighbour(Transform neighbour)
    {
        neighbours.Add(neighbour);
    }

    /// <summary>
    /// Removes a neighbour so the agent won't separate from it anymore.
    /// </summary>
    /// <param name="neighbour">Neighbour.</param>
    public void RemoveNeighbour(Transform neighbour)
    {
        if (neighbours.Contains(neighbour))
        {
            neighbours.Remove(neighbour);
        }
    }
}
