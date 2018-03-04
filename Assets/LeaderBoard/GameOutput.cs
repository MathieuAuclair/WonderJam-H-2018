using UnityEngine;
using UnityEngine.UI;

public class GameOutput : MonoBehaviour
{
	Text displayUiText;

	string output = PlayerPrefs.GetString ("Score");

	void Start ()
	{
		displayUiText = gameObject.GetComponent<Text> ();
 
		foreach (string tagName in ScoreBoard.GetScores().Keys) {
			output += tagName + " - " + ScoreBoard.GetScores () [tagName] + "M$\n";
		}

		displayUiText.text = output;
		PlayerPrefs.SetString ("Score", output);
	}
}

public class PlayerScore
{
	public string Name;
	public int Score;
}
