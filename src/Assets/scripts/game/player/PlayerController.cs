using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {
  public AudioClip fxCollectFruit;

  public enum controlState { GAME, FLYING, GAMEOVER };
  public controlState currentState { get; private set; }
  
  public float Speed { get; set; } // TODO: set default speed, if sped up/slowed down by jam, ease back into default speed slowly?
  public Vector3 Direction { get; set; }
  private float DDirection;
  public float DirectionChangeSpeed { get; set; }

  private float FlyTimer = 0;
  private float FlyDuration = 2000;

  // Use this for initialization
  public void Start () {
    currentState = controlState.GAME;

    Speed = 5;
    Direction = new Vector3(1, 0, 0);
    DirectionChangeSpeed = 200;
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
  public void Update () {
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
      // TODO play fly audio
    }

    float scale = 1 - Mathf.Exp(FlyTimer / FlyDuration);
    transform.localScale = Vector3.one * scale;

    FlyTimer += Time.deltaTime;

    if (FlyTimer > FlyDuration) {
      // TODO play destroy audio
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

  private void CollectFruit(Collider2D fruitCollider) {
    GameObject.Destroy(fruitCollider.gameObject);
		audio.PlayOneShot(fxCollectFruit);
  }
}
