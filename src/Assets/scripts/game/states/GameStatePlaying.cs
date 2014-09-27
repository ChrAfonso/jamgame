using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

  public class GameStatePlaying : AbstractState  {

    public GameStatePlaying(string stateName)
      : base(stateName) {
    }

    public override void OnInitialize() {
     
    }

    public override void OnEnter(object onEnterParams = null) {
      GameController.Instance.BreakfastSpawner.Spawn();
    }

    public override void OnLeave() {
      
    }

    public override void OnUpdate() {
      
    }
  }
