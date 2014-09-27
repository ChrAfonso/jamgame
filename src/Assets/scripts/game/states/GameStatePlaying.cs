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
      GameController.Instance.FruitSpawner.SpawningEnabled = true;
    }

    public override void OnLeave() {
      GameController.Instance.FruitSpawner.SpawningEnabled = false;
    }

    public override void OnUpdate() {
      
    }
  }
