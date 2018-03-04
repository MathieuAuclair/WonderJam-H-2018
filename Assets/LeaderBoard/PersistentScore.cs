using UnityEngine;
using UnityEngine.UI;

public class PersistentScore : MonoBehaviour
{
	Text displayUiText;

	string output = PlayerPrefs.GetString ("Score");

	void Start ()
	{
		displayUiText = gameObject.GetComponent<Text> ();
 
		foreach (int playerId in ScoreBoard.GetScores().Keys) {
			output += playerId + " - " + ScoreBoard.GetScores () [playerId] + "M$\n";
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
