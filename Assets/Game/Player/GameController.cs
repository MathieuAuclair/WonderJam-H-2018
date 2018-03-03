using UnityEngine;
using System.Collections.Generic;

public class GameController : MonoBehaviour
{
    enum Phase
    {
        REGISTRATION,
        COUNTDOWN,
        GAME,
        END,
    }

    const string JOIN = "P{0}Jump";

    [SerializeField] Bonfire.Screen screen;
    [SerializeField] PlayerController playerPrefab;
    [SerializeField] int playerCount = 4;
    [SerializeField] CountDown startCountdown;

    IDictionary<int, PlayerController> players = new Dictionary<int, PlayerController>();

    Phase currentPhase;

    void Start()
    {
        screen.Initialize();
    }

    void Update()
    {
        screen.Update();

        switch (currentPhase)
        {
            case Phase.REGISTRATION:
                UpdateRegistration();
                break;
            case Phase.COUNTDOWN:
                break;
            case Phase.GAME:
                break;
        }
    }

    void UpdateRegistration()
    {
        for (int i = 1; i <= playerCount; i++)
        {
            if (!players.ContainsKey(i))
            {
                if (Input.GetButtonDown(string.Format(JOIN, i)))
                {
                    AddPlayer(i);
                }
            }
            else if (Input.GetButtonDown("StartGame"))
            {
                currentPhase = Phase.COUNTDOWN;
                startCountdown.Initiate(3, "DÉTRUISEZ!!!", StartGame);
            }
        }
    }

    void StartGame()
    {
        GiveControl();
        currentPhase = Phase.GAME;
    }

    void AddPlayer(int playerId)
    {
        var player = Instantiate(playerPrefab);
        player.PlayerId = playerId;
        screen.Register(player.GetComponentInChildren<Character>().transform);
        players.Add(playerId, player);
    }

    void GiveControl()
    {
        foreach (var player in players.Values)
        {
            player.GetComponentInChildren<Character>().SetController(player);
        }
    }
}
