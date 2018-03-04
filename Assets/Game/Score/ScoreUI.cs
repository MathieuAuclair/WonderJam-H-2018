using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ScoreUI : MonoBehaviour
{
	public string playerID;

	[SerializeField] TextMeshPro scoreText;

	void Update ()
	{
		scoreText.text = "$" + ScoreBoard.GetScore(playerID) + "M";
	}
} 