using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class GameStateCountdown : AbstractState {

  private GameObject goCountdown { get; set; }

  private Countdown Countdown { get; set; }

  public GameStateCountdown(string stateName) : 
    base(stateName) {
  }

  public override void OnInitialize() {
    goCountdown = GameObject.Find("ui/countdown");
    Countdown = goCountdown.GetComponent<Countdown>();
  }

  public override void OnEnter(object onEnterParams = null) {
    Countdown.StartCountdown();
  }

  public override void OnLeave() {

  }

  public override void OnUpdate() {
    if (Countdown.IsDone) {
      GameController.Instance.ChangeState("GameStatePlaying");
    }
  }
}
