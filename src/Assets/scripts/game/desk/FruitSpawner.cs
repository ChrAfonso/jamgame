using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FruitSpawner : MonoBehaviour {

  public List<GameObject> prefabs;

  public float spawnIntervalMin = 3;
  public float spawnIntervalMax = 6;
  public float destroyDelayMin = 4;
  public float destroyDelayMax = 10;

  private bool spawningEnabled;

  public bool SpawningEnabled {
    get { return spawningEnabled; }
    set {
      if (spawningEnabled == false) {
        spawningEnabled = value;
        StartCoroutine(SpawnFruits());
      } else {
        spawningEnabled = value;
      }
    }
  }

  private System.Random Random { get; set; }

  public void Awake() {
    Random = new System.Random();
  }

  private IEnumerator SpawnFruits() {
    while (SpawningEnabled) {
      float delay = UnityEngine.Random.Range(spawnIntervalMin, spawnIntervalMax);
      yield return new WaitForSeconds(delay);

      if (SpawningEnabled) {
        SpawnFruitAtRandomPosition();
      }
    }
  }

  private void SpawnFruitAtRandomPosition() {
    Playground playground = GameController.Instance.Playground;

    GameObject prefabFruit = prefabs[Random.Next(prefabs.Count)];

    GameObject goFruit = GameObject.Instantiate(prefabFruit) as GameObject;
    goFruit.transform.parent = transform;

    Vector3 position = new Vector3();
    position.x = playground.MinX + UnityEngine.Random.value * (playground.MaxX - playground.MinX);
    position.y = playground.MinY + UnityEngine.Random.value * (playground.MaxY - playground.MinY);
    goFruit.transform.position = position;

    float delay = UnityEngine.Random.Range(spawnIntervalMin, spawnIntervalMax);
    GameObject.Destroy(goFruit, delay);
  }
  

}
