using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// Create an instance of this class from the Assets menu. TextureMapping allows to
/// identify textures by name for dynamic texture changes. It is mainly targetted
/// towards UI uses.
/// </summary>
[CreateAssetMenu]
public class SpriteMapping : ScriptableObject
{
    [System.Serializable]
    class Pair
    {
        [SerializeField] string name;
        [SerializeField] Sprite texture;

        public string Name { get { return name; } }

        public Sprite Texture { get { return texture; } }
    }

    [SerializeField] Pair[] mapping;

    IDictionary<string, Sprite> spritesByName;

    public Sprite this [string index] { get { return spritesByName[index]; } }

    void OnEnable()
    {
        spritesByName = new Dictionary<string, Sprite>();
        foreach (var pair in mapping)
        {
            spritesByName.Add(pair.Name, pair.Texture);
        }
    }
}
