using System;
using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class GameController : MonoBehaviour {

  public List<PlayerController> Players { get; private set; }
  public List<KeyBinding> KeyBindings;
  public List<Transform> SpawnPoints;
  public List<Transform> LifeBars;
  public int numPlayers = 2;
  public GameObject PlayerPrefab;

  public int LivesPerPlayer = 10;
  private List<int> livesLeft;

  public bool showMouse = false;

  public Playground playground;
  public Playground Playground {
    get {
      return playground;
    }
  }

  public BreakfastSpawner breakfastSpawner;
  public BreakfastSpawner BreakfastSpawner {
    get {
      return breakfastSpawner;
    }
  }

  public FruitSpawner fruitSpawner;
  public FruitSpawner FruitSpawner {
    get {
      return fruitSpawner;
    }
  }

  public MusicManager musicManager;
  public MusicManager MusicManager {
    get {
      return musicManager;
    }
  }

  private static GameController instance;
  public static GameController Instance {
    get {
      return instance;
    }
  }

  private StateMachine StateMachine { get; set; }

  public void Awake() {
    instance = this;

    Screen.showCursor = showMouse;

    List<AbstractState> states = new List<AbstractState>();
    states.Add(new GameStateIntro("GameStateIntro"));
    states.Add(new GameStatePlaying("GameStatePlaying"));
    states.Add(new GameStateGameOver("GameStateGameOver"));
    states.Add(new GameStateCredits("GameStateCredits"));

    StateMachine = StateMachine.Create("state_machine", states, "GameStateIntro");

    Players = new List<PlayerController>();
  }

  public void ChangeState(string stateName, object onEnterParams = null) {
    StateMachine.ChangeState(stateName, onEnterParams);
  }

  public void ResetLives() {
    livesLeft = new List<int>();
    for (int p = 0; p < numPlayers; p++) {
      livesLeft.Add(LivesPerPlayer);
    }
  }

  public void CreatePlayers(int index = -1) {
    for (int p = 0; p < numPlayers; p++) {
      Debug.Log("Create Player " + p + "...");
      
      if(p < SpawnPoints.Count && p < KeyBindings.Count) {
        GameObject player = (GameObject)Instantiate(PlayerPrefab, SpawnPoints[p].position, Quaternion.identity);
        PlayerController controller = (PlayerController)player.GetComponent<PlayerController>();
        controller.keyLeft = KeyBindings[p].keyLeft;
        controller.keyRight = KeyBindings[p].keyRight;
        controller.keyJam = KeyBindings[p].keyJam;

        controller.Direction = SpawnPoints[p].rotation * Vector3.right;

        controller.playerIndex = p;
        Players.Add(controller);
      } else {
        Debug.LogError("Not enough SpawnPoints/KeyBindings defined for player " + p +"!");
        return;
      }
    }
  }

  public void UpdateLifeBars() {
    for (int p = 0; p < numPlayers; p++) {
      Transform lifeBar = LifeBars[p];
      LifeBarController controller = (LifeBarController) lifeBar.GetComponent<LifeBarController>();
      controller.UpdateLives(LivesPerPlayer, livesLeft[p]);
    }
  }

  public void OnPlayerDead(int playerIndex) {
    livesLeft[playerIndex]--;

    List<int> playersLeft = PlayersLeft();
    if (playersLeft.Count < 2) {
      Players.ForEach(player =>
      {
        player.DestroyTrail();
        GameObject.Destroy(player.gameObject);
      });
      Players.Clear();

      UpdateLifeBars();
      ChangeState("GameStateGameOver", playersLeft[0]);
    } else {
      UpdateLifeBars();
      Players[playerIndex].Start(); // restart
    }
  }

  private List<int> PlayersLeft() {
    List<int> result = new List<int>();
    for (int p = 0; p < numPlayers; p++) {
      if (livesLeft[p] > 0) {
        result.Add(p);
      }
    }

    return result;
  }
}
