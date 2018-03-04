using UnityEngine;
using UnityEngine.UI;
using System;

public class Timer : MonoBehaviour
{
    [SerializeField] Text display;

    float timeLeft;
    bool running;
    Action onEnd;

    void Start()
    {
        GetComponent<Text>().text = string.Empty;
    }

    public void Initiate(int duration, Action onEnd = null)
    {
        timeLeft = duration;
        running = true;
        this.onEnd = onEnd ?? delegate
        {
        };
    }

    void Update()
    {
        if (running)
        {
            timeLeft -= Time.deltaTime;
            if (timeLeft <= 0)
            {
                End();
            }
            else
            {
                UpdateTimer();
            }
        }
    }

    void End()
    {
        running = false;
        display.text = "0";
        onEnd();
    }

    void UpdateTimer()
    {
        float displayedTime = (Mathf.Round(timeLeft * 100) * 0.01f);
        display.text = displayedTime.ToString();
    }
}
