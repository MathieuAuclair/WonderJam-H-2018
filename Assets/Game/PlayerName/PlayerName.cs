using UnityEngine;
using ProceduralToolkit.Examples;
using TMPro;

public class PlayerName : MonoBehaviour
{
	string _name;
	NameGenerator nameGenerator;
	[SerializedField] TextAsset namesJson;
	[SerializeField] TextMeshPro nameText;

	void GenerateNewName ()
	{
		_name = nameGenerator.fullName;
	}

	void Awake ()
	{
		nameGenerator = new NameGenerator (namesJson);
		_name = nameGenerator.fullName;
		nameText.text = _name;
	}
}
