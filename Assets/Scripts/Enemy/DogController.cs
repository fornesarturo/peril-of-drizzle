using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

public class DogController : MonoBehaviour {

	public GameObject[] players;
	private static int speed = 5;
	private int life;
	private GameObject closest;
	private GameObject prevClosest;
	private bool attackActive = false;
	private bool isFar;
	private float direction;
	private Animator animator;
	public GameObject closeRangeAttack;
	public GameObject farRangeAttack;
    public GameObject coin;

	// Use this for initialization
	void Start () {
		this.animator = this.GetComponent<Animator>();
        this.life = 150;
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
			isFar = Vector2.Distance (transform.position, prevClosest.transform.position) > 6f;
			direction = (transform.position - prevClosest.transform.position).x;
			if (isFar) {
				animator.SetFloat("Speed", direction);
				transform.position = Vector2.MoveTowards(transform.position, prevClosest.transform.position, speed * Time.deltaTime);
			}
			else {
				animator.SetFloat("Speed", direction);
				if (!attackActive) {
					StartCoroutine(attackCorroutine(closest));
					attackActive = true;
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
                Instantiate(coin, transform.position, transform.rotation);
				Destroy (transform.gameObject);
				break;
			}
			StartCoroutine(hitCorroutine (0.1f));
			break;
		case "Melee":
			Destroy (c.transform.gameObject);
			life--;
			if (life <= 0) {
                Instantiate(coin, transform.position, transform.rotation);
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

	private IEnumerator closeRange() {
		int looking = 1;
		if (direction > 0) {
			looking = -1;
		}
		GameObject closeCloneA = Instantiate (closeRangeAttack, transform.position + (new Vector3(5 * looking, -1.5f, 0)), transform.rotation, transform) as GameObject;
		GameObject closeCloneB = Instantiate (closeRangeAttack, transform.position + (new Vector3(10 * looking, -1.5f, 0)), transform.rotation, transform) as GameObject;
		GameObject closeCloneC = Instantiate (closeRangeAttack, transform.position + (new Vector3(15 * looking, -1.5f, 0)), transform.rotation, transform) as GameObject;
		yield return new WaitForSeconds(1f);
		Destroy (closeCloneA);
		Destroy (closeCloneB);
		Destroy (closeCloneC);
		yield break;
	}

	private IEnumerator farRange() {
		GameObject farCloneA = Instantiate (farRangeAttack, transform.position + (new Vector3(3, -1f, 0)), transform.rotation, transform) as GameObject;
		farCloneA.GetComponent<Rigidbody2D> ().AddForce (new Vector2(-20, 0), ForceMode2D.Impulse);
		GameObject farCloneB = Instantiate (farRangeAttack, transform.position + (new Vector3(-3, -1f, 0)), transform.rotation, transform) as GameObject;
		farCloneB.GetComponent<Rigidbody2D> ().AddForce (new Vector2(20, 0), ForceMode2D.Impulse);
		yield return new WaitForSeconds(3f);
		Destroy (farCloneA);
		Destroy (farCloneB);
		yield break;
	}

	IEnumerator attackCorroutine(GameObject go) {
		while (true) {
			animator.SetTrigger("Attack");
			bool localFar = Vector2.Distance (transform.position, go.transform.position) > 6.0f;
			if (localFar) {
				animator.SetTrigger ("Attack");
				StartCoroutine (farRange ());
				attackActive = false;
				yield break;
			}
			yield return new WaitForSeconds (0.2f);
			StartCoroutine(closeRange ());
			yield return new WaitForSeconds(1f);
		}
	}

	IEnumerator hitCorroutine(float seconds) {
		speed = 0;
		yield return new WaitForSeconds (seconds);
		speed = 5;
		yield break;
	}
}
