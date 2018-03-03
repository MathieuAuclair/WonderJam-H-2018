using UnityEngine;
using ProceduralToolkit.Examples;
using TMPro;

public class PlayerName : MonoBehaviour
{
	string name;
	NameGenerator nameGenerator;
	[SerializeField] TextAsset namesJson;
	[SerializeField] TextMeshPro nameText;

	public void Generate ()
	{
		name = nameGenerator.fullName;
	}

	void Awake ()
	{
		nameGenerator = new NameGenerator (namesJson);
		name = nameGenerator.fullName;
		nameText.text = name;
	}
}
