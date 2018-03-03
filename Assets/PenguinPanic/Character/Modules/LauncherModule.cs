using UnityEngine;
using System;
using UnityEngine.Serialization;

[System.Serializable]
public class LauncherModule : CharacterModule
{
    [FormerlySerializedAs("aimingArrow")]
    [Tooltip("Effect shown when aiming a torpedo.")]
    [SerializeField]
    GameObject aimingFeedback;

    Vector3 aim;
    ThresholdAction propulsion;
    bool isAiming;

    Func<IProjectile> _getProjectile;

    public Func<IProjectile> ProjectileProvider
    {
        set { _getProjectile = value; }
    }

    public override void Initialize(Transform parent, Animator animator, Rigidbody body)
    {
        base.Initialize(parent, animator, body);
        propulsion = new ThresholdAction(0.2f, Begin, End);
    }

    public void Propel(float buttonPressure)
    {
        propulsion.SetMagnitude(buttonPressure);
    }

    /// <summary>
    /// Store the aiming input for later use.
    /// </summary>
    /// <param name="h">Horizontal component</param>
    /// <param name="v">Vertical component</param>
    public void SetAim(float h, float v)
    {
        aim.Set(h, 0, v);
    }

    /// <summary>
    /// Begin Aiming for the torpedo.
    /// </summary>
    public void Begin()
    {
        isAiming = true;
        aimingFeedback.transform.rotation = Parent.rotation;
        aimingFeedback.SetActive(true);           
    }

    /// <summary>
    /// End Aiming for the torpedo.
    /// </summary>
    public void End()
    {
        aimingFeedback.SetActive(false);
        isAiming = false;
        if (aim.sqrMagnitude > 0.3f)
        {
            IProjectile projectile = _getProjectile();
            if (projectile != null)
            {
                projectile.Launch(aim);
            }
            aim.Set(0, 0, 0);
        }
    }

    public override void FixedUpdate()
    {       
        if (isAiming)
        {
            ApplyAim();
        }
    }

    void ApplyAim()
    {
        if (aim != Vector3.zero)
        {
            // Point arrow in the direction where we are aiming
            Quaternion targetRotation = Quaternion.LookRotation(aim, Vector3.up);
            aimingFeedback.transform.rotation = targetRotation;
        }
    }
}
