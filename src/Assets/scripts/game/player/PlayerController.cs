using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {
  public AudioClip fxCollectFruit;
  public AudioClip fxFly;
  public AudioClip fxDestroy;
  public AudioClip fxBump;

  public float breakFastPushForce = 100;

  public enum controlState { GAME, FLYING, GAMEOVER };
  public controlState currentState { get; private set; }

  public float Speed { get; set; } // TODO: set default speed, if sped up/slowed down by jam, ease back into default speed slowly?
  public Vector3 Direction { get; set; }
  private float DDirection;
  public float DirectionChangeSpeed { get; set; }

  private float FlyTimer = 0;
  private float FlyDuration = 2000;


  public float jamFillMin;
  public float jamFillMax;
  public Transform transformSpriteJam;
  public float reloadPercentagePerFruit = 0.3f;

  private float jamFillPercentage;
  public float JamFillPercentage {
    get { return jamFillPercentage; }
    set {
      jamFillPercentage = Mathf.Clamp01(value);

      if (transformSpriteJam != null) {
        Vector3 localScale = transformSpriteJam.localScale;
        localScale.y = jamFillMin + (jamFillMax - jamFillMin) * jamFillPercentage;
        transformSpriteJam.localScale = localScale;
      }
    }
  }


  // Use this for initialization
  public void Start() {
    currentState = controlState.GAME;

    Speed = 5;
    Direction = new Vector3(1, 0, 0);
    DirectionChangeSpeed = 200;
    jamFillPercentage = 0;
  }

  public void setControlState(controlState NewState) {
    if (currentState == controlState.GAME && NewState == controlState.FLYING) {
      FlyTimer = 0;
    } else if (currentState == controlState.FLYING && NewState == controlState.GAMEOVER) {
      FlyTimer = 0;
    }

    currentState = NewState;
  }

  // Update is called once per frame
  public void Update() {
    switch (currentState) {
      case controlState.GAME:
        UpdateStateGame();
        break;
      case controlState.FLYING:
        UpdateStateFlying();
        break;
    }

    transform.position += (Direction * (Speed * Time.deltaTime));
  }

  private void UpdateStateGame() {
    if (Input.GetKeyDown(KeyCode.LeftArrow)) {
      DDirection = 1;
    } else if (Input.GetKeyDown(KeyCode.RightArrow)) {
      DDirection = -1;
    } else if (Input.GetKeyUp(KeyCode.LeftArrow) && DDirection == 1) {
      DDirection = 0;
    } else if (Input.GetKeyUp(KeyCode.RightArrow) && DDirection == -1) {
      DDirection = 0;
    }

    Direction = Quaternion.AngleAxis(DDirection * DirectionChangeSpeed * Time.deltaTime, new Vector3(0, 0, 1)) * Direction;
  }

  private void UpdateStateFlying() {
    if (FlyTimer == 0) {
      audio.PlayOneShot(fxFly);
    }

    float scale = 1 - Mathf.Exp(FlyTimer / FlyDuration);
    transform.localScale = Vector3.one * scale;

    FlyTimer += Time.deltaTime;

    if (FlyTimer > FlyDuration) {
      audio.PlayOneShot(fxFly);
      // TODO hide jar, show broken mess? (visible?)

      transform.localScale = Vector3.one; // restore size
      setControlState(controlState.GAMEOVER);
    }
  }

  public void OnTriggerEnter2D(Collider2D other) {
    Debug.Log("OnTriggerEnter2D: " + other.name);
    switch (other.gameObject.tag) {
      case "fruit":
        CollectFruit(other);

        break;
      case "borders":
        // TODO: set control state
        break;
      case "jam":
        // TODO: change speed etc.
        break;
    }
  }

  public void OnCollisionEnter2D(Collision2D collision) {
    Debug.Log("OnCollisionEnter2D: " + collision.gameObject.name);
    switch (collision.gameObject.tag) {
      case "breakfast":
        HandleCollisionWithBreakfast(collision);
        break;
    }
  }

  private void HandleCollisionWithBreakfast(Collision2D other) {


    if (other.contacts.Length > 0) {
      Direction = other.contacts[0].normal;

      Vector2 otherNormal = other.contacts[0].normal;
      Direction = otherNormal;
      other.rigidbody.AddForce(otherNormal * -1f * breakFastPushForce);

      audio.PlayOneShot(fxBump);

    }
  }

  private void CollectFruit(Collider2D fruitCollider) {
    GameObject.Destroy(fruitCollider.gameObject);
    audio.PlayOneShot(fxCollectFruit);

    // reload jam
    JamFillPercentage += reloadPercentagePerFruit;
  }
}
