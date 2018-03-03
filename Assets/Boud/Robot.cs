using UnityEngine;

public class Robot : Character
{
    [Header("Modules")]
    [SerializeField] MovementModule movement;

    [Header("Sensors")]
    [SerializeField] GroundSensor ground;

    void Start()
    {
        InitializeModules(new CharacterModule[]
            {
                movement,
            });
        
        movement.Ground = ground;

        SetController(GetComponent<InputController>());
    }

    protected override void SubscribeToController()
    {
        Controller.Subscribe(CharacterAction.MOVE, Move);
    }

    void Move(float h, float v)
    {
        var input = new Vector2(h, v);
        movement.MoveTowardsLocal(h, v, input.sqrMagnitude >= 1 ? 1 : input.magnitude);
    }
}
