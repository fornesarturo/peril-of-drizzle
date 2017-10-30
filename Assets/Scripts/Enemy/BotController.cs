﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

public class BotController : MonoBehaviour {

	public GameObject[] players;
	private static int speed = 5;
	public int life = 100;
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
		if (players.Length > 0) {
			float maxDistance = players.Min(x => (x.transform.position - this.transform.position).sqrMagnitude);
			GameObject closest = players.First(x => (x.transform.position - this.transform.position).sqrMagnitude == maxDistance);

			if(closest != prevClosest) {
				prevClosest = closest;
			}
			isFar = Vector2.Distance (transform.position, closest.transform.position) > 1.1f;
			float direction = (transform.position - prevClosest.transform.position).x;
			if (isFar) {
				animator.SetFloat("Speed", -direction);
				transform.position = Vector2.MoveTowards (transform.position, prevClosest.transform.position, speed * Time.deltaTime);
			} else {
				animator.SetFloat("Speed", 0);
				if (!attackActive) {
                    attackActive = true;
                    StartCoroutine (attackCorroutine (closest));
				}
			}
		}
	}

	void OnTriggerEnter2D(Collider2D c) {
		switch (c.transform.tag) {
		case "Bullet":
			Destroy (c.transform.gameObject);
			life--;
			if (life <= 0) {
				Destroy (transform.gameObject);
				break;
			}
			StartCoroutine(hitCorroutine (0.1f));
			break;
		case "Melee":
			Destroy (c.transform.gameObject);
			life--;
			if (life <= 0) {
				Destroy (transform.gameObject);
				break;
			}
			StartCoroutine(hitCorroutine (0.2f));
			break;
        case "Border":
            Destroy(transform.gameObject);
            break;
        }
	}

    bool Attack(GameObject go) {
        Debug.Log(go.name);
		if (go != null) {
			PlayerController pc = go.GetComponent<PlayerController> ();
			if (pc != null) {
				pc.life = pc.life - 2;
			}
            if(pc.life <= 0) {
                return true;
            }
            else {
                return false;
            }
        }
        else {
            return false;
        }

    }

	IEnumerator attackCorroutine(GameObject go) {
		while (true) {
            animator.SetTrigger("Attack");
            bool isDead = Attack (go);
			if (isDead) {
				attackActive = false;
				yield break;
			}
			yield return new WaitForSeconds(3f);
		}
	}

	IEnumerator hitCorroutine(float seconds) {
		speed = 0;
		yield return new WaitForSeconds (seconds);
		speed = 5;
		yield break;
	}
}
