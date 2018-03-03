using UnityEngine;

public class Robot : Character
{
    [Header("Modules")]
    [SerializeField] MovementModule movement;
    [SerializeField] JetPackModule jetpack;

    [Header("Sensors")]
    [SerializeField] GroundSensor ground;

    void Start()
    {
        InitializeModules(new CharacterModule[]
            {
                movement,
                jetpack,
            });

        ground.Initialize();
        movement.Ground = ground;

        SetController(GetComponent<InputController>());
    }

    protected override void SubscribeToController()
    {
        Controller.Subscribe(CharacterAction.MOVE, Move);
        Controller.Subscribe(CharacterAction.JUMP, jetpack.Begin, jetpack.End);
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
