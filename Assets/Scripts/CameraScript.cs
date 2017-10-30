using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour {

	private GameObject[] players;
	private int current = 0;
	private int prevLength;
	private Vector3 offset;
	// Use this for initialization
	void Start () {
		players = GameObject.FindGameObjectsWithTag ("PlayerTag");
		prevLength = players.Length;
		offset = transform.position - players [0].transform.position;
	}
	
	// Update is called once per frame
	void Update () {
		GameObject[] tempPlayers = GameObject.FindGameObjectsWithTag ("PlayerTag");
		for (int i = 0; i < tempPlayers.Length; i++) {
			Debug.Log (tempPlayers [i].transform.name + " - " + Time.time);
		}
		if (!(prevLength == tempPlayers.Length)) {
			players = tempPlayers;
		}
		if (players.Length > 0) {
			transform.position = players [0].transform.position + offset;
			Debug.Log ("Position: " + transform.position);
		}
	}
}
