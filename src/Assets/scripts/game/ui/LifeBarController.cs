using UnityEngine;
using System.Collections;
using System;

[Serializable]
public class LifeBarController : MonoBehaviour {

  public GameObject Filled;
  public GameObject Empty;

  public float Distance = 0.2f;

  public void UpdateLives(int total, int left) {
    Debug.Log("Updating life bar (" + left + "/" + total + ")");
    ClearLifeBar();

    GameObject lifemarker;
    for (int l = 0; l < total; l++) {
      Vector3 position = transform.position + (transform.rotation * new Vector3(Distance * l, 0, 0));
      if(l < left) {
        lifemarker = (GameObject) Instantiate(Filled, position, Quaternion.identity);
      } else {
        lifemarker = (GameObject) Instantiate(Empty, position, Quaternion.identity);
      }

      lifemarker.transform.parent = transform;
    }
  }

  private void ClearLifeBar() {
    for (int c = 0; c < transform.childCount; c++) {
      Destroy(transform.GetChild(c).gameObject);
    }
  }
}
