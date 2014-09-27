using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

  public class GameStateGameOver : AbstractState  {

    private GameObject goWin { get; set; }

    public GameStateGameOver(string stateName)
      : base(stateName) {
    }

    public override void OnInitialize() {
      goWin = GameObject.Find("ui/win");
      goWin.SetActive(false);
    }

    public override void OnEnter(object onEnterParams = null) {
      // TODO: activate player 1 OR player 2 win screen
      goWin.SetActive(true);
    }

    public override void OnLeave() {
      GameController.Instance.BreakfastSpawner.Cleanup();
      goWin.SetActive(false);
    }

    public override void OnUpdate() {
      if (Input.GetKeyUp(KeyCode.Space) || Input.GetKeyUp(KeyCode.Return)) {
        GameController.Instance.ChangeState("GameStateIntro");
      }
    }
  }
