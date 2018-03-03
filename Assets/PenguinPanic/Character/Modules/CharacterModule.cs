using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[System.Serializable]
public class CharacterModule
{
    [SerializeField] bool enabled = true;
    [SerializeField] string stateBoolean;

    Animator ownAnimator;

    protected Transform Parent { get; private set; }

    protected Rigidbody OwnBody { get; private set; }

    public virtual bool IsEnabled
    {
        get { return enabled; }
        set { enabled = value; }
    }

    public virtual void Initialize(Transform parent, Animator animator)
    {
        Parent = parent;
        OwnBody = parent.GetComponent<Rigidbody>();
        ownAnimator = animator;
    }

    public virtual void FixedUpdate()
    {
    }

    public virtual void OnCollisionEnter(Collision collision)
    {
    }

    protected void EnableState()
    {
        if (Parent.gameObject.activeSelf && !string.IsNullOrEmpty(stateBoolean))
        {
            ownAnimator.SetBool(stateBoolean, true);
        }
    }

    protected void DisableState()
    {
        if (Parent.gameObject.activeSelf && !string.IsNullOrEmpty(stateBoolean))
        {
            ownAnimator.SetBool(stateBoolean, false);
        }
    }

    protected void SetAnimFloat(string name, float value)
    {
        if (Parent.gameObject.activeSelf && !string.IsNullOrEmpty(name))
        {
            ownAnimator.SetFloat(name, value);
        }
    }

    protected void SetAnimBool(string name, bool value)
    {
        if (Parent.gameObject.activeSelf && !string.IsNullOrEmpty(name))
        {
            ownAnimator.SetBool(name, value);
        }
    }

}
