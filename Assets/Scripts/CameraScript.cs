using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour {

	private GameObject[] players;
	private int prevLength;
	private Vector3 offset;
	private Camera cam;
	// Use this for initialization
	void Start () {
		players = GameObject.FindGameObjectsWithTag ("PlayerTag");
		prevLength = players.Length;
        offset = transform.position - players[0].transform.position;
		cam = GetComponent<Camera>();
    }
	
	// Update is called once per frame
	void Update () {
		GameObject[] tempPlayers = GameObject.FindGameObjectsWithTag ("PlayerTag");
		if (prevLength != tempPlayers.Length) {
			players = tempPlayers;
            prevLength = players.Length;
		}
		Debug.Log ("TempPlayers=" + tempPlayers.ToString());
		if (players.Length > 1) {
			FixedCamera ();
		} else if (players.Length == 0) {
			Debug.Log("Doneso.");
		}
        else {
            transform.position = players[0].transform.position + offset;
			cam.orthographicSize = 7f;
        }
	}

    public void FixedCamera() {
        float zoom = 1.5f;
        float followTime = 0.8f;
        Vector3 middle = new Vector3();
        foreach(GameObject go in players) {
            middle += go.transform.position;
        }
        middle = middle / (float)prevLength;
        float distance = 0;
        for(int i = 0; i < prevLength; i++) {
            for(int j = i+1; j < prevLength; j++) {
                float d = Vector3.Distance(players[i].transform.position, players[j].transform.position);
                if(d > distance) {
                    distance = d;
                }
            }
        }
        if (distance < 10) {
            distance = 10f;
        }
        if (distance > 30) {
            distance = 30f;
        }
        Vector3 cameraDest = middle - cam.transform.forward * distance * zoom;
        if (cam.orthographic) {
            cam.orthographicSize = distance;
        }
        cam.transform.position = Vector3.Slerp(cam.transform.position, cameraDest, followTime);
        if (Vector3.Distance(cameraDest, cam.transform.position) <= 0.05f) {
            cam.transform.position = cameraDest;
        }
    }
}
