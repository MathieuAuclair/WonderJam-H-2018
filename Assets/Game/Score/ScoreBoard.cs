using System.Collections.Generic;

public partial class ScoreBoard
{
	readonly IDictionary<string, int> scores = new Dictionary<string, int>();
	private string maxScoreKey = "";

	public IDictionary<string, int> GetScores()
	{
		return scores;
	}

	public void Reset()
	{
		scores.Clear();
	}

	public void IncreaseScore(string id, int gain)
	{
		if (scores.ContainsKey(id))
			scores[id] += gain;
		else
			scores[id] = gain;

		if (scores [id] > scores[maxScoreKey]) {
			maxScoreKey = id;
		}
	}

	public string GetLeading(){
		return maxScoreKey;
	}
}
