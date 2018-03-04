using UnityEngine;
using TMPro;

public class PlayerViewCanvas : MonoBehaviour
{
	int _playerId;

	public void SetPlayerId (int playerId)
	{
		_playerId = playerId;
	}

	[SerializeField] TextMeshPro tmName;
	[SerializeField] TextMeshPro tmScore;

	public void SetPlayerName (string name)
	{
		tmName.text = name;
	}

	public void SetPlayerScore (float score)
	{
		tmScore.text = score.ToString ();
	}

	void Update ()
	{
		SetPlayerScore (ScoreBoard.GetScore (_playerId));
	}
}
