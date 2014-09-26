using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

  public class GameStateIntro : AbstractState  {

    public GameStateIntro(string stateName)
      : base(stateName) {
    }

    public override void OnInitialize() {
     
    }

    public override void OnEnter(object onEnterParams = null) {
      
    }

    public override void OnLeave() {
      
    }

    public override void OnUpdate() {
      if (Input.GetKeyUp(KeyCode.Space)) {
        GameController.Instance.ChangeState("GameStatePlaying");
      }
    }
  }
