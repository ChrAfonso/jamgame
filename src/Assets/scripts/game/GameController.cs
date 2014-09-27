using System;
using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class GameController : MonoBehaviour {

  public List<PlayerController> Players { get; private set; }
  public int numPlayers = 2;
  public GameObject PlayerPrefab;

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

  public void CreatePlayers() {
    Vector3 startPosition;
    Vector3 startDirection;
    KeyCode[] keyCodes;

    for (int p = 0; p < numPlayers; p++) {
      Debug.Log("Create Player " + p);

      // dirty!
      switch(p) {
        case 0:
          startPosition = Playground.player1_spawn.position;
          startDirection = Playground.player1_spawn.rotation * Vector3.right;
          keyCodes = new KeyCode[3] { KeyCode.LeftArrow, KeyCode.RightArrow, KeyCode.DownArrow };
          break;
        case 1:
          startPosition = Playground.player2_spawn.position;
          startDirection = Playground.player2_spawn.rotation * Vector3.right;
          keyCodes = new KeyCode[3] { KeyCode.A, KeyCode.D, KeyCode.S };
          break;
        default:
          Debug.LogError("Could not create " + numPlayers + " players!");
          return;
      }

      GameObject player = (GameObject) Instantiate(PlayerPrefab, startPosition, Quaternion.identity);
      PlayerController controller = (PlayerController) player.GetComponent<PlayerController>();
      controller.keyLeft = keyCodes[0];
      controller.keyRight = keyCodes[1];
//    controller.keyJam = keyCodes[2];

      controller.Direction = startDirection;

      Players.Add(controller);
    }
  }

  public void OnPlayerDead() {
    Players.ForEach(player => {
      player.DestroyTrail();
      GameObject.Destroy(player.gameObject);
    });
    Players.Clear();

    ChangeState("GameStateGameOver");
  }
}
