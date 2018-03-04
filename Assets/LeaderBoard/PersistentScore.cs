using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Linq;

public class PersistentScore : MonoBehaviour
{
    Text displayUiText;

    string output;

    void Start()
    {
		var json = PlayerPrefs.GetString ("ScoreJson1");
		var a = JsonUtility.FromJson<PlayerScores> (json != "" ? json : "{}");

        displayUiText = gameObject.GetComponent<Text>();
		foreach (int playerId in ScoreBoard.GetScores().Keys)
		{
			a.Scores.Add (new PlayerScore (){ Name = ScoreBoard.GetName(playerId), Score = ScoreBoard.GetScore(playerId) });
		}
		foreach (PlayerScore player in a.Scores.OrderByDescending (x => x.Score))
		{
			output += player.Name + " - " + player.Score + "M$\n";
		}

        displayUiText.text = output;
		PlayerPrefs.SetString("ScoreJson1", JsonUtility.ToJson(a));
    }
}

[System.Serializable]
public class PlayerScore
{
    public string Name;
    public float Score;
}
[System.Serializable]
public class PlayerScores
{
	public List<PlayerScore> Scores;
}