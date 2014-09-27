using System;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class MusicManager : MonoBehaviour {

  public AudioClip clipIntro;

  public enum Theme {
    Intro, 
    Countdown,
    Game,
    Credits,
    GameOver,
  }

  public void PlayTrack(Theme theme) {
    switch (theme) { 
      case Theme.Intro:
        audio.clip = clipIntro;
        break;
    }

    audio.loop = true;
    audio.Play();
  }
}
