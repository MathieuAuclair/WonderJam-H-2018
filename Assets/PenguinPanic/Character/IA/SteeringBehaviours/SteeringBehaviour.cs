using UnityEngine;

[System.Serializable]
public class SteeringBehaviour
{
    [SerializeField][Range(0, 5)] float power;
    [SerializeField] bool isActive;

    public bool IsActive
    { 
        get { return isActive; }
        set { isActive = value; }
    }

    protected Transform Parent { get; private set; }

    public void Initialize(Transform parent)
    {
        Parent = parent;
    }

    public Vector3 GetSteeringForce()
    {
        return power * ComputeSteeringForce();
    }

    protected virtual Vector3 ComputeSteeringForce()
    {
        return Vector3.zero;
    }
}
