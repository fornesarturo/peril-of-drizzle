using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

public class FlameController : MonoBehaviour {

	public GameObject[] players = new GameObject[4];
	private static int speed = 5;
	private static int life = 3;
    private Transform closest;
    private Transform prevClosest;
	// Use this for initialization
	void Start () {
        players = GameObject.FindGameObjectsWithTag("PlayerTag");
	}
	
	// Update is called once per frame
	void Update () {
        float maxDistance = players.Min(x => (x.transform.position - this.transform.position).sqrMagnitude);
        Transform closest = players.First(x => (x.transform.position - this.transform.position).sqrMagnitude == maxDistance).transform;

        if(closest != prevClosest) {
            prevClosest = closest;
        }
        
		if (Vector2.Distance (transform.position, prevClosest.position) > 1f) {
			transform.position = Vector2.MoveTowards (transform.position, prevClosest.position, speed * Time.deltaTime);
		} else {
			Attack ();
		}
	}
		
	void OnTriggerEnter2D(Collider2D c) {
		Debug.Log ("Collision");
		switch (c.transform.tag) {
		case "Bullet":
			Destroy (c.transform.gameObject);
			life--;
			if (life <= 0) {
				Destroy (transform.gameObject);
			}
			break;
		}
	}

	void Attack() {
		Debug.Log ("Attack!" + Time.time);
	}
}
