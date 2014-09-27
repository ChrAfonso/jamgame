using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

  public class GameStateCredits : AbstractState  {

    public GameStateCredits(string stateName)
      : base(stateName) {
    }

    public override void OnInitialize() {
     
    }

    public override void OnEnter(object onEnterParams = null) {

		GameController.Instance.MusicManager.PlayTrack(MusicManager.Theme.Credits);
      
    }

    public override void OnLeave() {
      
    }

    public override void OnUpdate() {
      
    }
  }
