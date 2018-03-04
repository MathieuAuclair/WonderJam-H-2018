using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FastBoost : RobotActionBase
{
    public override void ActionBegins()
    {
        Rigidbody body = gameObject.GetComponent<Rigidbody>();
        body.velocity =  body.velocity * 1.5f;
    }

    public override void ActionEnds()
    {
        Rigidbody body = gameObject.GetComponent<Rigidbody>();
        body.velocity =  body.velocity / 1.5f;
    }
}