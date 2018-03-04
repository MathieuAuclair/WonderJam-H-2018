using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpPicker : MonoBehaviour
{

    [SerializeField]
    public enum Power
    {
        LASER,
        BIG_ARM,
        HEAVY_ARM,
        FAST,
    }

    public GameObject robotMainBodyPart;

    void OnCollisionEnter(Collision other)
    {
        switch (other.transform.tag)
        {
            case "powerUpLazer":
                robotMainBodyPart.GetComponent<Robot>().ActivatePowerUp(Power.LASER, other.gameObject);
                break;
            case "powerUpBigArm":
                robotMainBodyPart.GetComponent<Robot>().ActivatePowerUp(Power.BIG_ARM, other.gameObject);
                break;
            case "powerUpHeavyArm":
                robotMainBodyPart.GetComponent<Robot>().ActivatePowerUp(Power.HEAVY_ARM, other.gameObject);
                break;
            case "powerUpFastBoost":
                robotMainBodyPart.GetComponent<Robot>().ActivatePowerUp(Power.FAST, other.gameObject);
                break;
        }
    }
}
