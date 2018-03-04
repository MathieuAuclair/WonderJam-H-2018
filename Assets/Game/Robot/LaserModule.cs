using UnityEngine;

[System.Serializable]
public class LaserModule : CharacterModule
{
    [SerializeField] GameObject laser;
    [SerializeField] float duration;
    [SerializeField] float cooldown;

    float timeLeft;
    float delayLeft;

    public override void FixedUpdate()
    {
        if (timeLeft >= 0)
        {
            timeLeft -= Time.fixedDeltaTime;
        }
        else
        {
            laser.SetActive(false);
            if (delayLeft <= 0)
            {
                delayLeft = cooldown;
                timeLeft = duration;
                timeLeft = duration;
                laser.SetActive(true);
            }
            else
            {
                delayLeft -= Time.deltaTime;
            }
        }

    }
}
