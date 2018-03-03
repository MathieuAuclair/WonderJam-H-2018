using UnityEngine;

public class GameController : MonoBehaviour
{
    [SerializeField] Bonfire.Screen screen;

    void Start()
    {
        screen.Initialize();
        var players = FindObjectsOfType<PlayerController>();
        foreach (var player in players)
        {
            screen.Register(player.transform);
        }
    }

    void Update()
    {
        screen.Update();
    }
}
