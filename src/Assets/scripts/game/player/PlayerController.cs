using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {
  public AudioClip fxCollectFruit;
  public AudioClip fxFly;
  public AudioClip fxDestroy;

  public KeyCode keyLeft = KeyCode.LeftArrow;
  public KeyCode keyRight = KeyCode.RightArrow;

  public enum controlState { GAME, FLYING, GAMEOVER };
  public controlState currentState { get; private set; }

  public enum obstacle { SLIPPERY, STICKY };
  public const float SPEED_SLIPPERY = 7;
  public const float SPEED_STICKY = 2;

  public float Speed { get; set; }

  public Vector3 Direction { get; set; }
  private float DDirection;
  public float DirectionChangeSpeed { get; set; }

  private float SpeedDefault = 4;
  private float Acceleration = 2;

  private float SlippingTimer = -1;
  private float SlippingDuration = 2000;

  private float FlyTimer = 0;
  private float FlyDuration = 2000;

  // Use this for initialization
  public void Start () {
    currentState = controlState.GAME;

    Speed = 0;
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
      case controlState.GAMEOVER:
        // TEMP instant reset
        setControlState(controlState.GAME);
        transform.position = OriginalPosition;
        break;
    }

    transform.position += (Direction * (Speed * Time.deltaTime));
  }

  private void UpdateStateGame() {
    if (Input.GetKeyDown(keyLeft)) {
      DDirection = 1;
    } else if (Input.GetKeyDown(keyRight)) {
      DDirection = -1;
    } else if (Input.GetKeyUp(keyLeft) && DDirection == 1) {
      DDirection = 0;
    } else if (Input.GetKeyUp(keyRight) && DDirection == -1) {
      DDirection = 0;
    }

    float TurnDirection;
    if (SlippingTimer == -1) {
      // normal, no slipping
      TurnDirection = DDirection;
    } else {
      // twist direction during slipping
      TurnDirection = Mathf.Sin(SlippingTimer / SlippingDuration * 4 * Mathf.PI);

      SlippingTimer += Time.deltaTime;
      if (SlippingTimer > SlippingDuration) {
        SlippingTimer = -1;
      }
    }

    Speed = Mathf.Lerp(Speed, SpeedDefault, Time.deltaTime * Acceleration);
    Direction = Quaternion.AngleAxis(TurnDirection * DirectionChangeSpeed * Time.deltaTime, new Vector3(0, 0, 1)) * Direction;
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
        print("yum!");
        CollectFruit(other);

        break;
      case "borders":
        print("Aaaaaaaaaah!");
        setControlState(controlState.FLYING);
        break;
      case "jam":
        print("Wheeeeeeee!");
        UpdateSpeed(obstacle.SLIPPERY);
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
    
  }

  private void CollectFruit(Collider2D fruitCollider) {
    GameObject.Destroy(fruitCollider.gameObject);
		audio.PlayOneShot(fxCollectFruit);
  }

  private void UpdateSpeed(obstacle Obstacle) {
    switch (Obstacle) {
      case obstacle.SLIPPERY:
        Speed = SPEED_SLIPPERY;
        StartSlipping();
        break;
      case obstacle.STICKY:
        Speed = SPEED_STICKY;
        break;
    }
  }

  private void StartSlipping() {
    SlippingTimer = 0;
  }
}
