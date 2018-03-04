using UnityEngine;
using ProceduralToolkit.Examples;
using TMPro;

public class PlayerName : MonoBehaviour
{
	string _name;
	NameGenerator nameGenerator;
	[SerializeField] TextAsset namesJson;

	void GenerateNewName ()
	{
		_name = nameGenerator.fullName;
	}

	void Awake ()
	{
		nameGenerator = new NameGenerator (namesJson);
		_name = nameGenerator.fullName;
	}

	public string GetName() {
		return _name;
	}
}
