using System;
using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class GameController : MonoBehaviour {

  public List<PlayerController> Players { get; private set; }
  public List<KeyBinding> KeyBindings;
  public List<Transform> SpawnPoints;
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
    for (int p = 0; p < numPlayers; p++) {
      Debug.Log("Create Player " + p + "...");
      
      if(p < SpawnPoints.Count && p < KeyBindings.Count) {
        GameObject player = (GameObject)Instantiate(PlayerPrefab, SpawnPoints[p].position, Quaternion.identity);
        PlayerController controller = (PlayerController)player.GetComponent<PlayerController>();
        controller.keyLeft = KeyBindings[p].keyLeft;
        controller.keyRight = KeyBindings[p].keyRight;
        controller.keyJam = KeyBindings[p].keyJam;

        controller.Direction = SpawnPoints[p].rotation * Vector3.right;

        Players.Add(controller);
      } else {
        Debug.LogError("Not enough SpawnPoints/KeyBindings defined for player " + p +"!");
        return;
      }
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
