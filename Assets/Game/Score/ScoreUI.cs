using UnityEngine;
using TMPro;

public class ScoreUI : MonoBehaviour
{
	public int playerID;

	[SerializeField] TextMeshPro scoreText;

	void Update ()
	{
		scoreText.text = "$" + ScoreBoard.GetScore (playerID) + "M";
	}
} 