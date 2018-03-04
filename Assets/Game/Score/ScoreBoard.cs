using System.Collections.Generic;

public class ScoreBoard : PersistentRAIISingleton<ScoreBoard>
{
    readonly IDictionary<int, int> scores = new Dictionary<int, int>();
    int maxScoreKey = -1;

    public static IDictionary<int, int> GetScores()
    {
        return Instance._GetScores();
    }

    IDictionary<int, int> _GetScores()
    {
        return scores;
    }

    public static void Reset()
    {
        Instance._Reset();
    }

    void _Reset()
    {
        scores.Clear();
    }

    public static void IncreaseScore(int playerKey, int gain)
    {
        Instance._IncreaseScore(playerKey, gain);
    }

    void _IncreaseScore(int playerKey, int gain)
    {
        if (scores.ContainsKey(playerKey))
        {
            scores[playerKey] += gain;
        }
        else
        {
            scores[playerKey] = gain;
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

    public static int GetScore(int playerId)
    {
        return Instance._GetScore(playerId);
    }

    int _GetScore(int playerId)
    {
        if (!scores.ContainsKey(playerId))
        {
            scores[playerId] = 0;
        }
        return scores[playerId];
    }

    public static KeyValuePair<int, int> GetLeadingWithScore()
    {
        return Instance._GetLeadingWithScore();
    }

    KeyValuePair<int, int> _GetLeadingWithScore()
    {
        return new KeyValuePair<int, int>(maxScoreKey, scores[maxScoreKey]);
    }
}
