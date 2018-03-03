using UnityEngine;
using UnityEngine.UI;
using TMPro;

[RequireComponent (typeof(Text))]
public class ScoreManager : MonoBehaviour
{
	public static int score;

	[SerializeField] TextMeshPro scoreText;

	void Awake ()
	{
		score = 0;
	}

	void Update ()
	{
		scoreText.text = "$" + score + "M";
	}
}