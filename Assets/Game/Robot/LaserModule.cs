using UnityEngine;

[System.Serializable]
public class LaserModule : CharacterModule
{
    [SerializeField] GameObject laser;

    public void Activate()
    {
        laser.SetActive(true);
    }

    public void Deactivate()
    {
        laser.SetActive(false);
    }
}
