using System;
using System.Collections.Generic;
using UnityEngine;

public class BreakfastSpawner : MonoBehaviour {

  public int spawnCount = 3;
  public float SpawnScale = 0.5f;

  public List<GameObject> prefabs;

  private List<GameObject> SpawnedObjects { get; set; }

  private System.Random Random { get; set; }

  public void Awake() {
    SpawnedObjects = new List<GameObject>();
    Random = new System.Random();
  }

  public void Spawn() {
    List<GameObject> availableObjects = new List<GameObject>();
    availableObjects.AddRange(prefabs);

    for (int i = 0; i < spawnCount; i++) {
      if (availableObjects.Count == 0) {
        break;
      }

      GameObject prefab = availableObjects[Random.Next(availableObjects.Count)];
      availableObjects.Remove(prefab);
      SpawnedObjects.Add(SpawnAtRandomPosition(prefab));
    }

  }

  public void Cleanup() {
    SpawnedObjects.ForEach(go => GameObject.Destroy(go));
    SpawnedObjects.Clear();
  }


  private GameObject SpawnAtRandomPosition(GameObject prefab) {
    Playground playground = GameController.Instance.Playground;

    GameObject go = GameObject.Instantiate(prefab) as GameObject;
    go.transform.parent = playground.transform;

    Vector3 position = new Vector3();
    position.x = playground.MinX + UnityEngine.Random.value * (playground.MaxX - playground.MinX);
    position.y = playground.MinY + UnityEngine.Random.value * (playground.MaxY - playground.MinY);
    go.transform.position = position;

    go.transform.localScale = Vector3.one * SpawnScale;

    return go;
  }
}

