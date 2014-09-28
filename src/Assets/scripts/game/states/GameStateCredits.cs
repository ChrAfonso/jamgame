using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class GameStateCredits : AbstractState {

  private GameObject goCredits { get; set; }

  public GameStateCredits(string stateName)
    : base(stateName) {
  }

  public override void OnInitialize() {
    goCredits = GameObject.Find("ui/credits");
    goCredits.SetActive(false);
  }

  public override void OnEnter(object onEnterParams = null) {
    GameController.Instance.MusicManager.PlayTrack(MusicManager.Theme.Credits);
    goCredits.SetActive(true);
  }

  public override void OnLeave() {
    goCredits.SetActive(false);
  }

  public override void OnUpdate() {
    if (Input.GetKeyUp(KeyCode.Escape)) {
      GameController.Instance.ChangeState("GameStateIntro");
    }
  }
}
