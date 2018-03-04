using UnityEngine;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;

public class ScoreBoardPlayModeTest {

	[UnityTest]
	public IEnumerator ScoreBoardIsEmptyOnStart ()
	{
		var scores = ScoreBoard.GetScores ();
		Assert.IsEmpty (scores);

		yield return null;
	}

	[UnityTest]
	public IEnumerator ScoreWillReturnsGoodLeadingPlayer ()
	{
		int[] playerIds = { 1, 2, 3, 4 };

		foreach (int playerId in playerIds) {
			ScoreBoard.IncreaseScore (playerId, 5);
		}
		ScoreBoard.IncreaseScore (3, 20);
		var scores = ScoreBoard.GetScores ();
		Assert.IsNotEmpty (scores);
		Assert.AreEqual (ScoreBoard.GetScore (3), 20 + 5);
		Assert.AreEqual (ScoreBoard.GetLeading (), 3);
		var x = ScoreBoard.GetLeadingWithScore ();

		yield return null;
	}
}
