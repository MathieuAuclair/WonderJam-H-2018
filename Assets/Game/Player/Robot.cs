using UnityEngine;

public class Robot : Character
{
    [Header("Modules")]
    [SerializeField] MovementModule movement;
    [SerializeField] JetPackModule jetpack;
    [SerializeField] TorsoModule torso;

    [Header("Sensors")]
    [SerializeField] GroundSensor ground;

    void Start()
    {
        InitializeModules(new CharacterModule[]
            {
                movement,
                jetpack,
                torso,
            });

        ground.Initialize();
        movement.Ground = ground;
    }

    public void ShutDown()
    {
        movement.IsEnabled = false;
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
