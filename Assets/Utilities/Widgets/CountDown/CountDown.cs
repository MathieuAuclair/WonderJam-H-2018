using UnityEngine;
using System;
using UnityEngine.UI;

[RequireComponent(typeof(Animation))]
[RequireComponent(typeof(Text))]
public class CountDown : MonoBehaviour
{
    string endMessage;
    float timeLeft;
    int nextDisplayedSecond;
    bool running;
    Action onEnd;

    void Start()
    {
        GetComponent<Text>().text = string.Empty;
    }

    public void Initiate(int duration, string endMessage, Action onEnd = null)
    {
        this.endMessage = endMessage;
        timeLeft = duration;
        nextDisplayedSecond = duration;
        running = true;
        this.onEnd = onEnd ?? delegate
        {
        };
    }

    public void Update()
    {
        if (running)
        {
            timeLeft -= Time.deltaTime;
            if (timeLeft <= 0)
            {
                End();
            }
            else if (timeLeft <= nextDisplayedSecond)
            {
                ShowNextSecond();
            }
        }
    }

    void End()
    {
        running = false;
        GetComponent<Text>().text = endMessage;
        GetComponent<Animation>().Play();
        //CrackleAudio.SoundController.PlaySound("CountdownEnd");
        onEnd();
    }

    void ShowNextSecond()
    {
        GetComponent<Text>().text = nextDisplayedSecond.ToString();
        nextDisplayedSecond--;
        GetComponent<Animation>().Play();
        //CrackleAudio.SoundController.PlaySound("CountdownSecond");
    }
}
