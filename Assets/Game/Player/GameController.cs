using UnityEngine;
using System.Collections.Generic;

public class GameController : MonoBehaviour
{
    enum Phase
    {
        REGISTRATION,
        GAME,
        END,
    }

    const string JOIN = "P{0}Jump";

    [Header("Tweakables")]
    [SerializeField][Range(1, 4)] int playerCount = 4;
    [SerializeField] int gameTime = 30;
    
    [Header("Can't Touch This")]
    [SerializeField] Bonfire.Screen screen;
    [SerializeField] PlayerController playerPrefab;
    [SerializeField] CountDown startCountdown;
    [SerializeField] Timer endGameTimer;

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
                currentPhase = Phase.GAME;
                startCountdown.Initiate(3, "DÉTRUISEZ!!!", BeginGame);
            }
        }
    }

    void BeginGame()
    {
        endGameTimer.Initiate(gameTime, "", EndGame);
        GiveControl();
    }

    void EndGame()
    {
        RemoveControl();
        currentPhase = Phase.END;
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

    void RemoveControl()
    {
        foreach (var player in players.Values)
        {
            player.UnsubscribeEverything();
            player.GetComponentInChildren<Robot>().ShutDown();
        }
    }
}
