using System;
using System.Collections.Generic;
using UnityEngine;

public class BreakfastItem : MonoBehaviour {

  public float angularVelocity = 100;

  public float scaleDownSpeed = 0.2f;

  public float destructionDelay = 2.0f;

  private bool IsFalling { get; set; }

  private bool ScalingDown { get; set; }

  private float TimeInTheAir { get; set; }

  public void OnTriggerStay2D(Collider2D other) {

    switch (other.tag) {
      case "borders":
        if (IsFalling == false && IsCenterInTrigger(other)) {
          IsFalling = true;
          StartFalling();
        }
        break;
    }
  }

  private bool IsCenterInTrigger(Collider2D other) {
    return other.OverlapPoint(transform.position);
  }

  private void StartFalling() {
    rigidbody2D.isKinematic = false;
    Playground playground = GameController.Instance.Playground;
    Vector3 currentPosition = transform.position;
    if (currentPosition.y > playground.MaxY && currentPosition.x > playground.MinX && currentPosition.x < playground.MaxX) {
      rigidbody2D.gravityScale = -1;
    } else {
      rigidbody2D.gravityScale = 1;
    }
    rigidbody2D.fixedAngle = false;
    rigidbody2D.angularVelocity = angularVelocity;

    ScalingDown = true;

    GameObject.Destroy(gameObject, destructionDelay);
    TimeInTheAir = 0;
  }

  public void Update() {
    if (ScalingDown) {
      TimeInTheAir += Time.deltaTime;
      float scale = 0.5f * (1 - Mathf.Pow(TimeInTheAir / destructionDelay, 2));
      transform.localScale = Vector3.one * scale;
    }
  }
}
