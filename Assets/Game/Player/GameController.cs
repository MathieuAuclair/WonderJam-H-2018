using UnityEngine;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

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
    [SerializeField] ColorSwatch[] swatches;
    [SerializeField] Camera periphericView;
    [SerializeField] IntroScreen intro;
    [SerializeField] GameObject leaderBoard;

    IDictionary<int, PlayerController> players = new Dictionary<int, PlayerController>();

    Phase currentPhase;

    void Start()
    {
        screen = Instantiate(screen);
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
            case Phase.END:
                UpdateEndGame();
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
                    intro.HideLogo();
                    AddPlayer(i);
                    periphericView.gameObject.SetActive(false);
                }
            }
            else if (Input.GetButtonDown("StartGame"))
            {
                currentPhase = Phase.GAME;
                startCountdown.Initiate(3, "DÉTRUISEZ!!!", BeginGame);
                intro.HideInstructions();
            }
        }
    }

    void BeginGame()
    {
        CrackleAudio.SoundController.PlayMusic("main", 0.5f);
        endGameTimer.Initiate(gameTime, EndGame);
        GiveControl();
    }

    void EndGame()
    {
        RemoveControl();
        currentPhase = Phase.END;
        Invoke("ShowLeaderBoard", 3);
    }

    void ShowLeaderBoard()
    {
        periphericView.gameObject.SetActive(true);
        screen.CleanUp();
        leaderBoard.SetActive(true);
    }

    void UpdateEndGame()
    {
        if (Input.GetButtonDown("StartGame"))
		{
			ScoreBoard.Reset ();
            SceneManager.LoadScene(0);
        }
    }

    void AddPlayer(int playerId)
    {
        var player = Instantiate(playerPrefab);
        player.PlayerId = playerId;
        screen.Register(player.GetComponentInChildren<Character>().transform);
        players.Add(playerId, player);
        if (swatches.Length <= playerId)
        {
            player.GetComponent<Painter>().ApplySwatch(swatches[playerId - 1]);
        }
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
        CrackleAudio.SoundController.PlaySound("shutdown");
        foreach (var player in players.Values)
        {
            player.UnsubscribeEverything();
            player.GetComponentInChildren<Robot>().ShutDown();
        }
    }
}
