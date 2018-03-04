using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RobotSound : MonoBehaviour {

    public void WalkSound()
    {
        CrackleAudio.SoundController.PlaySound("robotwalking");
    }
}
