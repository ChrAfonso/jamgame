using UnityEngine;
using System.Collections;

public class musicManagerScript : MonoBehaviour {

	public AudioClip[] songs;

	// Use this for initialization
	void Start () {
		audio.clip = songs[0];
		audio.Play ();
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
