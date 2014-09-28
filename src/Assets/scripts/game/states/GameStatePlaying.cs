using System;
using System.Collections.Generic;
using UnityEngine;

public class GameStatePlaying : AbstractState {

  public GameStatePlaying(string stateName)
    : base(stateName) {
  }

  public override void OnInitialize() {

  }

  public override void OnEnter(object onEnterParams = null) {
    GameController.Instance.CreatePlayers();
    GameController.Instance.UpdateLifeBars();
    GameController.Instance.BreakfastSpawner.Spawn();
    GameController.Instance.FruitSpawner.SpawningEnabled = true;
	GameController.Instance.MusicManager.PlayTrack(MusicManager.Theme.Game1);
  }

  public override void OnLeave() {
    GameController.Instance.FruitSpawner.SpawningEnabled = false;
  }

  public override void OnUpdate() {
    if (Input.GetKeyUp(KeyCode.Escape)) {
      GameController.Instance.ChangeState("GameStateIntro");
    }
  }
}
