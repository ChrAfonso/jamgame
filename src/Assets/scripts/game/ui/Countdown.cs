using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class Countdown : MonoBehaviour {

  public List<GameObject> countdownSprites;

  public float delay = 1;

  public AudioClip clipCountdown;

  public static bool IsDone { get; private set; }

  public void Awake() {
    DeactivateAll();
  }

  public void StartCountdown() {
    IsDone = false;
    StartCoroutine(DoCountdown());
  }

  private IEnumerator DoCountdown() {
    audio.PlayOneShot(clipCountdown);

    // dirty, dirty
    for (int i = 0; i < 3; i++) {
      DeactivateAll();
      countdownSprites[i].SetActive(true);
      yield return new WaitForSeconds(delay);
    }
    IsDone = true;
    DeactivateAll();
  }

  private void DeactivateAll() {
    countdownSprites.ForEach(go => go.SetActive(false));
  }
}
