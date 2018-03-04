using UnityEngine;

public static class CharacterAction
{
    public const string MOVE = "Move";
    public const string JETPACK = "P{0}Jump";
    public const string TORSO = "Torso";
    public const string LASER = "P{0}Action2";
}

/// <summary>
/// Manages character movements and actions.
/// </summary>
[System.Serializable]
public abstract class Character : MonoBehaviour
{
    [SerializeField] Animator animator;
    [SerializeField] Rigidbody body;

    CharacterModule[] modules;
    InputController controller;

    protected InputController Controller { get { return controller; } }

    protected void OnCollisionEnter(Collision collision)
    {
        foreach (CharacterModule module in modules)
        {
            if (module.IsEnabled)
            {
                module.OnCollisionEnter(collision);
            }
        }
    }

    /// <summary>
    /// Prepares the character's module so their shared behaviours are managed by the
    /// 'Character' base class.
    /// </summary>
    /// <param name="modules">Modules.</param>
    protected void InitializeModules(CharacterModule[] modules)
    {
        this.modules = modules;
        Transform moduleParent = transform;
        foreach (CharacterModule module in modules)
        {
            module.Initialize(moduleParent, animator, body);
        }
    }

    /// <summary>
    /// Setting a controller will override the previous one. Character actions should be
    /// mapped to its controller by overriding the 'SubscribeToController' method.
    /// </summary>
    /// <param name="controller">Controller to subscribe to.</param>
    public void SetController(InputController controller)
    {
        if (this.controller != null)
        {
            this.controller.UnsubscribeEverything();
        }
        this.controller = controller;
        SubscribeToController();
    }

    /// <summary>
    /// This method is called when a controller is set and should only be used to make
    /// calls to the controller's 'Subscribe' nethod.
    /// </summary>
    protected abstract void SubscribeToController();

    protected virtual void FixedUpdate()
    {
        foreach (CharacterModule module in modules)
        {
            if (module.IsEnabled)
            {
                module.FixedUpdate();
            }
        }
    }
}
