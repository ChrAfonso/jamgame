using System;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class MusicManager : MonoBehaviour {

  public AudioClip clipIntro;
  public AudioClip clipCountdown;
  public AudioClip clipGame;
  public AudioClip clipCredits;
  public AudioClip clipGameOver1;
  public AudioClip clipGameOver2;

  public enum Theme {
    Intro, 
    Countdown,
    Game,
    Credits,
    GameOver1,
	GameOver2,
  }

  public void PlayTrack(Theme theme) {
    switch (theme) { 

      case Theme.Intro:
            audio.clip = clipIntro;
            break;
			  
	  case Theme.Countdown:
			audio.clip = clipCountdown;
			break;

	  case Theme.Game:
			audio.clip = clipGame;
			break;

	  case Theme.Credits:
			audio.clip = clipCredits;
			break;

	  case Theme.GameOver1:
			audio.clip = clipGameOver1;
			break;

	  case Theme.GameOver2:
			audio.clip = clipGameOver2;
			break;
    }

    audio.loop = true;
    audio.Play();
  }
}
