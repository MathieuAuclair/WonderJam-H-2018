using System.Collections.Generic;
using UnityEngine;

public class Robot : Character
{
    [Header("Modules")]
    [SerializeField] MovementModule movement;
    [SerializeField] HeavyArm heavyArm;
    [SerializeField] BigArm bigArm;
    [SerializeField] FastBoost fastBoost;
    [SerializeField] JetPackModule jetpack;
    [SerializeField] TorsoModule torso;
    [SerializeField] LaserModule laser;

    [Header("Sensors")]
    [SerializeField] GroundSensor ground;

    IDictionary<PowerUpPicker.Power, CharacterModule> powerUps;

    void Start()
    {
        InitializeModules(new CharacterModule[]
            {
                movement,
                heavyArm,
                bigArm,
                fastBoost,
                jetpack,
                torso,
                laser,
            });

        ground.Initialize();
        movement.Ground = ground;

        powerUps = new Dictionary<PowerUpPicker.Power, CharacterModule>()
        {
            { PowerUpPicker.Power.BIG_ARM, bigArm },
            { PowerUpPicker.Power.HEAVY_ARM, heavyArm },
            { PowerUpPicker.Power.FAST, fastBoost },
            { PowerUpPicker.Power.LASER, laser },
        };
    }

    public void ShutDown()
    {
        movement.IsEnabled = false;
        fastBoost.IsEnabled = false;
    }

    public void ActivatePowerUp(PowerUpPicker.Power power, GameObject other)
    {
        CrackleAudio.SoundController.PlaySound("powerup");
        Destroy(other);
        if (!powerUps[power].IsEnabled)
            powerUps[power].IsEnabled = true;
    }

    protected override void SubscribeToController()
    {
        Controller.Subscribe(CharacterAction.MOVE, Move);
        Controller.Subscribe(CharacterAction.JETPACK, jetpack.Begin, jetpack.End);
        Controller.Subscribe(CharacterAction.TORSO, torso.Rotate);
    }

    void Move(float h, float v)
    {
        var input = new Vector2(h, v);
        movement.MoveTowardsLocal(h, v, input.sqrMagnitude >= 1 ? 1 : input.magnitude);
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();
        if (ground.SenseAny())
        {
            jetpack.Recharge();
        }

        movement.CanWalk = !jetpack.IsPropeling;
        jetpack.CanJump = ground.IsGrounded;
    }

    void OnDrawGizmos()
    {
        ground.DrawGizmo();
    }
}
