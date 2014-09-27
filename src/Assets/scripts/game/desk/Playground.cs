using System;
using System.Collections.Generic;
using UnityEngine;

public class Playground : MonoBehaviour {

  public Transform min;
  public Transform max;

  public Transform player1_spawn;
  public Transform player2_spawn;

  public float MinX {
    get {
      return min.position.x;
    }
  }

  public float MinY {
    get {
      return min.position.y;
    }
  }

  public float MaxX {
    get {
      return max.position.x;
    }
  }

  public float MaxY {
    get {
      return max.position.y;
    }
  }
}

