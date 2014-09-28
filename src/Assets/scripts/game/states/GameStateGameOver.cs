using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class GameStateGameOver : AbstractState {

  private GameObject goWin { get; set; }
  private WinScreen WinScreen { get; set; }

  public GameStateGameOver(string stateName)
    : base(stateName) {
  }

  public override void OnInitialize() {
    goWin = GameObject.Find("ui/win");
    WinScreen = goWin.GetComponent<WinScreen>();
    goWin.SetActive(false);
  }

  public override void OnEnter(object onEnterParams = null) {
    int playerIndex = onEnterParams != null ? (int)onEnterParams : 0;

    goWin.SetActive(true);
    WinScreen.ShowWinScreenForPlayer(playerIndex % 2);

    if (playerIndex % 2 ==  0) {
      GameController.Instance.MusicManager.PlayTrack(MusicManager.Theme.GameOver1);
    } else {
      GameController.Instance.MusicManager.PlayTrack(MusicManager.Theme.GameOver2);
    }
  }

  public override void OnLeave() {
    GameController.Instance.BreakfastSpawner.Cleanup();
    goWin.SetActive(false);
  }

  public override void OnUpdate() {
    if (Input.GetKeyUp(KeyCode.Space) || Input.GetKeyUp(KeyCode.Return) || Input.GetMouseButtonUp(0)) {
      GameController.Instance.ChangeState("GameStateIntro");
    }
  }
}
