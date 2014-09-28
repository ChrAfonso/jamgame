using System;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class MusicManager : MonoBehaviour {

  public AudioClip clipIntro;
  public AudioClip clipCountdown;
  public AudioClip clipGame1;
  public AudioClip clipGame2;
  public AudioClip clipCredits;
  public AudioClip clipGameOver1;
  public AudioClip clipGameOver2;

  public enum Theme {
    Intro, 
    Countdown,
    Game1,
	Game2,
    Credits,
    GameOver1,
	GameOver2,
  }

  public void PlayTrack(Theme theme) {
    switch (theme) { 

      case Theme.Intro:
            audio.clip = clipIntro;
			audio.loop = true;
            break;
			  
	  case Theme.Countdown:
			audio.clip = clipCountdown;
			break;

	  case Theme.Game1:
			audio.clip = clipGame1;
			break;

	  case Theme.Game2:
			audio.clip = clipGame2;
			break;

	  case Theme.Credits:
			audio.clip = clipCredits;
			break;

	  case Theme.GameOver1:
			audio.clip = clipGameOver1;
			audio.loop = false;
			break;

	  case Theme.GameOver2:
			audio.clip = clipGameOver2;
			audio.loop = false;
			break;
    }

    audio.Play();
  }
}
