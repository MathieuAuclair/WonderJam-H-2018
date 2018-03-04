using System.Collections.Generic;

public class ScoreBoard : PersistentRAIISingleton<ScoreBoard>
{
	readonly IDictionary<int, float> scores = new Dictionary<int, float> ();
	int maxScoreKey = -1;

	public static IDictionary<int, float> GetScores ()
	{
		return Instance._GetScores ();
	}

	IDictionary<int, float> _GetScores ()
	{
		return scores;
	}

	public static void Reset ()
	{
		Instance._Reset ();
	}

    void _Reset()
    {
        scores.Clear();
    }

	public static void IncreaseScore (int playerKey, float gain)
	{
		Instance._IncreaseScore (playerKey, gain);
	}

	void _IncreaseScore (int playerKey, float gain)
	{
		if (scores.ContainsKey (playerKey)) {
			scores [playerKey] += gain;
		} else {
			scores [playerKey] = gain;
		}

        if (!scores.ContainsKey(maxScoreKey) || scores[playerKey] > scores[maxScoreKey])
        {
            maxScoreKey = playerKey;
        }
    }

    public static int GetLeading()
    {
        return Instance._GetLeading();
    }

    int _GetLeading()
    {
        return maxScoreKey;
    }

	public static float GetScore (int playerId)
	{
		return Instance._GetScore (playerId);
	}

	float _GetScore (int playerId)
	{
		if (!scores.ContainsKey (playerId)) {
			scores [playerId] = 0;
		}
		return scores [playerId];
	}

	public static KeyValuePair<int, float> GetLeadingWithScore ()
	{
		return Instance._GetLeadingWithScore ();
	}

	KeyValuePair<int, float> _GetLeadingWithScore ()
	{
		return new KeyValuePair<int, float> (maxScoreKey, scores [maxScoreKey]);
	}
}
