﻿using System;
using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class GameController : MonoBehaviour {

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
  }

  public void ChangeState(string stateName, object onEnterParams = null) {
    StateMachine.ChangeState(stateName, onEnterParams);
  }
}
