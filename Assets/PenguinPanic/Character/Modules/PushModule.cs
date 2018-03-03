using UnityEngine;

/// <summary>
/// Manages pushing stuff around.
/// </summary>
[System.Serializable]
public class PushModule : CharacterModule
{
    [Tooltip("Used for knowing if there is a floating object to push in front.")]
    [SerializeField] Sensor floatingObject;
    [SerializeField] float power;

    public override void Initialize(Transform parent)
    {
        base.Initialize(parent);
        floatingObject.Initialize();
    }

    public override void FixedUpdate()
    {
        if (floatingObject.SenseAny())
        {
            EnableState();
            Rigidbody pushedObject = floatingObject.SensedBody;
            if (pushedObject != null)
            {
                pushedObject.AddForceAtPosition(Parent.forward * power, Parent.position);
            }
        }
        else
        {
            DisableState();
        }
    }

    public void DrawGizmos()
    {
        floatingObject.DrawGizmo();
    }
}
