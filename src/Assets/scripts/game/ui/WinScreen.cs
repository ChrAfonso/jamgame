using System;
using System.Collections.Generic;
using UnityEngine;

public class WinScreen : MonoBehaviour {

  public GameObject goPlayer1;
  public GameObject goPlayer2;

  public SpriteRenderer spritePlayer1Phrase;
  public SpriteRenderer spritePlayer2Phrase;

  public List<Sprite> phrases;

  public void ShowWinScreenForPlayer(int index) {
    goPlayer1.SetActive(index == 0);
    goPlayer2.SetActive(index == 1);

    ShowRandomPhrase(index == 0 ? spritePlayer1Phrase : spritePlayer2Phrase);
  }

  private void ShowRandomPhrase(SpriteRenderer renderer) {
    renderer.sprite = phrases[UnityEngine.Random.Range(0, phrases.Count - 1)];
  }
}

