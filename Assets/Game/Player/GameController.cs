using UnityEngine;
using System.Collections.Generic;

public class GameController : MonoBehaviour
{
    const string JOIN = "P{0}Jump";

    [SerializeField] Bonfire.Screen screen;
    [SerializeField] PlayerController playerPrefab;

    IDictionary<int, PlayerController> players = new Dictionary<int, PlayerController>();
    int playerCount;

    void Start()
    {
        screen.Initialize();
    }

    void Update()
    {
        screen.Update();
        for (int i = 1; i <= 4; i++)
        {
            if (!players.ContainsKey(i) && Input.GetButtonDown(string.Format(JOIN, i)))
            {
                AddPlayer(i);
            }
        }
    }

    void AddPlayer(int playerId)
    {
        var player = Instantiate(playerPrefab);
        player.PlayerId = playerId;
        player.GetComponent<Character>().SetController(player);
        screen.Register(player.transform);
        players.Add(playerId, player);
    }
}
