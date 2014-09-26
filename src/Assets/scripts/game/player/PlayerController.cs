using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {

  public float DDirection { get; private set; }
  public GameObject go { get; set; }
  
	// Use this for initialization
	public void Start () {
	  
	}
	
	// Update is called once per frame
	public void Update () {
    if(Input.GetKeyDown(KeyCode.LeftArrow)) {
      DDirection = -1;
    } else if(Input.GetKeyDown(KeyCode.RightArrow)) {
      DDirection = 1;
    }

    if(Input.GetKeyUp(KeyCode.LeftArrow)) {
      DDirection = 0;
    } else if(Input.GetKeyUp(KeyCode.RightArrow)) {
      DDirection = 0;
    }

    transform.position = new Vector3(transform.position.x + DDirection * Time.deltaTime, transform.position.y, transform.position.z);
	}
}
