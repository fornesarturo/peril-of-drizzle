using System.Collections;
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
	private float direction;
    private Animator animator;
	private GameObject attackHitbox;
    private bool canShoot;
    public GameObject bullet;


    // Use this for initialization
    void Start () {
        this.animator = this.GetComponent<Animator>();
		this.attackHitbox = transform.GetChild (0).gameObject;
		this.attackHitbox.SetActive (true);
        this.canShoot = false;
        StartCoroutine(Cooldown(4f));
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
			direction = (transform.position - prevClosest.transform.position).x;
			if (isFar) {
				animator.SetFloat("Speed", -direction);
				transform.position = Vector2.MoveTowards (transform.position, prevClosest.transform.position, speed * Time.deltaTime);
                if(canShoot) {
                    animator.SetTrigger("Attack");
					GameObject bulletClone = Instantiate(bullet, transform.position, transform.rotation, transform) as GameObject;
                    bulletClone.GetComponent<Rigidbody2D>().AddForce(new Vector2(-1*(direction/Mathf.Abs(direction)) * 40, 0), ForceMode2D.Impulse);
                    canShoot = false;
                    StartCoroutine(Cooldown(2.5f));
                }
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

	IEnumerator attackCorroutine(GameObject go) {
		while (true) {
			this.attackHitbox.SetActive (true);
			animator.SetTrigger("Attack");
			bool localFar = Vector2.Distance (transform.position, go.transform.position) > 1.1f;
			if (localFar) {
				attackActive = false;
				this.attackHitbox.SetActive (false);
				yield break;
			}
			yield return new WaitForSeconds (0.2f);
			this.attackHitbox.SetActive (false);
			yield return new WaitForSeconds(1f);
		}
	}

	IEnumerator hitCorroutine(float seconds) {
		speed = 0;
		yield return new WaitForSeconds (seconds);
		speed = 5;
		yield break;
	}

    IEnumerator Cooldown(float seconds) {
        yield return new WaitForSeconds(seconds);
        canShoot = true;
        yield break;
    }
}
