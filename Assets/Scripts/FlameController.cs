using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlameController : MonoBehaviour {

	public Transform player;
	private static int speed = 5;
	private static int life = 3;
	// Use this for initialization
	void Start () {
		player = GameObject.FindGameObjectWithTag ("PlayerTag").transform;
	}
	
	// Update is called once per frame
	void Update () {
		if (Vector2.Distance (transform.position, player.position) > 1f) {
			transform.position = Vector2.MoveTowards (transform.position, player.position, speed * Time.deltaTime);
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
}
