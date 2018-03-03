using UnityEngine;
using System.Collections.Generic;

public static class PlayerAction
{
    public const string ROTATE_VIEW = "Camera";
}

public class PlayerController : InputController
{
    public const string AIM_HORIZONTAL = "P{0}Horizontal2";
    public const string AIM_VERTICAL = "P{0}Vertical2";
    public const string HORIZONTAL_AXIS = "P{0}Horizontal1";
    public const string VERTICAL_AXIS = "P{0}Vertical1";
    public const string JUMP = "P{0}Jump";

    [SerializeField] int playerId;

    IList<string> actions;
    IList<string> axialActions;
    Vector2 move;
    Vector2 aim;


    PlayerView _view;

    public PlayerView View
    {
        private get { return _view; }
        set
        {
            _view = value;
            Subscribe(PlayerAction.ROTATE_VIEW, View.Rotate);
        }
    }

    IDictionary<string, string> inputMapping;

    void Start()
    {
        MapInputs();

        actions = new List<string>(1)
        {
            CharacterAction.JUMP,
        };

        axialActions = new List<string>(0);
    }

    void MapInputs()
    {
        inputMapping = new Dictionary<string, string>(5);
        Map(HORIZONTAL_AXIS);
        Map(VERTICAL_AXIS);
        Map(AIM_HORIZONTAL);
        Map(AIM_VERTICAL);
        Map(CharacterAction.JUMP);
    }

    void Map(string key)
    {
        inputMapping.Add(key, string.Format(key, playerId));
    }

    void Update()
    {
        foreach (string action in actions)
        {
            if (Input.GetButtonDown(inputMapping[action]))
            {
                BeginAction(action);
            }
            if (Input.GetButtonUp(inputMapping[action]))
            {
                EndAction(action);
            }
        }
        foreach (string axialAction in axialActions)
        {
            AxialAction(axialAction, Input.GetAxis(axialAction));
        }
    }


    void FixedUpdate()
    {
        move.Set(
            Input.GetAxis(inputMapping[HORIZONTAL_AXIS]), 
            Input.GetAxis(inputMapping[VERTICAL_AXIS]));
        aim.Set(
            Input.GetAxis(inputMapping[AIM_HORIZONTAL]),
            Input.GetAxis(inputMapping[AIM_VERTICAL]));
        
        BiaxialAction(PlayerAction.ROTATE_VIEW, aim.x, aim.y);

        if (View != null)
        {
            aim = Vector2.up.Rotate(-View.Angle);
            move = move.Rotate(-View.Angle);
        }

        BiaxialAction(CharacterAction.MOVE, move.x, move.y);
    }
}
