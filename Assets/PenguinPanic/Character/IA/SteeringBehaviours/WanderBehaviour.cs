using UnityEngine;

[System.Serializable]
public class WanderBehaviour : SteeringBehaviour
{
    const float TARGET_CIRCLE_RADIUS = 0.2f;
    const float TARGET_CIRCLE_DISTANCE = 1.33f;

    [SerializeField][Range(0, 1)] float enthropy;

    float angle;

    protected override Vector3 ComputeSteeringForce()
    {
        angle += Random.Range(-enthropy, enthropy);

        Vector3 targetDestination = new Vector3(Mathf.Cos(angle), 0, Mathf.Sin(angle))
                                    * TARGET_CIRCLE_RADIUS;
        targetDestination += Vector3.forward * TARGET_CIRCLE_DISTANCE;

        return Parent.TransformVector(targetDestination);
    }
}
