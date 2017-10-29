using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

	public GameObject bullet;
	private Rigidbody2D rb2;
	private static int speed = 10;
    private static float climbSpeed = 8f;
    private static Vector2 jumpVector = new Vector2(0, 15.0f);
    private int jumps = 2;
    // private static bool grounded;
    private bool rope;
    private static float gravity = 4.0f;
	public int life = 10;
	private int direction = 1;
 
    // Control of character
    public string horizontalControl = "Horizontal_P1";
    public string verticalControl = "Vertical_P1";
    //public string jumpControl = "Jump_P1";
    //public string fireControl = "Fire_P1";
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
        if (selectingPlayer == false) {
            float h = Input.GetAxisRaw(horizontalControl);
            float v = Input.GetAxisRaw(verticalControl);
            //float jump = Input.GetAxisRaw(jumpControl);
            //float fire = Input.GetAxisRaw(fireControl);

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

            // If button b is pressed, shot
            if (Input.GetKeyDown("joystick " + playerNo + " button 1") || Input.GetKeyDown(KeyCode.LeftControl)) {
                if (this.characterSpriteNumber == 0 || this.characterSpriteNumber == 1) { // if range
                    GameObject bulletClone = Instantiate(bullet, transform.position, transform.rotation) as GameObject;
                    bulletClone.GetComponent<Rigidbody2D>().AddForce(new Vector2(direction * 40, 0), ForceMode2D.Impulse);
                }
                animator.SetTrigger("Attack");
            }

            // If button x is pressed, shot special
            if (Input.GetKeyDown("joystick " + playerNo + " button 2") || Input.GetKeyDown(KeyCode.LeftAlt)) {
                if (this.characterSpriteNumber == 1) { // if alien
                    GameObject bulletClone = Instantiate(bullet, transform.position, transform.rotation) as GameObject;
                    bulletClone.GetComponent<Rigidbody2D>().AddForce(new Vector2(direction * 40, 0), ForceMode2D.Impulse);
                    GameObject bulletClone2 = Instantiate(bullet, transform.position, transform.rotation) as GameObject;
                    bulletClone2.GetComponent<Rigidbody2D>().AddForce(new Vector2(-direction * 40, 0), ForceMode2D.Impulse);
                }
                else if (this.characterSpriteNumber == 0) { // if human
                    GameObject bulletClone = Instantiate(bullet, transform.position, transform.rotation) as GameObject;
                    bulletClone.GetComponent<Rigidbody2D>().AddForce(new Vector2(direction * 40, 0), ForceMode2D.Impulse);
                    GameObject bulletClone2 = Instantiate(bullet, transform.position-new Vector3(0, 0.2f, 0), transform.rotation) as GameObject;
                    bulletClone2.GetComponent<Rigidbody2D>().AddForce(new Vector2(direction * 40, 0), ForceMode2D.Impulse);
                }
                else if (this.characterSpriteNumber == 2) { // if fem
                    GameObject bulletClone = Instantiate(bullet, transform.position, transform.rotation) as GameObject;
                    bulletClone.GetComponent<Rigidbody2D>().AddForce(new Vector2(direction * 80, 0), ForceMode2D.Impulse);
                }
                else if (this.characterSpriteNumber == 3) { // if n
                    GameObject bulletClone = Instantiate(bullet, transform.position, transform.rotation) as GameObject;
                    bulletClone.GetComponent<Rigidbody2D>().AddForce(new Vector2(direction * 40, 0), ForceMode2D.Impulse);
                    GameObject bulletClone2 = Instantiate(bullet, transform.position - new Vector3(0, 0.2f, 0), transform.rotation) as GameObject;
                    bulletClone2.GetComponent<Rigidbody2D>().AddForce(new Vector2(direction * 40, 0), ForceMode2D.Impulse);
                    GameObject bulletClone3 = Instantiate(bullet, transform.position + new Vector3(0, 0.2f, 0), transform.rotation) as GameObject;
                    bulletClone.GetComponent<Rigidbody2D>().AddForce(new Vector2(direction * 40, 0), ForceMode2D.Impulse);
                    GameObject bulletClone4 = Instantiate(bullet, transform.position, transform.rotation) as GameObject;
                }
                animator.SetTrigger("Special");
            }
        }
	}

    private IEnumerator selectCharacter() {
        this.spriteRenderer = this.gameObject.AddComponent<SpriteRenderer>();
        this.animator = this.gameObject.AddComponent<Animator>();
        this.spriteRenderer.sprite = this.sprites[this.characterSpriteNumber];
        this.animator.runtimeAnimatorController = this.animators[this.characterSpriteNumber];
        while (this.selectingPlayer == true) {
            if (Input.GetKeyDown("joystick " + this.playerNo + " button 5" ) || Input.GetKeyDown(KeyCode.H)) {
                print("right!");
                this.characterSpriteNumber = (this.characterSpriteNumber + 1) % 4;
                this.spriteRenderer.sprite = this.sprites[this.characterSpriteNumber];
                this.animator.runtimeAnimatorController = this.animators[this.characterSpriteNumber];
            }
            else if (Input.GetKeyDown("joystick " + this.playerNo + " button 4") || Input.GetKeyDown(KeyCode.G)) {
                print("left!");
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

    private void OnCollisionEnter2D(Collision2D collision) {
        switch (collision.transform.tag) {
            case "Ground":
                jumps = 2;
                // grounded = true;
                break;
			case "EnemyTag":
				life--;
				if (life == 0) {
					Destroy (transform.gameObject);
				}
				break;
        }
    }

    private void OnCollisionExit2D(Collision2D collision) {
        switch (collision.transform.tag) {
            case "Ground":
                print("Not Grounded");
                // grounded = false;
                break;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        switch(collision.transform.tag) {
            case "Rope":
                print("Touching Rope");
                rope = true;
                break;
		case "EnemyTag":
			life--;
			if (life == 0) {
				Destroy (transform.gameObject);
			}
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
        }
    }
}
