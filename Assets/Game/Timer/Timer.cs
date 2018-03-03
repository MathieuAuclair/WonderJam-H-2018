using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Timer : MonoBehaviour
{
    [SerializeField]
    float timeLeft = 30f;

    // Update is called once per frame
    void Update()
    {
        if (timeLeft > 0)
        {
            timeLeft -= Time.deltaTime;
            timeLeft = Mathf.Round(timeLeft * 100f) / 100f;
            string timeLeftString = timeLeft.ToString();
            timeLeftString = timeLeftString.PadRight(5, '0');
            GetComponent<Text>().text = timeLeftString;
        }
        else
        {
            timeLeft = 0;
            timeLeft = Mathf.Round(timeLeft * 100f) / 100f;
            GetComponent<Text>().text = "" + timeLeft;
            Debug.Log("EndGame");
            SceneManager.LoadScene(0);
        }

    }
}
