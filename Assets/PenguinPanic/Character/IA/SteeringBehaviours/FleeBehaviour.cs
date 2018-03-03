using UnityEngine;

[System.Serializable]
public class FleeBehaviour : SteeringBehaviour
{
    /// <summary>
    /// Target towards which the agent will flee.
    /// </summary>
    /// <value>The target.</value>
    public Transform Target { get; set; }


    [SerializeField] float duration = 5.0f;
    public float Duration { get{return duration;}}

    protected override Vector3 ComputeSteeringForce()
    {
        if (Target == null)
        {
            return Vector3.zero;
        }

        Vector3 directionSeek =  Parent.position - Target.position;

        if (directionSeek.magnitude <= 3f)
        {
            directionSeek = directionSeek * (directionSeek.magnitude / 3f);
        }

        return directionSeek;
    }
}

