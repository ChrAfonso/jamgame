using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

  public class GameStateGameOver : AbstractState  {

    public GameStateGameOver(string stateName)
      : base(stateName) {
    }

    public override void OnInitialize() {
     
    }

    public override void OnEnter(object onEnterParams = null) {
      
    }

    public override void OnLeave() {
      GameController.Instance.BreakfastSpawner.Cleanup();
    }

    public override void OnUpdate() {
      if (Input.GetKeyUp(KeyCode.Space) || Input.GetKeyUp(KeyCode.Return)) {
        GameController.Instance.ChangeState("GameStateIntro");
      }
    }
  }
