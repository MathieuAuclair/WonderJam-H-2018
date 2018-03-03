using UnityEngine;

[CreateAssetMenu]
public class MeshPicker : ScriptableObject
{
    [SerializeField] Mesh[] meshes;

    public Mesh Pick()
    {
        int index = Random.Range(0, meshes.Length);
        return meshes[index];
    }
}
