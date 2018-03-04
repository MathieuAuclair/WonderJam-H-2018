using System.Collections.Generic;

public class ScoreBoard : PersistentRAIISingleton<ScoreBoard>
{
	readonly IDictionary<string, int> scores = new Dictionary<string, int> ();
	string maxScoreKey = "";

	public static IDictionary<string, int> GetScores ()
	{
		return Instance._GetScores ();
	}

	IDictionary<string, int> _GetScores ()
	{
		return scores;
	}

	public static void Reset ()
	{
		Instance._Reset ();
	}

	void _Reset ()
	{
		scores.Clear ();
	}

	public static void IncreaseScore (string playerKey, int gain)
	{
		Instance._IncreaseScore (playerKey, gain);
	}

	void _IncreaseScore (string playerKey, int gain)
	{
		if (scores.ContainsKey (playerKey)) {
			scores [playerKey] += gain;
		} else {
			scores [playerKey] = gain;
		}

		if (!scores.ContainsKey (maxScoreKey) || scores [playerKey] > scores [maxScoreKey]) {
			maxScoreKey = playerKey;
		}
	}

	public static string GetLeading ()
	{
		return Instance._GetLeading ();
	}

	string _GetLeading ()
	{
		return maxScoreKey;
	}

	public static int GetScore (string playerKey)
	{
		return Instance._GetScore (playerKey);
	}

	int _GetScore (string playerKey)
	{
		if (!scores.ContainsKey (playerKey)) {
			scores [playerKey] = 0;
		}
		return scores [playerKey];
	}

	public static KeyValuePair<string, int> GetLeadingWithScore ()
	{
		return Instance._GetLeadingWithScore ();
	}

	KeyValuePair<string, int> _GetLeadingWithScore ()
	{
		return new KeyValuePair<string, int> (maxScoreKey, scores [maxScoreKey]);
	}
}
