using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

	public GameObject bullet;
	public GameObject meleeLeft;
	public GameObject meleeRight;
	private Rigidbody2D rb2;
	private static int speed = 10;
    private static float climbSpeed = 8f;
    private static Vector2 jumpVector = new Vector2(0, 15.0f);
    private int jumps = 2;
    private bool rope;
    private static float gravity = 4.0f;
	public int life = 10;
	private int direction = 1;
	private bool specialWait = false;
	private bool standardWait = false;

	private const int SPECIAL_WAIT = 0;
	private const int STANDARD_WAIT = 1;
 
    // Control of character
    public string horizontalControl = "Horizontal_P1";
    public string verticalControl = "Vertical_P1";
    public int playerNo;

    public Sprite[] sprites; // Initial sprite array
    private SpriteRenderer spriteRenderer; // Load sprite
    private bool selectingPlayer; // true while selecting player
    private int characterSpriteNumber; // default to player number
    public RuntimeAnimatorController[] animators; // Initial animator array
    private Animator animator; // Load animator

    private void Awake() {
        rb2 = GetComponent<Rigidbody2D> ();
        rb2.gravityScale = gravity;
        this.characterSpriteNumber = this.playerNo - 1;
    }

    void Start () {
        this.selectingPlayer = true;
        StartCoroutine(this.selectCharacter());
	}

	void Update () {
		if (life <= 0) {
			Die ();
		}
        if (selectingPlayer == false) {
            float h = Input.GetAxisRaw(horizontalControl);
            float v = Input.GetAxisRaw(verticalControl);

			// Normalize controller input
			bool validInput = Mathf.Abs (h) > 0.5f || Mathf.Abs (v) > 0.5f;
			if (validInput && Mathf.Abs (h) > Mathf.Abs (v)) {
				if (h > 0) {
					h = 1;
					v = 0;
				} else if (h < 0) {
					h = -1;
					v = 0;
				}
			} else if (validInput && v > 0) {
				v = 1;
				h = 0;
			} else if (validInput && v < 0) {
				v = -1;
				h = 0;
			} else {
				h = 0;
				v = 0;
			}

            animator.SetFloat("Speed", h);
            rb2.velocity = new Vector2(h * speed, rb2.velocity.y);

            if (h > 0) {
                direction = 1;
            }
            else if (h < 0) {
                direction = -1;
            }

            // If button a is pressed, jump
            if (Input.GetKeyDown("joystick " + playerNo + " button 0") || Input.GetKeyDown(KeyCode.Space)) {
                if (jumps > 0) {
                    jumps--;
                    rb2.velocity = new Vector2(rb2.velocity.x, 0);
                    rb2.AddForce(jumpVector, ForceMode2D.Impulse);
                }
            }

            // Go up rope
            if (rope && Mathf.Abs(v) > 0) {

                rb2.gravityScale = 0;
                rb2.velocity = new Vector2();
                rb2.position = new Vector2(rb2.position.x, rb2.position.y + (v * climbSpeed * Time.deltaTime));
                Physics2D.IgnoreLayerCollision(gameObject.layer, 8, true);
                // grounded = false;
                jumps = 0;
            }

            // Button 'b' is pressed: STANDARD ATTACK.
            if (Input.GetKeyDown("joystick " + playerNo + " button 1") || Input.GetKeyDown(KeyCode.LeftControl)) {
				if (this.characterSpriteNumber == 0 || this.characterSpriteNumber == 1) { // if character with ranged attack
					if (!standardWait) {
						animator.SetTrigger("Attack");
						standardWait = true;
						GameObject bulletClone = Instantiate (bullet, transform.position, transform.rotation) as GameObject;
						bulletClone.GetComponent<Rigidbody2D> ().AddForce (new Vector2 (direction * 40, 0), ForceMode2D.Impulse);
						StartCoroutine (Cooldown (0.25f, STANDARD_WAIT));
					}
				} else {
					animator.SetTrigger("Attack");
					if (direction > 0) {
						GameObject meleeClone = Instantiate (meleeRight, transform.position + new Vector3 (direction, 0f, 0f), transform.rotation) as GameObject;
						StartCoroutine (WaitToDestroy (0.2f, meleeClone));
					} else {
						GameObject meleeClone = Instantiate (meleeLeft, transform.position + new Vector3 (direction, 0f, 0f), transform.rotation) as GameObject;
						StartCoroutine (WaitToDestroy (0.2f, meleeClone));
					}

				}
            }

            // Button 'x' is pressed: SPECIAL.
            if (Input.GetKeyDown("joystick " + playerNo + " button 2") || Input.GetKeyDown(KeyCode.LeftAlt)) {
				if (!specialWait) {
					Special ();
					specialWait = true;
					StartCoroutine (Cooldown (4f, SPECIAL_WAIT));
				}
            }
        }
	}

    private void OnCollisionEnter2D(Collision2D collision) {
        switch (collision.transform.tag) {
            case "Ground":
                jumps = 2;
                break;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        switch(collision.transform.tag) {
		case "Rope":
			rope = true;
			break;
        case "Border":
            Destroy(transform.gameObject);
            break;
        }
    }

    private void OnTriggerExit2D(Collider2D collision) {
        switch (collision.transform.tag) {
            case "Rope":
                rope = false;
                Physics2D.IgnoreLayerCollision(gameObject.layer, 8, false);
                rb2.gravityScale = gravity;
                jumps = 1;
                break;
            case "Border":
                Destroy(transform.gameObject);
                break;
        }
    }

	private void Die() {
		gameObject.tag = "Dead";
		animator.SetBool ("Dead", true);
		// Destroy (transform.gameObject);
		Destroy (this);
	}

	private void Special() {
		animator.SetTrigger("Special");
		if (this.characterSpriteNumber == 1) { // if jose (this is an immigrant joke)
			GameObject bulletClone = Instantiate(bullet, transform.position, transform.rotation) as GameObject;
			bulletClone.GetComponent<Rigidbody2D>().AddForce(new Vector2(direction * 40, 0), ForceMode2D.Impulse);
			GameObject bulletClone2 = Instantiate(bullet, transform.position, transform.rotation) as GameObject;
			bulletClone2.GetComponent<Rigidbody2D>().AddForce(new Vector2(-direction * 40, 0), ForceMode2D.Impulse);
		}
		else if (this.characterSpriteNumber == 0) { // if bob
			GameObject bulletClone = Instantiate(bullet, transform.position, transform.rotation) as GameObject;
			bulletClone.GetComponent<Rigidbody2D>().AddForce(new Vector2(direction * 40, 0), ForceMode2D.Impulse);
			GameObject bulletClone2 = Instantiate(bullet, transform.position-new Vector3(0, 0.2f, 0), transform.rotation) as GameObject;
			bulletClone2.GetComponent<Rigidbody2D>().AddForce(new Vector2(direction * 40, 0), ForceMode2D.Impulse);
		}
		else if (this.characterSpriteNumber == 2) { // if rebecca
			GameObject bulletClone = Instantiate(bullet, transform.position, transform.rotation) as GameObject;
			bulletClone.GetComponent<Rigidbody2D>().AddForce(new Vector2(direction * 80, 0), ForceMode2D.Impulse);
		}
		else if (this.characterSpriteNumber == 3) { // if tyronne
			GameObject bulletClone = Instantiate(bullet, transform.position, transform.rotation) as GameObject;
			bulletClone.GetComponent<Rigidbody2D>().AddForce(new Vector2(direction * 40, 0), ForceMode2D.Impulse);
			StartCoroutine (WaitToDestroy (0.5f, bulletClone));
			GameObject bulletClone2 = Instantiate(bullet, transform.position - new Vector3(0, 0.2f, 0), transform.rotation) as GameObject;
			bulletClone2.GetComponent<Rigidbody2D>().AddForce(new Vector2(direction * 40, 0), ForceMode2D.Impulse);
			StartCoroutine (WaitToDestroy (0.5f, bulletClone2));
			GameObject bulletClone3 = Instantiate(bullet, transform.position + new Vector3(0, 0.2f, 0), transform.rotation) as GameObject;
			bulletClone3.GetComponent<Rigidbody2D>().AddForce(new Vector2(direction * 40, 0), ForceMode2D.Impulse);
			StartCoroutine (WaitToDestroy (0.5f, bulletClone3));
			GameObject bulletClone4 = Instantiate(bullet, transform.position, transform.rotation) as GameObject;
			bulletClone4.GetComponent<Rigidbody2D>().AddForce(new Vector2(direction * 40, 0), ForceMode2D.Impulse);
			StartCoroutine (WaitToDestroy (0.5f, bulletClone4));
		}
	}

	private IEnumerator selectCharacter() {
		this.spriteRenderer = this.gameObject.AddComponent<SpriteRenderer>();
		this.animator = this.gameObject.AddComponent<Animator>();
		this.spriteRenderer.sprite = this.sprites[this.characterSpriteNumber];
		this.animator.runtimeAnimatorController = this.animators[this.characterSpriteNumber];
		while (this.selectingPlayer == true) {
			if (Input.GetKeyDown("joystick " + this.playerNo + " button 5" ) || Input.GetKeyDown(KeyCode.H)) {
				this.characterSpriteNumber = (this.characterSpriteNumber + 1) % 4;
				this.spriteRenderer.sprite = this.sprites[this.characterSpriteNumber];
				this.animator.runtimeAnimatorController = this.animators[this.characterSpriteNumber];
			}
			else if (Input.GetKeyDown("joystick " + this.playerNo + " button 4") || Input.GetKeyDown(KeyCode.G)) {
				this.characterSpriteNumber--;
				if (this.characterSpriteNumber == -1) {
					this.characterSpriteNumber = 3;
				}
				this.spriteRenderer.sprite = this.sprites[this.characterSpriteNumber];
				this.animator.runtimeAnimatorController = this.animators[this.characterSpriteNumber];
			}
			else if (Input.GetKeyDown("joystick " + this.playerNo + " button 0") || Input.GetKeyDown(KeyCode.Y)) {
				this.selectingPlayer = false;
				yield break;
			}
			yield return null;
		}
	}

	private IEnumerator Cooldown(float seconds, int typeOfWait) {
		yield return new WaitForSeconds (seconds);
		switch (typeOfWait) {
		case SPECIAL_WAIT:
			specialWait = false;
			break;
		case STANDARD_WAIT:
			standardWait = false;
			break;
		}
		Debug.Log ("Player " + playerNo + "\'s special ready! (" + Time.time + ")");
		yield break;
	}

	private IEnumerator WaitToDestroy(float seconds, GameObject go) {
		yield return new WaitForSeconds (seconds);
		Destroy (go);
	}
}
