using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {
  public AudioClip fxCollectFruit;
  public AudioClip fxFly;
  public AudioClip fxDestroy;
  public AudioClip fxBump;

  public float breakFastPushForce = 100;

  public KeyCode keyLeft = KeyCode.LeftArrow;
  public KeyCode keyRight = KeyCode.RightArrow;

  public enum controlState { GAME, FLYING, GAMEOVER };
  public controlState currentState { get; private set; }

  public enum obstacle { SLIPPERY, STICKY };
  public const float SPEED_SLIPPERY = 7;
  public const float SPEED_STICKY = 2;

  private Vector3 OriginalPosition;
  private float DefaultScale = -1;
  public float Speed { get; set; }

  public Vector3 Direction { get; set; }
  private float DDirection;
  public float DirectionChangeSpeed { get; set; }

  public float SpeedDefault = 4;
  public float Acceleration = 2;
  public float FallSlowdown = 4;

  private float SlippingTimer = -1;
  public float SlippingDuration = 1;

  private float FlyTimer = 0;
  public float FlyDuration = 2;

  // Use this for initialization
  public void Start () {
    if (DefaultScale == -1) {
      DefaultScale = transform.localScale.x;
      OriginalPosition = transform.position;
    } else {
      transform.localScale = Vector3.one * DefaultScale;
      transform.position = OriginalPosition;
    }

  public void Start() {
    currentState = controlState.GAME;

    Speed = 0;
    Direction = new Vector3(1, 0, 0);
    DirectionChangeSpeed = 360;
  }

  public void setControlState(controlState NewState) {
    if(NewState == controlState.GAME) {
      Start(); // TEMP
    } else if (currentState == controlState.GAME && NewState == controlState.FLYING) {
      print("Aaaaaaaaaah!");
      FlyTimer = 0;
      currentState = NewState;
    } else if (NewState == controlState.GAMEOVER) {
      print("GameOver!");
      FlyTimer = 0;
      currentState = NewState;
    }
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
      case controlState.GAMEOVER:
        // TEMP instant reset
        setControlState(controlState.GAME);
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

    float scale = 0.5f * (1 - Mathf.Pow(FlyTimer / FlyDuration, 2));
    transform.localScale = Vector3.one * scale;

    FlyTimer += Time.deltaTime;

    if (FlyTimer > FlyDuration) {
      audio.PlayOneShot(fxFly);
      // TODO hide jar, show broken mess? (visible?)

      setControlState(controlState.GAMEOVER);
    }

    Speed = Mathf.Lerp(Speed, 0, Time.deltaTime * FallSlowdown);
  }

  public void OnTriggerEnter2D(Collider2D other) {
    Debug.Log("OnTriggerEnter2D: " + other.name);
    switch (other.gameObject.tag) {
      case "fruit":
        print("yum!");
        CollectFruit(other);
        break;
      case "borders":
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
