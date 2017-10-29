﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

public class BotController : MonoBehaviour {

	public GameObject[] players = new GameObject[4];
	private static int speed = 5;
	private static int life = 200;
	private GameObject closest;
	private GameObject prevClosest;
	private bool attackActive = false;
	private bool isFar;
    private Animator animator;
	// Use this for initialization
	void Start () {
        this.animator = this.GetComponent<Animator>();
	}

	// Update is called once per frame
	void Update () {
		players = GameObject.FindGameObjectsWithTag("PlayerTag");
		float maxDistance = players.Min(x => (x.transform.position - this.transform.position).sqrMagnitude);
		GameObject closest = players.First(x => (x.transform.position - this.transform.position).sqrMagnitude == maxDistance);

		if(closest != prevClosest) {
			prevClosest = closest;
		}
		isFar = Vector2.Distance (transform.position, prevClosest.transform.position) > 1f;
        float direction = (transform.position - prevClosest.transform.position).x;
        if (isFar) {
            animator.SetFloat("Speed", -direction);
            transform.position = Vector2.MoveTowards (transform.position, prevClosest.transform.position, speed * Time.deltaTime);
		} else {
            animator.SetFloat("Speed", -direction);
            if (!attackActive) {
                StartCoroutine (attackCorroutine (closest));
				attackActive = true;
			}
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

	void Attack(GameObject go) {
		Debug.Log ("Attack!" + Time.time);
		PlayerController pc = go.GetComponent<PlayerController> ();
		pc.life--;
	}

	IEnumerator attackCorroutine(GameObject go) {
		while (true) {
			yield return new WaitForSeconds(1f);
            animator.SetTrigger("Attack");
            Attack (go);
			if (isFar) {
				attackActive = false;
				yield break;
			}
		}
	}
}
