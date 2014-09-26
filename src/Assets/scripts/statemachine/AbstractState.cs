using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public abstract	class AbstractState {

  public string StateName { get; private set; }

  public AbstractState(string stateName) {
    StateName = stateName;

    OnInitialize();
  }

  public abstract void OnInitialize();
  public abstract void OnEnter(object onEnterParams = null);
  public abstract void OnLeave();
  public abstract void OnUpdate();
  public virtual void OnGUI() { }
}
