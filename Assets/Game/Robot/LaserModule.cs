using UnityEngine;

[System.Serializable]
public class LaserModule : CharacterModule
{
    const string SOUND = "laser";

    [SerializeField] GameObject laser;
    [SerializeField] float duration;
    [SerializeField] float cooldown;

    float timeLeft;
    float delayLeft;

    public override bool IsEnabled
    {
        get { return base.IsEnabled; }
        set
        {
            base.IsEnabled = value;
            if (!value)
                laser.SetActive(false);
        }
    }

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
                CrackleAudio.SoundController.PlaySound(SOUND);
            }
            else
            {
                delayLeft -= Time.deltaTime;
            }
        }

    }
}
