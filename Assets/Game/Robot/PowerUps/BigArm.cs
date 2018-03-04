using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BigArm : RobotActionBase
{
    public override void ActionBegins()
    {
		gameObject.transform.localScale = gameObject.transform.localScale * 1.4f;
    }

    public override void ActionEnds()
    {
		gameObject.transform.localScale = gameObject.transform.localScale / 1.4f;
    }
}
