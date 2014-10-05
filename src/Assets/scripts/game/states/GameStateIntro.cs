using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

  public class GameStateIntro : AbstractState  {

    private GameObject goTitle { get; set; }

    private Rect rectButtonStart { get; set; }

    private Rect rectButtonCredits { get; set; }

    public GameStateIntro(string stateName)
      : base(stateName) {
    }

    public override void OnInitialize() {
      goTitle = GameObject.Find("ui/start");
      goTitle.SetActive(false);

      float buttonHeightStart = Screen.height * 0.20f;
      float buttonHeightCredits  = Screen.height * 0.15f;
      float buttonWidthStart = Screen.width * 0.35f;
      float buttonWidthCredits = Screen.width * 0.25f;
      rectButtonStart = new Rect(Screen.width * 0.5f - buttonWidthStart * 0.5f, Screen.height * 0.65f, buttonWidthStart, buttonHeightStart);
      rectButtonCredits = new Rect(Screen.width * 0.5f - buttonWidthCredits * 0.5f, Screen.height * 0.85f, buttonWidthCredits, buttonHeightCredits);
    }

    public override void OnEnter(object onEnterParams = null) {
      goTitle.SetActive(true);
      GameController.Instance.BreakfastSpawner.Cleanup();
      GameController.Instance.MusicManager.PlayTrack(MusicManager.Theme.Intro);
      GameController.Instance.ResetLives();
    }

    public override void OnLeave() {
      goTitle.SetActive(false);
    }

    public override void OnUpdate() {
      if (Input.GetKeyUp(KeyCode.Space) || Input.GetKeyUp(KeyCode.Return)) {
        StartGame();
      }

      if (Input.GetKeyUp(KeyCode.Escape)) {
        Application.Quit();
      }

      if (Input.GetMouseButtonUp(0)) {
        Vector2 mousePosition = Input.mousePosition;
        mousePosition.y = Screen.height - mousePosition.y;

        if (rectButtonStart.Contains(mousePosition)) {
          StartGame();
        }

        if (rectButtonCredits.Contains(mousePosition)) {
          ShowCredits();
        }
      }
    }

    //public override void OnGUI() {
    //  if (GUI.Button(rectButtonStart, "Start")) {
    //    StartGame();
    //  }
    //  if (GUI.Button(rectButtonCredits, "Credits")) {
    //    ShowCredits();
    //  }
    //}

    private void ShowCredits() {
      GameController.Instance.ChangeState("GameStateCredits");
    }

    private void StartGame() {
      GameController.Instance.ChangeState("GameStateCountdown");
    } 
  }
