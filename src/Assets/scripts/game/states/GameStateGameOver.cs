using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class GameStateGameOver : AbstractState {

  private GameObject goWin { get; set; }

  public GameStateGameOver(string stateName)
    : base(stateName) {
  }

  public override void OnInitialize() {
    goWin = GameObject.Find("ui/win");
    goWin.SetActive(false);
  }

  public override void OnEnter(object onEnterParams = null) {
    int playerIndex = onEnterParams != null ? (int)onEnterParams : 0;

    goWin.SetActive(true);
    GameObject goWinPlayer0 = goWin.transform.Find("player_1").gameObject;
    GameObject goWinPlayer1 = goWin.transform.Find("player_2").gameObject;

    if (playerIndex == 0) {
      GameController.Instance.MusicManager.PlayTrack(MusicManager.Theme.GameOver1);
      goWinPlayer0.SetActive(true);
      goWinPlayer1.SetActive(false);
    } else {
      GameController.Instance.MusicManager.PlayTrack(MusicManager.Theme.GameOver2);
      goWinPlayer0.SetActive(false);
      goWinPlayer1.SetActive(true);
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
