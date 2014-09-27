using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {
  
  public float Speed { get; set; } // TODO: set default speed, if sped up/slowed down by jam, ease back into default speed slowly?
  public Vector3 Direction { get; set; }
  private float DDirection;
  public float DirectionChangeSpeed { get; set; }
  
  // Use this for initialization
  public void Start () {
    Speed = 5;
    Direction = new Vector3(1, 0, 0);
    DirectionChangeSpeed = 200;
  }
  
  // Update is called once per frame
  public void Update () {
    if(Input.GetKeyDown(KeyCode.LeftArrow)) {
      DDirection = -1;
    } else if(Input.GetKeyDown(KeyCode.RightArrow)) {
      DDirection = 1;
    } else if(Input.GetKeyUp(KeyCode.LeftArrow) && DDirection == -1) {
      DDirection = 0;
    } else if (Input.GetKeyUp(KeyCode.RightArrow) && DDirection == 1) {
      DDirection = 0;
    }

    Direction = Quaternion.AngleAxis(DDirection * DirectionChangeSpeed * Time.deltaTime, new Vector3(0, 0, 1)) * Direction;
    transform.position += (Direction * (Speed * Time.deltaTime));
  }
}
