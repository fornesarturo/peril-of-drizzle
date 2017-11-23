using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour {

	public GameObject bullet;
	public GameObject meleeLeft;
	public GameObject meleeRight;
    public GameObject healRadius;
	private Rigidbody2D rb2;
	private static int speed = 10;
    private static float climbSpeed = 8f;
    private static Vector2 jumpVector = new Vector2(0, 15.0f);
    private int jumps = 2;
    private bool rope;
    private static float gravity = 4.0f;
	public int life;
    private Image healthBar;
    private Image specialBar;
    private float maxSpecialTime;
    private int maxLife = 20;
	private int direction = 1;
	private bool specialWait = false;
	private bool standardWait = false;

	private const int SPECIAL_WAIT = 0;
	private const int STANDARD_WAIT = 1;

    public AudioClip[] audioClip;
 
    // Control of character
    public string horizontalControl = "Horizontal_P1";
    public string verticalControl = "Vertical_P1";
    public int playerNo;

    public Sprite[] sprites; // Initial sprite array
    private SpriteRenderer spriteRenderer; // Load sprite
    private int characterSpriteNumber; // default to player number
    public RuntimeAnimatorController[] animators; // Initial animator array
    private Animator animator; // Load animator

    private void Awake() {
        rb2 = GetComponent<Rigidbody2D> ();
        rb2.gravityScale = gravity;
		this.characterSpriteNumber = PlayerPrefs.GetInt("PlayerSprite" + this.playerNo);
		if (this.characterSpriteNumber == -1) {
            transform.tag = "Dead";
            Camera.main.GetComponent<CameraScript>().SearchPlayers();
            Destroy (gameObject);
		} else {
			this.spriteRenderer = this.gameObject.AddComponent<SpriteRenderer>();
			this.animator = this.gameObject.AddComponent<Animator>();
			this.spriteRenderer.sprite = this.sprites[this.characterSpriteNumber];
			this.animator.runtimeAnimatorController = this.animators[this.characterSpriteNumber];
		}
    }

    void Start () {
		this.life = PlayerPrefs.GetInt ("PlayerLife" + this.playerNo);
        this.healthBar = this.transform.Find("PlayerCanvas").Find("HealthBG").Find("Health").GetComponent<Image>();
        this.healthBar.fillAmount = (float)this.life / (float)this.maxLife;
        this.maxSpecialTime = (this.characterSpriteNumber == 1) ? 6f : 4f;
        this.specialBar = this.transform.Find("PlayerCanvas").Find("SpecialBG").Find("Special").GetComponent<Image>();
        this.specialBar.fillAmount = (float)this.maxSpecialTime;
    }

	void Update () {
		if (life <= 0) {
			PlayerPrefs.SetInt ("PlayerLife" + this.playerNo, 0);
			Die ();
		}
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
                PlaySound(0);
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
                    PlaySound(1);
                    animator.SetTrigger("Attack");
					standardWait = true;
					GameObject bulletClone = Instantiate (bullet, transform.position, transform.rotation, transform) as GameObject;
					bulletClone.GetComponent<Rigidbody2D> ().AddForce (new Vector2 (direction * 40, 0), ForceMode2D.Impulse);
					StartCoroutine (Cooldown (0.25f, STANDARD_WAIT));
				}
			} else {
                PlaySound(5);
                animator.SetTrigger("Attack");
				if (direction > 0) {
					GameObject meleeClone = Instantiate (meleeRight, transform.position + new Vector3 (direction, 0f, 0f), transform.rotation, transform) as GameObject;
					StartCoroutine (WaitToDestroy (0.2f, meleeClone));
				} else {
					GameObject meleeClone = Instantiate (meleeLeft, transform.position + new Vector3 (direction, 0f, 0f), transform.rotation, transform) as GameObject;
					StartCoroutine (WaitToDestroy (0.2f, meleeClone));
				}

			}
        }

        // Button 'x' is pressed: SPECIAL.
        if (Input.GetKeyDown("joystick " + playerNo + " button 2") || Input.GetKeyDown(KeyCode.LeftAlt)) {
			if (!specialWait) {
				Special ();
				specialWait = true;
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

    private void DoDamage(int l) {
        life -= l;
        healthBar.fillAmount = (float)this.life / (float)this.maxLife;
        PlayerPrefs.SetInt("PlayerLife" + this.playerNo, life);
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        switch(collision.transform.tag) {
            case "Rope":
			    rope = true;
			    break;
            case "Border":
                Destroy(transform.gameObject);
                break;
		    case "FlameAttack":
			    if ((transform.position - collision.transform.position).x < 0)
				    Knockback (1);
			    else
				    Knockback (-1);
			    DoDamage(1);
			    break;
		    case "JellyAttack":
			    if ((transform.position - collision.transform.position).x < 0)
				    Knockback (1);
			    else
				    Knockback (-1);
			    DoDamage(1);
			    break;
		    case "BotAttack":
			    if ((transform.position - collision.transform.position).x < 0)
				    Knockback (1);
			    else
				    Knockback (-1);
			    DoDamage(2);
			    break;
            case "BotBullet":
                Destroy (collision.transform.gameObject);
                if ((transform.position - collision.transform.position).x < 0)
                    Knockback(1);
                else
                    Knockback(-1);
                DoDamage(1);
                break;
		    case "DogFarAttack":
			    if ((transform.position - collision.transform.position).x < 0)
				    Knockback (1);
			    else
				    Knockback (-1);
			    DoDamage (3);
			    break;
		    case "DogCloseAttack":
			    if ((transform.position - collision.transform.position).x < 0)
				    Knockback (1);
			    else
				    Knockback (-1);
			    DoDamage (4);
			    break;
            case "Coin":
                Destroy(collision.gameObject);
                print("Money");
                Camera.main.GetComponent<CameraScript>().TakeCoin();
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

	public void Knockback(float direction) {
		Debug.Log ("Knockback Magnitude: " + direction);
		float magnitude = 10;
		float xMagnitude = 10;
		if (direction < 0) {
			xMagnitude = magnitude * -1;
		}
		rb2.AddForce (new Vector2 (0, magnitude), ForceMode2D.Impulse);
		rb2.AddForce (new Vector2 (xMagnitude, 0), ForceMode2D.Impulse);
	}

	private void Die() {
        PlaySound(7);
        gameObject.tag = "Dead";
        Camera.main.GetComponent<CameraScript>().SearchPlayers();
        animator.SetBool ("Dead", true);
        Destroy(this.transform.Find("PlayerCanvas").gameObject);
		// Destroy (transform.gameObject);
		Destroy (this);
	}

	private void Special() {
		animator.SetTrigger("Special");
        StartCoroutine(Cooldown(maxSpecialTime, SPECIAL_WAIT));
        if (this.characterSpriteNumber == 1) { // if jose (this is an immigrant joke)
            PlaySound(6);
            GameObject healClone = Instantiate(healRadius, transform.position, transform.rotation) as GameObject;
            StartCoroutine(WaitToDestroy(0.2f, healClone));
            StartCoroutine(Heal(3));
		}
		else if (this.characterSpriteNumber == 0) { // if bob
            PlaySound(2);
            GameObject bulletClone = Instantiate(bullet, transform.position, transform.rotation) as GameObject;
			bulletClone.GetComponent<Rigidbody2D>().AddForce(new Vector2(direction * 40, 0), ForceMode2D.Impulse);
			StartCoroutine (WaitToDestroy (1f, bulletClone));

			GameObject bulletClone2 = Instantiate(bullet, transform.position-new Vector3(0, 0.2f, 0), transform.rotation, transform) as GameObject;
			bulletClone2.GetComponent<Rigidbody2D>().AddForce(new Vector2(direction * 40, 0), ForceMode2D.Impulse);
			StartCoroutine (WaitToDestroy (1f, bulletClone2));
		}
		else if (this.characterSpriteNumber == 2) { // if rebecca
            PlaySound(3);
            StartCoroutine(SniperAttack());
		}
		else if (this.characterSpriteNumber == 3) { // if tyronne
            PlaySound(4);
            GameObject bulletClone = Instantiate(bullet, transform.position, transform.rotation) as GameObject;
			bulletClone.GetComponent<Rigidbody2D>().AddForce(new Vector2(direction * 40, 0), ForceMode2D.Impulse);
			StartCoroutine (WaitToDestroy (0.5f, bulletClone));

			GameObject bulletClone2 = Instantiate(bullet, transform.position - new Vector3(0, 0.2f, 0), transform.rotation, transform) as GameObject;
			bulletClone2.GetComponent<Rigidbody2D>().AddForce(new Vector2(direction * 40, 0), ForceMode2D.Impulse);
			StartCoroutine (WaitToDestroy (0.5f, bulletClone2));

			GameObject bulletClone3 = Instantiate(bullet, transform.position + new Vector3(0, 0.2f, 0), transform.rotation, transform) as GameObject;
			bulletClone3.GetComponent<Rigidbody2D>().AddForce(new Vector2(direction * 40, 0), ForceMode2D.Impulse);
			StartCoroutine (WaitToDestroy (0.5f, bulletClone3));

			GameObject bulletClone4 = Instantiate(bullet, transform.position, transform.rotation, transform) as GameObject;
			bulletClone4.GetComponent<Rigidbody2D>().AddForce(new Vector2(direction * 40, 0), ForceMode2D.Impulse);
			StartCoroutine (WaitToDestroy (0.5f, bulletClone4));
		}
	}

	private IEnumerator Cooldown(float seconds, int typeOfWait) {
		switch (typeOfWait) {
		case SPECIAL_WAIT:
            this.specialBar.fillAmount = 0;
            int intSecs = (int)seconds;
            for (int i = 0; i <= intSecs; i++) {
                yield return new WaitForSeconds(1);
                this.specialBar.fillAmount = i / (float)this.maxSpecialTime;
            }
            specialWait = false;
			break;
		case STANDARD_WAIT:
                yield return new WaitForSeconds(seconds);
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

    public void HealPlayer(int qty) {
        this.life += qty;
        this.healthBar.fillAmount = (float)this.life / (float)this.maxLife;
    }

    private IEnumerator Heal(int seconds) {
        Collider2D[] colliderPlayers = Physics2D.OverlapCircleAll(this.transform.position, 5);
        List<GameObject> players = new List<GameObject>();
        foreach (Collider2D coll in colliderPlayers) {
            if(coll.transform.tag == "PlayerTag") {
                print(coll.transform.name);
                players.Add(coll.transform.gameObject);
            }
        }
        for(int i = 0; i < seconds; i++) {
            yield return new WaitForSeconds(1f);
            foreach(GameObject player in players) {
                PlayerController script = player.GetComponent<PlayerController>();
                script.HealPlayer(1);
            }
        }
        yield break;
    }

    private IEnumerator SniperAttack() {
        GameObject bulletClone = Instantiate(bullet, transform.position, transform.rotation) as GameObject;
        bulletClone.GetComponent<Rigidbody2D>().AddForce(new Vector2(direction * 80, 0), ForceMode2D.Impulse);
        StartCoroutine(WaitToDestroy(2f, bulletClone));
        yield return new WaitForSeconds(.15f);
        GameObject bulletClone2 = Instantiate(bullet, transform.position, transform.rotation, transform) as GameObject;
        bulletClone2.GetComponent<Rigidbody2D>().AddForce(new Vector2(direction * 80, 0), ForceMode2D.Impulse);
        StartCoroutine(WaitToDestroy(2f, bulletClone2));
        yield return new WaitForSeconds(.15f);
        GameObject bulletClone3 = Instantiate(bullet, transform.position, transform.rotation, transform) as GameObject;
        bulletClone3.GetComponent<Rigidbody2D>().AddForce(new Vector2(direction * 80, 0), ForceMode2D.Impulse);
        StartCoroutine(WaitToDestroy(2f, bulletClone3));
        yield break;
    }

    void PlaySound (int clip) {
        print("Sound: " + clip);
        AudioSource audioS = this.GetComponent<AudioSource>();
        audioS.clip = audioClip[clip];
        audioS.Play();
    }
}
