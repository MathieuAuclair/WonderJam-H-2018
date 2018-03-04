using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeavyArm : RobotActionBase
{
    public override void ActionBegins()
    {
        Rigidbody body = gameObject.GetComponent<Rigidbody>();
        body.mass = body.mass * 5;
    }

    public override void ActionEnds()
    {
        Rigidbody body = gameObject.GetComponent<Rigidbody>();
        body.mass = body.mass * 5;
    }
}
