using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Base class for anything that controls a character, be it an AI or player controller.
/// </summary>
public class InputController : MonoBehaviour
{
    readonly IDictionary<string, Action> instantActionsBegin = 
        new Dictionary<string, Action>(8);
    readonly IDictionary<string, Action> instantActionsEnd = 
        new Dictionary<string, Action>(8);
    readonly IDictionary<string, Action<float, float>> biaxialActions = 
        new Dictionary<string, Action<float, float>>(4);
    readonly IDictionary<string, Action<float>> analogicActions = 
        new Dictionary<string, Action<float>>(4);
    readonly IDictionary<string, Sensor> sharedSensors =
        new Dictionary<string, Sensor>(8);

    public void Subscribe(string action, Action onBegin, Action onEnd = null)
    {
        if (onBegin != null)
        {
            instantActionsBegin.Add(action, onBegin);
        }

        if (onEnd != null)
        {
            instantActionsEnd.Add(action, onEnd);
        }
    }

    public void Subscribe(string action, Action<float> reaction)
    {
        analogicActions.Add(action, reaction);
    }

    public void Subscribe(string action, Action<float, float> reaction)
    {
        biaxialActions.Add(action, reaction);
    }

    public void ShareSensor(string action, Sensor sharedSensor)
    {
        sharedSensors.Add(action, sharedSensor);
    }

    protected Sensor GetSensor(string action)
    {
        return sharedSensors[action];
    }

    public void UnsubscribeEverything()
    {
        instantActionsBegin.Clear();
        instantActionsEnd.Clear();
        biaxialActions.Clear();
        analogicActions.Clear();
    }

    public void Unsubscribe(string action)
    {
        instantActionsBegin.Remove(action);
        instantActionsEnd.Remove(action);
        biaxialActions.Remove(action);
        analogicActions.Remove(action);
    }

    protected void BeginAction(string action)
    {
        if (instantActionsBegin.ContainsKey(action))
        {
            instantActionsBegin[action]();
        }
    }

    protected void EndAction(string action)
    {
        if (instantActionsEnd.ContainsKey(action))
        {
            instantActionsEnd[action]();
        }
    }

    protected void BiaxialAction(string action, float a, float b)
    {
        if (biaxialActions.ContainsKey(action))
        {
            biaxialActions[action](a, b);
        }
    }

    protected void AxialAction(string action, float m)
    {
        if (analogicActions.ContainsKey(action))
        {
            analogicActions[action](m);
        }
    }
}
