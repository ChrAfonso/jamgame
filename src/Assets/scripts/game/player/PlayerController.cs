using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {
  public AudioClip fxCollectFruit;
  public AudioClip fxFly;
  public AudioClip fxDestroy;
  public AudioClip fxBump;
  public AudioClip fxSlip;

  public AudioSource runLoop;
  public AudioSource slideLoop;

  public float breakFastPushForce = 100;
  public float playerPushForce = 200;
  public float playerReactivationDelayAfterPush = 0.5f;
  public float gameOverAngularVelocity = 100.0f;

  public KeyCode keyLeft = KeyCode.LeftArrow;
  public KeyCode keyRight = KeyCode.RightArrow;
  public KeyCode keyJam = KeyCode.DownArrow;

  public enum controlState { GAME, FLYING, GAMEOVER };
  public controlState currentState { get; private set; }

  public enum obstacle { SLIPPERY, STICKY };
  public float SpeedupOnSlip = 1.5f;
  public float Speed_Slippery = 7;
  public float Speed_Sticky = 2;

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

  public float jamFillMin;
  public float jamFillMax;
  public Transform transformSpriteJam;

  [Range(0, 1)]
  public float reloadPercentagePerFruit = 0.3f;

  [Range(0, 1)]
  public float jamConsumptionPerSecond = 0.1f;
  public float jamTrailDeactivationDelay = 2.0f;

  [Range(0, 1)]
  public float initialJamFillPercentage = 0;

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

  public TrailRendererWith2DCollider jamTrail;
  public TrailRendererWith2DCollider JamTrail {
    get {
      return jamTrail;
    }
  }
  public float jamTrailOffset = 0.5f;

  public bool IsDead { get; private set; }

  private bool IsSpittingJam { get; set; }

  // Use this for initialization
  public void Start() {
    if (DefaultScale == -1) {
      DefaultScale = transform.localScale.x;
      OriginalPosition = transform.position;
    } else {
      transform.localScale = Vector3.one * DefaultScale;
      transform.position = OriginalPosition;
    }

    currentState = controlState.GAME;

    Speed = 0;
    DirectionChangeSpeed = 360;
    DDirection = 0;
    SlippingTimer = -1;
    FlyTimer = 0;
    JamFillPercentage = initialJamFillPercentage;
    IsDead = false;
    JamTrail.pausing = true;

    runLoop.Play();
    slideLoop.Stop();
  }

  public void setControlState(controlState NewState) {
    if (NewState == controlState.GAME) {
      Start();
    } else if (currentState == controlState.GAME && NewState == controlState.FLYING) {
      Debug.Log("Aaaaaaaaaah!");
      JamTrail.pausing = true;
      StartFalling();
      runLoop.Stop();
      slideLoop.Stop();
      FlyTimer = 0;
      currentState = NewState;
    } else if (NewState == controlState.GAMEOVER) {
      Debug.Log("GameOver!");
      runLoop.Stop();
      slideLoop.Stop();
      FlyTimer = 0;
      currentState = NewState;
    }
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
    rigidbody2D.angularVelocity = gameOverAngularVelocity;
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
        IsDead = true;
        GameController.Instance.OnPlayerDead();
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
      float SlippingAmount = 1 - (SlippingTimer / SlippingDuration);
      float Correction = -0.045f;
      TurnDirection = (SlippingAmount * (Mathf.Sin((1 - SlippingAmount) * 16 * Mathf.PI) + Correction))
        + ((1 - SlippingAmount) * DDirection);

      SlippingTimer += Time.deltaTime;
      if (SlippingTimer > SlippingDuration) {
        SlippingTimer = -1;

        runLoop.Play();
        slideLoop.Stop();
      }
    }

    Speed = Mathf.Lerp(Speed, SpeedDefault, Time.deltaTime * Acceleration);
    Direction = Quaternion.AngleAxis(TurnDirection * DirectionChangeSpeed * Time.deltaTime, new Vector3(0, 0, 1)) * Direction;

    if (Direction.x > 0) {
      transform.rotation = Quaternion.Euler(0, 0, 0);
    } else {
      transform.rotation = Quaternion.Euler(0, 180, 0);
    }

    if (JamTrail != null) {
      JamTrail.transform.position = transform.position + Direction * -1.0f * jamTrailOffset;
    }

    if (IsSpittingJam == false && Input.GetKeyUp(keyJam)) {
      IsSpittingJam = true;
      JamTrail.CreateTrail();
      JamTrail.pausing = false;
      StartCoroutine(StopJamTrailWithDelay(jamTrailDeactivationDelay));
    }
  }

  private IEnumerator StopJamTrailWithDelay(float jamTrailDeactivationDelay) {
    while (jamTrailDeactivationDelay > 0 && JamFillPercentage > 0) {
      jamTrailDeactivationDelay -= Time.deltaTime;
      JamFillPercentage -= jamConsumptionPerSecond * Time.deltaTime;
      yield return null;
    }
        
    JamTrail.pausing = true;
    IsSpittingJam = false;
  }

  private void UpdateStateFlying() {
    if (FlyTimer == 0) {
      audio.PlayOneShot(fxFly);
    }

    float scale = 0.5f * (1 - Mathf.Pow(FlyTimer / FlyDuration, 2));
    transform.localScale = Vector3.one * scale;

    FlyTimer += Time.deltaTime;

    if (FlyTimer > FlyDuration) {
      audio.PlayOneShot(fxDestroy);
      // TODO hide jar, show broken mess? (visible?)

      setControlState(controlState.GAMEOVER);
    }

    Speed = Mathf.Lerp(Speed, 0, Time.deltaTime * FallSlowdown);
  }

  public void OnTriggerEnter2D(Collider2D other) {
    Debug.Log("OnTriggerEnter2D: " + other.name);
    switch (other.gameObject.tag) {
      case "fruit":
        Debug.Log("yum!");
        CollectFruit(other);
        break;
      case "borders":
        
        setControlState(controlState.FLYING);
        break;
      case "jam":
        Debug.Log("Wheeeeeeee!");
        runLoop.Stop();
        slideLoop.Play();
        UpdateSpeed(obstacle.SLIPPERY);
        break;
      case "player":
        HandleCollisionWithPlayer(other);
        break;
    }
  }

  private void HandleCollisionWithPlayer(Collider2D other) {
    PlayerController otherPlayer = other.gameObject.GetComponent<PlayerController>();
    //otherPlayer.StartSlipping();
    //StartSlipping();
    Vector3 changedDirection = other.transform.position - transform.position;
    //changedDirection = changedDirection.normalized;
    //Direction = changedDirection;
    //otherPlayer.Direction = changedDirection * -1.0f;

    
    StartCoroutine(PushPlayer(changedDirection, playerPushForce, playerReactivationDelayAfterPush));
    otherPlayer.StartCoroutine(PushPlayer(changedDirection * -1f, playerPushForce, playerReactivationDelayAfterPush));

    // TODO: audio - play player vs. player collision fx
  }

  private IEnumerator PushPlayer(Vector2 direction, float force, float reactivationDelay) {
    rigidbody2D.isKinematic = false;
    rigidbody2D.AddForce(direction * force);
    Direction = direction;
    yield return new WaitForSeconds(reactivationDelay);
    rigidbody2D.isKinematic = true;
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
      Speed = 0;
      other.rigidbody.AddForce(otherNormal * -1f * breakFastPushForce);

      audio.PlayOneShot(fxBump);

    }
  }

  private void CollectFruit(Collider2D fruitCollider) {
    GameObject.Destroy(fruitCollider.gameObject);
    audio.PlayOneShot(fxCollectFruit);
    JamFillPercentage += reloadPercentagePerFruit;
  }

  private void UpdateSpeed(obstacle Obstacle) {
    switch (Obstacle) {
      case obstacle.SLIPPERY:
        Speed *= SpeedupOnSlip;
        StartSlipping();
        break;
      case obstacle.STICKY:
        Speed = Speed_Sticky;
        break;
    }
  }

  private void StartSlipping() {
    SlippingTimer = 0;
  }

  public void DestroyTrail() {
    GameObject.Destroy(JamTrail.TrailObject);
  }
}
