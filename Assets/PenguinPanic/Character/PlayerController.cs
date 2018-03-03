using UnityEngine;
using System.Collections.Generic;

public static class PlayerAction
{
    public const string CHANGE_HAT = "ChangeHat";
    public const string ROTATE_VIEW = "Camera";
}

public class PlayerController : InputController
{
    const string HORIZONTAL_AXIS = "Horizontal";
    const string VERTICAL_AXIS = "Vertical";
    const string TORPEDO_HORIZONTAL_AXIS = "Horizontal_Torpedo";
    const string TORPEDO_VERTICAL_AXIS = "Vertical_Torpedo";

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

    void Start()
    {
        actions = new List<string>(6)
        {
            CharacterAction.JUMP,
            PlayerAction.CHANGE_HAT,
            CharacterAction.JUMP,
        };

        axialActions = new List<string>(1)
        {
        };
    }

    public void Update()
    {
        foreach (string action in actions)
        {
            if (Input.GetButtonDown(action))
            {
                BeginAction(action);
            }
            if (Input.GetButtonUp(action))
            {
                EndAction(action);
            }
        }
        foreach (string axialAction in axialActions)
        {
            AxialAction(axialAction, Input.GetAxis(axialAction));
        }
    }


    public void FixedUpdate()
    {
        move.Set(
            Input.GetAxis(HORIZONTAL_AXIS), 
            Input.GetAxis(VERTICAL_AXIS));
        aim.Set(
            Input.GetAxis(TORPEDO_HORIZONTAL_AXIS),
            Input.GetAxis(TORPEDO_VERTICAL_AXIS));
        
        BiaxialAction(PlayerAction.ROTATE_VIEW, aim.x, aim.y);

        if (View != null)
        {
            aim = Vector2.up.Rotate(-View.Angle);
            move = move.Rotate(-View.Angle);
        }

        BiaxialAction(CharacterAction.MOVE, move.x, move.y);
    }
}
